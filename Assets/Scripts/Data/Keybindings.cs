using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "Settings/Key Bindings", order = 0)]
public class Keybindings : DataStorage
{
    [System.Serializable]
    public class KeybindingPair
    {
        public KeybindingActions KeybindingAction;
        public KeyCode KeyCode;
    }

    public KeybindingPair[] KeybindingPairs;
}