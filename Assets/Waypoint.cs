using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public List<GameObject> other; // The other part of the waypoint, if this is B, the other will be A
	public List<GameObject> parentWaypoints; // The waypoint that comes before this one

	void OnDrawGizmos(){
		// Check to see if there's another waypoint
		if(other.Count > 0){
			// Check to see if there is only one other waypoint
			if(other.Count == 1){
				// Ensure the other waypoint is still valid
				if(other[0] != null){
					Gizmos.color = Color.white; // Set the color to white
					Gizmos.DrawLine(transform.position, other[0].transform.position); // Draw a line between this waypoint, and the other waypoint
				}
			}
			// If there's more than 1 other waypoint
			else{
				// Loop through each other waypoint
				foreach(GameObject lineObject in other){
					// Ensure the current other waypoint is valid
					if(lineObject != null){
						Gizmos.color = Color.blue; // Set the color to blue
						Gizmos.DrawLine(transform.position, lineObject.transform.position); // Draw a line between this waypoint, and the other one
					}
				}
			}
		}
	}

	// This will create a new waypoint
	public GameObject CreateNextWaypoint(){
		GameObject nextWaypoint = new GameObject("Waypoint"); // Create a new gameobject for the waypoint
		nextWaypoint.transform.position = transform.position; // Move the new waypoint to the current waypoints position
		nextWaypoint.AddComponent<Waypoint>(); // Turn the new gameobject into a waypoint
		nextWaypoint.GetComponent<Waypoint>().other = new List<GameObject>(); // Create a list for possible other waypoints
		nextWaypoint.GetComponent<Waypoint>().parentWaypoints = new List<GameObject>() {gameObject}; // Set the parent waypoint
		List<GameObject> newOther = new List<GameObject>(); // Create a new list to fill with the waypoints already added to this object
		// Loop through the waypoints already attached to this object
		for(int i = 0;i<other.Count;i+=1){
			// Ensure the waypoint is valid
			if(other[i] != null){
				newOther.Add(other[i]); // Add the waypoint to our new list
			}
		}
		newOther.Add(nextWaypoint); // Add the new waypoint to the list of waypoints
		other = newOther; // Replace our old list with the new one
		return nextWaypoint; // Return the new waypoint
	}
}
