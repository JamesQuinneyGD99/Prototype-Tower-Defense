using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject toSpawn; // The object that is spawned
	float nextSpawn; // When the next object will spawn
	[SerializeField]
	float spawnDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
		nextSpawn = Time.time + spawnDelay; // Set when the next object will spawn
    }

    // Update is called once per frame
    void Update()
    {
		// Check if it's time to spawn a new object
		if(nextSpawn < Time.time){
			nextSpawn = Time.time + spawnDelay; // We set when the next object will spawn

			GameObject spawnedObject = Instantiate(toSpawn,transform.position,Quaternion.identity); // We create the object
			// If the object isn't reversed
			if(!spawnedObject.GetComponent<RailObject>().reversed){
				spawnedObject.GetComponent<RailObject>().currentWaypoint = GetComponent<Waypoint>().other[Random.Range(0, GetComponent<Waypoint>().other.Count)]; // Set where the first waypoint will be
			}
			// If the object is reversed
			else{
				spawnedObject.GetComponent<RailObject>().currentWaypoint = GetComponent<Waypoint>().parentWaypoints[Random.Range(0, GetComponent<Waypoint>().parentWaypoints.Count)]; // Set where the first waypoint will be
			}
		}
    }
}
