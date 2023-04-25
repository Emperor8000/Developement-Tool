using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(sc_GunSelector))]
class sc_GunSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MonoBehaviour monoBev = (MonoBehaviour)target;
        sc_PlayerAction playerAction = monoBev.GetComponent<sc_PlayerAction>();

        if (GUILayout.Button("Implement") && playerAction == null)
        {
            Debug.Log("It's alive: " + target.name);
            playerAction = monoBev.gameObject.AddComponent<sc_PlayerAction>();
            playerAction.GunSelector = monoBev.GetComponent<sc_GunSelector>(); 
        }
    }
}
