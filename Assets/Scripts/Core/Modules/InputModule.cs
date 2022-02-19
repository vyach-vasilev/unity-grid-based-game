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
        Debug.Log("Execute: " + Id);
        InputManager.Create(_keyBindings);
    }
}