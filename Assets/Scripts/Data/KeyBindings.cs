using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "Settings/Key Bindings", order = 0)]
public class KeyBindings : DataStorage
{
    public KeyCode Select = KeyCode.Mouse0;
    public KeyCode Action = KeyCode.Mouse1;
    public KeyCode PrepareToAttack = KeyCode.Space;
    public KeyCode Deselect = KeyCode.Escape;
    public KeyCode Minimap = KeyCode.M;
    public KeyCode Highlighting = KeyCode.LeftShift;
}