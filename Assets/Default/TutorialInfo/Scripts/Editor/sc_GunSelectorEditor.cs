using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(sc_GunSelector))]
//[CanEditMultipleObjects]
class sc_GunSelectorEditor : Editor
{
    //serialized variables from the gun selector script
    SerializedProperty ActiveGunName;
    SerializedProperty GunParent;
    SerializedProperty Guns;
    

    sc_GunSelector ThisScript = null;
    void OnEnable()
    {
        ActiveGunName = serializedObject.FindProperty("ActiveGunName");
        GunParent = serializedObject.FindProperty("GunParent");
        Guns = serializedObject.FindProperty("Guns");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); //update object in the game

        EditorGUILayout.PropertyField(ActiveGunName);
        GenerateTooltip("The name of the gun you want to be active");

        EditorGUILayout.PropertyField(GunParent);
        GenerateTooltip("The object whose transform is used when spawning weapons (note: weapon spawn variables add to this transform)");

        EditorGUILayout.PropertyField(Guns);
        GenerateTooltip("List of weapons: must add all weapons you want this object to use in order for it to work right");

        

        //Private stuff, automatically filled
        MonoBehaviour monoBev = (MonoBehaviour)target;
        ThisScript = monoBev.GetComponent<sc_GunSelector>();
        sc_PlayerAction playerAction = monoBev.GetComponent<sc_PlayerAction>();

        if (GUILayout.Button("Implement") && playerAction == null)
        {
            playerAction = monoBev.gameObject.AddComponent<sc_PlayerAction>();
            playerAction.GunSelector = ThisScript;


            GameObject defaultPivot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ThisScript.GunParent = defaultPivot.transform;
        }
        GenerateTooltip("Press to implement some of the necessary elements for this object to use weapons; does nothing if playerAction componenet is already setup");

        
        serializedObject.ApplyModifiedProperties(); //update component in the editor
    }

    public void GenerateTooltip(string text)
    {
        var propRect = GUILayoutUtility.GetLastRect();
        GUI.Label(propRect, new GUIContent("", text));
    }
}
