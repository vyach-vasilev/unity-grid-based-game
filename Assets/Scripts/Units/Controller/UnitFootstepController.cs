using UnityEngine;

public class UnitFootstepController: MonoBehaviour
{
    private const int MaxVisibleSteps = 4;
    [SerializeField] private DataProvider _dataProvider;
    [SerializeField] private Transform _leftMark;
    [SerializeField] private Transform _rightMark;
    
    public void LeftStep()
    {
        if (_dataProvider.Footsteps.Count >= MaxVisibleSteps)
            _dataProvider.Footsteps.Dequeue();
        
        var customTransform = FormData(_leftMark);
        _dataProvider.Footsteps.Enqueue(customTransform);
    }

    public void RightStep()
    {
        if (_dataProvider.Footsteps.Count >= MaxVisibleSteps)
            _dataProvider.Footsteps.Dequeue();
        
        var customTransform = FormData(_rightMark);
        _dataProvider.Footsteps.Enqueue(customTransform);
    }

    private void OnDisable()
    {
        _dataProvider.Footsteps.Clear();
    }

    private CustomTransform FormData(Transform markTransform)
    {
        var pos = transform.TransformPoint(markTransform.localPosition);
        var customTransform = new CustomTransform
        {
            Position = new Vector3(pos.x, 0.01f, pos.z),
            Rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0),
            Scale = new Vector3(0.25f, 0.025f, 0.25f)
        };
        return customTransform;
    }
}