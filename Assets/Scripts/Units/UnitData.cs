﻿using UnityEngine;

[CreateAssetMenu(fileName = "Unit Settings", menuName = "Settings/Unit Settings", order = 0)]
public class UnitData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _unit;
    [SerializeField] private GameObject _preview;
    [SerializeField] private UnitType _type;
    [SerializeField] private Vector3 _position;
    
    public string Name => _name;
    public GameObject Unit => _unit;
    public GameObject Preview => _preview;
    public UnitType Type => _type;
    public Vector3 Position => _position;
}