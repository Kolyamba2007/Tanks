using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerLayout : Editor
{
    private EnemyController Target;

    private readonly float _intervalMin = 1f;
    private readonly float _intervalMax = 10f;
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

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(" ");
        EditorGUILayout.LabelField(_intervalMin.ToString());
        EditorGUILayout.LabelField(_intervalMax.ToString(), GUILayout.MaxWidth(15));
        EditorGUILayout.EndHorizontal();     
        
        EditorGUILayout.MinMaxSlider(_movementIntervalContent, ref Target.MovementIntervalMin, ref Target.MovementIntervalMax, _intervalMin, _intervalMax);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowTracking"));
        serializedObject.ApplyModifiedProperties();        
    }
}
#endif