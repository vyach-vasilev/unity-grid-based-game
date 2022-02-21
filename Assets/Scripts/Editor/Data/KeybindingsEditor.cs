using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Keybindings))]
public class KeybindingsEditor: Editor
{
    private const string KeybindingActionName = "KeybindingAction";
    private const string KeyCodeName = "KeyCode";

    private Keybindings _target;
    private SerializedProperty _keybindingPairs;
    private ReorderableList _reorderableList;

    private void OnEnable()
    {
        _target = (Keybindings)target;
        _keybindingPairs = serializedObject.FindProperty(nameof(_target.KeybindingPairs));
        _reorderableList = new ReorderableList(serializedObject, _keybindingPairs, true, true, true, true);
        _reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Keybindings");
        _reorderableList.drawElementCallback = (rect, index, active, focused) =>
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
        
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, Screen.width / 2 - 25, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative(KeybindingActionName),
                GUIContent.none
            );
            EditorGUI.PropertyField(
                new Rect(rect.x + Screen.width / 2 - 25, rect.y, Screen.width / 2 - 25, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative(KeyCodeName),
                GUIContent.none
            ); 
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update ();
        _reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties ();
    }
}