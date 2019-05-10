using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailObject : MonoBehaviour
{
	[SerializeField]
	float searchDistance = 0.5f; // The distance the object will search for a waypoint within
	public GameObject currentWaypoint; // The waypoint the object is currently moving towards
	[SerializeField]
	Rigidbody2D rb; // The rigidbody attached to the object
	[SerializeField]
	float speed = 1.0f;
	public float speedMultiplier = 1.0f; // This is the speed of the tower when influenced by other scripts.
	[SerializeField]
	bool destroyOnReach; // Whether the object destroys itself when it reaches its final waypoint
	public bool lookInMoveDirection = true; // Whether this object should look in the direction it is moving
	public bool reversed; // If reversed, the object will go backwards

    // Update is called once per frame
    void Update()
    {
		// Check to make sure there is a waypoint to move to
		if(currentWaypoint != null){
			// If we have not yet reached our waypoint
			if(Vector3.Distance(currentWaypoint.transform.position, transform.position) > searchDistance){
				// Check if the object should look where it's going
				if(lookInMoveDirection){
					// To Do
				}
				rb.velocity = Vector3.Normalize(currentWaypoint.transform.position - transform.position) * speed * speedMultiplier; // Move towards the waypoint
			}
			// If we have reached our waypoint
			else{
				List<GameObject> others; // This is where we will store possible waypoints
				// Check if the object is reversed
				if(reversed){
					others = currentWaypoint.GetComponent<Waypoint>().parentWaypoints; // We store all of the possible waypoints if the object is reversed
				}
				else{
					others = currentWaypoint.GetComponent<Waypoint>().other; // We store all of the possible waypoints if the object is not reversed
				}

				// Check to see if there is a new waypoint to move to
				if(others.Count > 0){
					transform.position = currentWaypoint.transform.position; // Stops objects from deviating from the track
					currentWaypoint = others[Random.Range(0,others.Count)]; // We move to a new waypoint at random
				}
				// If there are no waypoints to move to
				else{
					transform.position = currentWaypoint.transform.position; // Ensure we stop exactly on our waypoint
					rb.velocity = Vector3.zero; // Stop our object
					GameObject savedWaypoint = currentWaypoint; // This allows us to tell the object which waypoint it reached after it has reached it
					currentWaypoint = null; // We tell the object that it no longer needs to move towards the waypoint
					OnReachEnd(savedWaypoint); // Tell the object it has reached its waypoint
				}
			}
		}
    }

	// When the object reaches its final waypoint
	void OnReachEnd(GameObject waypoint){
		// Check if the object should be destroyed at the end
		if(destroyOnReach){
			Destroy(gameObject); // Destroy the object
		}
		else{
			Vector2 pos = transform.position; // We store the current position of the object
			transform.position = new Vector2(pos.x + Random.Range(-searchDistance,searchDistance),pos.y + Random.Range(-searchDistance, searchDistance)); // We offset the position by a random value
		}
	}
}
