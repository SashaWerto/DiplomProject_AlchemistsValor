using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(HandsManipulator))]
public class ItemFunctionalPosSet : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("!PLAYMODE! - Set Default Position"))
        {
            var handsManipulator = (HandsManipulator)target;
            EditorUtility.SetDirty(handsManipulator);
        }
    }
}
