using UnityEngine;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Data/Unit", order = 0)]
public class UnitData : DataStorage
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _unit;
    [SerializeField] private GameObject _preview;
    [SerializeField] private UnitType _type;
    [SerializeField] private Vector3 _position;
    [SerializeField, Range(1, 20)] private float _speed = 5;
    
    public string Name => _name;
    public GameObject Unit => _unit;
    public GameObject Preview => _preview;
    public UnitType Type => _type;
    public Vector3 Position => _position;
    public float Speed => _speed;
}