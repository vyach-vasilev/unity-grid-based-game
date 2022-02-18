using UnityEngine;

public class InputModule : Module<DataStorage, ModuleType>
{
    private readonly KeyBindings _keyBindings;
    
    public InputModule(ModuleType id, KeyBindings keyBindings) : base(id)
    {
        _keyBindings = keyBindings;
    }
    
    public override void Execute(DataStorage data)
    {
        Debug.Log("Activate " + this);
        InputManager.Create(_keyBindings);
    }
}