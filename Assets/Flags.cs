using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{
	public bool shootable; // Whether the object can be shot at
	public string alliance; // This is the team the object fights for (Stops people shooting their own team)
	public float health; // The health for this object
	public float maxHealth = 100; // The starting/max health for the object
	public static List<GameObject> shootables; // This is everything that can be shot in the scene

	// Called just before the first frame update
	void Start(){
		// We check if this game object can be shot
		if(shootable){
			// We check to see if a list hasn't been created yet for shootables
			if(shootables == null){
				shootables = new List<GameObject>(); // We start a list for our shootables
			}

			shootables.Add(gameObject); // We add this game object to shootables
		}

		health = maxHealth; // All objects start with full health
	}

	// This takes health and calls the necessary functions
	public void TakeDamage(float amount, GameObject inflictor){
		health = Mathf.Clamp(health - amount, 0.0f, maxHealth); // We reduce the health of the object

		// We check to see if the object has lost all of its health
		if(health == 0.0f){
			OnDeath(); // We tell the object that it has just died
		}
		// If the object is still alive
		else{
			OnTakeDamage(amount, inflictor); // We tell the object that it has been damaged
		}
	}

	// Called when the object runs out of health
	void OnDeath(){
		Destroy(gameObject); // The object destroys itself
	}

	// Called when the object has taken damage, but is still alive
	void OnTakeDamage(float amount, GameObject inflictor){
		// Nothing for now
	}

	// Called just as the object is about the be destroyed
	void OnDestroy(){
		// We check if this gameobject can be shot
		if(shootable){
			shootables.Remove(gameObject); // We state that this object can no longer be shot (because it's going to be destroyed)
		}
	}
}
