using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerController playerController = (PlayerController)target;

        GUILayout.Label("Player Settings");
         playerController.MoveSpeed = EditorGUILayout.Slider("Player Move Speed", playerController.MoveSpeed, 0, 10);
        EditorGUILayout.Space();
        playerController.MinVelocity = EditorGUILayout.Slider("Player Min Velocity", playerController.MinVelocity, 0, -10);
        EditorGUILayout.Space();
        playerController.SprintMax = EditorGUILayout.Slider("Player Sprint Max", playerController.SprintMax, 0, 10);
        EditorGUILayout.Space();
        playerController.JumpForce = EditorGUILayout.Slider("Jump Force", playerController.JumpForce, 0, 100);
        EditorGUILayout.Space();
        playerController.WetTime = EditorGUILayout.Slider("Wet time", playerController.WetTime, 0, 100);
    }
}
