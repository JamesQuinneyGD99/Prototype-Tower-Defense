using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	public string alliance; // This is to ensure it doesn't harm its own team
	public float damage; // The amount of damage to deal
	public GameObject owner; // The tower which spawned the projectile

	// When the bullet touches something
	void OnTriggerEnter2D(Collider2D collider){
		// Check if the object can be killed
		if(collider.gameObject.GetComponent<Flags>()){
			Flags objectFlags = collider.gameObject.GetComponent<Flags>();
			// Ensure it isn't harming teammates
			if(objectFlags.alliance != alliance){
				objectFlags.TakeDamage(damage,owner);
				Destroy(gameObject); // Remove the projectile
			}
		}
	}
}
