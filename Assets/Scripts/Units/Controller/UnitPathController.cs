using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitPathController
{
    private readonly Transform _transform;
    private readonly List<Vector3> _retracedPath = new();
    private readonly UnitController _unitController;
    private readonly float _speed;

    private int _targetIndex;
    private List<Vector3> _path;
    private bool _isMoving;

    public List<Vector3> Path => _path;
    public List<Vector3> WaypointsPath => RetracedPath();
    public List<Vector3> AvailablePath => GetAvailablePath();
    public bool IsMoving => _isMoving;
    
    public UnitPathController(UnitController unitController, Transform transform, float speed)
    {
        _unitController = unitController;
        _transform = transform;
        _speed = speed;
    }

    public void OnMoved(Vector3 destination)
    {
        var canMove = 
            !_isMoving &&
            !_unitController.InAttack;
        
        if (canMove)
        {
            PathRequestManager.RequestPath(_transform.position, destination, OnPathFound);
        }
    }
    
    private void OnPathFound(List<Vector3> newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            _retracedPath.Clear();
            _retracedPath.AddRange(newPath);
            _targetIndex = 0;

            if (!_unitController.View.Selected)
            {
                _path.Clear();
                return;
            }

            if (InputManager.Instance.CanMove)
            {
                _isMoving = true;
                FollowPath();
            }
        }
    }

    private async void FollowPath()
    {
        var currentWaypoint = _path[0];
        while (true)
        {
            if (!_transform)
            {
                break;
            }
            
            if (_transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Count)
                {
                    break;
                }
                currentWaypoint = _path[_targetIndex];
            }
            
            currentWaypoint.y = _transform.position.y;
            var targetDir = currentWaypoint - _transform.position;
            var step = _speed * Time.deltaTime;
            var newDir = Vector3.RotateTowards(_transform.forward, targetDir, step, 0.0f);
            _transform.rotation = Quaternion.LookRotation(newDir);
            _transform.position = Vector3.MoveTowards(_transform.position, currentWaypoint, _speed * Time.deltaTime);

            await Task.Yield();
        }

        _isMoving = false;
    }

    private List<Vector3> RetracedPath()
    {
        for (int i = 0; i < _retracedPath.Count; i++)
        {
            if (_unitController.View.Position.XZ() == _retracedPath[i].XZ())
            {
                _retracedPath.RemoveAt(i);
                break;
            }
        }
       
        return _retracedPath;
    }
    
    private List<Vector3> GetAvailablePath()
    {
        var list = new List<Vector3>();
        list.Add(_path[0]);
        return list;
    }
}