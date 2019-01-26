using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var player = target as PlayerController;
        player.RenderEditorGUI();
        base.OnInspectorGUI();
    }

}