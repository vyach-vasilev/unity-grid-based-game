using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UnitPathController
{
    private readonly InputManager _inputManager;
    private readonly Transform _transform;
    private readonly IUnitView _view;
    private readonly List<Vector3> _retracedPath = new();
    
    private IUnitView _selectedView;
    private int _targetIndex;
    private List<Vector3> _path;
    
    private float _speed;
    private bool _isMoving;

    public List<Vector3> Path => RetracedPath();
    
    public UnitPathController(InputManager inputManager, IUnitView view, Transform transform)
    {
        _inputManager = inputManager;
        _view = view;
        _transform = transform;
    }

    public void Update(IUnitView selectedView, float speed)
    {
        _selectedView = selectedView;
        _speed = speed;
    }
    
    public void OnMoved(Vector3 targetPosition)
    {
        if ((_view == _selectedView) && _view.Selected && !_isMoving)
        {
            PathRequestManager.RequestPath(_transform.position, targetPosition, OnPathFound);
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

            if (!_view.Selected)
            {
                _path.Clear();
                return;
            }

            if (Input.GetMouseButtonDown(1) && _inputManager.IsMoveLocked)
            {
                _isMoving = true;
                FollowPath();
            }
        }
    }

    private async void FollowPath()
    {
        _inputManager.IsMoveLocked = false;
        Vector3 currentWaypoint = _path[0];
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

            var targetDir = currentWaypoint - _transform.position;
            var step = _speed * Time.deltaTime;
            var newDir = Vector3.RotateTowards(_transform.forward, targetDir, step, 0.0f);
            _transform.rotation = Quaternion.LookRotation(newDir);
            _transform.position = Vector3.MoveTowards(_transform.position, currentWaypoint, _speed * Time.deltaTime);

            await Task.Yield();
        }
        _inputManager.IsMoveLocked = true;
        _isMoving = false;
    }

    private List<Vector3> RetracedPath()
    {
        for (int i = 0; i < _retracedPath.Count; i++)
        {
            if (_view.Position == _retracedPath[i])
            {
                _retracedPath.RemoveAt(i);
                break;
            }
        }
       
        return _retracedPath;
    }
}