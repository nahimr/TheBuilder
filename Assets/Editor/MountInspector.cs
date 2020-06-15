using Items;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Mount), true)]
public class MountInspector : Editor{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var myTarget = (Mount) target;

        myTarget.id = (ushort) Mathf.Clamp(EditorGUILayout.IntField("ID",myTarget.id), 0, ushort.MaxValue);
        myTarget.typeOfMount = (Mount.Type) EditorGUILayout.EnumPopup("Type Of Mount", myTarget.typeOfMount);
        if (myTarget.typeOfMount != Mount.Type.Dynamic) return;
        myTarget.optionsOfMount = (Mount.Options) EditorGUILayout.EnumPopup("Options", myTarget.optionsOfMount);
        switch (myTarget.optionsOfMount)
        {
            case Mount.Options.VelocityUpgrader:
                myTarget.velocitySpeed = (ushort) Mathf.Clamp(EditorGUILayout.IntField("Velocity Speed",myTarget.velocitySpeed), 0, ushort.MaxValue);
                break;
            case Mount.Options.Jetpack:
                myTarget.jetpackForce = (ushort) Mathf.Clamp(EditorGUILayout.IntField("Jetpack Speed",myTarget.jetpackForce), 0, ushort.MaxValue);
                myTarget.jetpackAmount = (ushort) Mathf.Clamp(EditorGUILayout.IntField("Jetpack Amount",myTarget.jetpackAmount), 0, ushort.MaxValue);
                break;
            

        }
        serializedObject.ApplyModifiedProperties();
        if (!GUI.changed) return;
        EditorUtility.SetDirty(myTarget);
        EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
        
    }
} 

[CustomEditor(typeof(Heart), true)]
public class HeartInspector : Editor{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var myTarget = (Heart) target;
        
        myTarget.random = EditorGUILayout.Toggle("Is Random ?", myTarget.random);
            
        if (myTarget.random)
        {
            myTarget.minHealth = (ushort) Mathf.Clamp(EditorGUILayout.IntField("Minimum Health",myTarget.minHealth), 0, ushort.MaxValue);
            myTarget.maxHealth = (ushort) Mathf.Clamp(EditorGUILayout.IntField("Maximum Health",myTarget.maxHealth), 0, ushort.MaxValue);
        }
        else
        {
            myTarget.health = (ushort) Mathf.Clamp(EditorGUILayout.IntField("Health",myTarget.health), 0, ushort.MaxValue);
        }

        serializedObject.ApplyModifiedProperties();
        if (!GUI.changed) return;
        EditorUtility.SetDirty(myTarget);
        EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
    }
} 