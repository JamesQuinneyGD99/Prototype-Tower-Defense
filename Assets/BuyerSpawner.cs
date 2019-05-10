using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerSpawner : MonoBehaviour
{
	[SerializeField]
	Vector2 spawnAt = new Vector2(0,0); // Where the item spawns (stops it from spawning outside of the map)
	[SerializeField]
	int price = 500;
	[SerializeField]
	GameObject buyable; // This is the object that spawns when you buy it

	// When the player clicks the object
	void OnMouseDown(){
		// Check if the player can afford the object
		if(Money.amount >= price){
			Money.Add(-price); // Take the price away from the player
			GameObject spawnedObject = Instantiate(buyable, spawnAt, Quaternion.identity); // Spawn the object
			spawnedObject.GetComponent<MouseMoveable>().PickUp(); // Pick up the object
		}
	}
}
