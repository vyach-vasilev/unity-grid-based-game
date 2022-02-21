using UnityEngine;

public class InputModule : Module<DataStorage, ModuleType>
{
    private readonly Keybindings _keybindings;
    
    public InputModule(ModuleType id, Keybindings keybindings) : base(id)
    {
        _keybindings = keybindings;
    }
    
    public override void Execute(DataStorage data)
    {
        Debug.Log("Execute: " + Id);
        InputManager.Create(_keybindings);
    }
}