using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		Waypoint waypointScript = (Waypoint)target;
		// Create a new button in the editor
		if(GUILayout.Button("create next waypoint")){
			Selection.activeGameObject = waypointScript.CreateNextWaypoint(); // Create the next waypoint and set it as the active object
		}
	}
}
