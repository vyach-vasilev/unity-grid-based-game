using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "Settings/Key Bindings", order = 0)]
public class KeyBindings : DataStorage
{
    public KeyCode Select = KeyCode.Mouse0;
    public KeyCode Action = KeyCode.Mouse1;
    
    public KeyCode Skill1 = KeyCode.Alpha1;
    public KeyCode Skill2 = KeyCode.Alpha2;
    public KeyCode Skill3 = KeyCode.Alpha3;
    
    public KeyCode Deselect = KeyCode.Escape;
    public KeyCode Minimap = KeyCode.M;
    public KeyCode Highlighting = KeyCode.LeftShift;
}