using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerLayout : Editor
{
    private EnemyController Target;

    private readonly GUIContent _movementIntervalContent = new GUIContent("Movement Interval");

    private void OnEnable()
    {
        Target = (EnemyController)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_health"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_reload"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_projectile"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Bot Options", EditorStyles.boldLabel);
        EditorGUILayout.MinMaxSlider(_movementIntervalContent, ref Target.MovementIntervalMin, ref Target.MovementIntervalMax, 1, 10f);
        serializedObject.ApplyModifiedProperties();        
    }
}
#endif