using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
	float nextAttack; // This is the time the soldier can attack again
	[SerializeField]
	float damagePerShot = 5; // The amount of damage per shot...
	[SerializeField]
	float shotCooldown = 1; // The time in seconds between each attack
	[SerializeField]
	float range = 2; // The range this soldier can attack
	[SerializeField]
	GameObject projectile; // The projectile fired by this object, if left null the object will attack anything within range instantly
	RailObject railScript; // This is the rail script, we can use this to slow the soldier down when attacking
	Flags flags; // These are the flags attached to this object

    // Start is called before the first frame update
    void Start()
    {
		// We check if this soldier moves on rails
		if(GetComponent<RailObject>()){
			railScript = GetComponent<RailObject>(); // We store the rail script
		}

		flags = GetComponent<Flags>(); // We find the flags for this object
    }

    // Update is called once per frame
    void Update()
    {
		bool slowDown = false; // Whether the soldier should slow down if it encounters another unity

		// We check through every object that can be shot
		foreach(GameObject shootable in Flags.shootables){
			// Check that the shootable is valid and within range
			if(shootable != null && Vector3.Distance(shootable.transform.position, transform.position) <= range){
				// Ensure that neither object is being held by the player
				if(!GetComponent<MouseMoveable>() && !shootable.GetComponent<MouseMoveable>()){
					Flags shootableFlags = shootable.GetComponent<Flags>();

					// We check to ensure the two objects aren't on the same team
					if(shootableFlags.alliance != flags.alliance){
						// Check to see if the object is ready to attack again
						if(nextAttack < Time.time){
							// Check if the soldier does not use projectiles (bullets)
							if(projectile == null){
								shootableFlags.TakeDamage(damagePerShot, gameObject); // We damage the enemy and tell them this object did it
							}
							// If there is a projectile
							else{
								GameObject proj = Instantiate(projectile,transform.position,Quaternion.identity); // Create the projectile
								proj.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(shootable.transform.position - transform.position) * 15.0f; // Shoot the projectile towards the enemy
								ProjectileScript projScript = proj.GetComponent<ProjectileScript>(); // Store the projectile's script
								projScript.alliance = flags.alliance; // Set the alliance of the projectile
								projScript.owner = gameObject; // Set the owner of the projectile
								projScript.damage = damagePerShot; // Set the damage of the projectile
							}
							nextAttack = Time.time + shotCooldown; // We tell the object when it can shoot again
						}
						slowDown = true; // The object should slow down
						break; // We can only shoot one enemy at once
					}
				}
			}
		}

		// We check to see if the object moves on rails
		if(railScript != null){
			// We check to see if the object needs to be slowed down, and that it hasn't already been slowed down
			if(slowDown && railScript.speedMultiplier == 1.0f){
				railScript.speedMultiplier = 0.4f; // We slow down the object
			}
			// We check to see if the object no longer needs to slow down and is currently slowed down
			else if(!slowDown && railScript.speedMultiplier == 0.4f){
				railScript.speedMultiplier = 1.0f; // We return the object back to normal speed
			}
		}
    }
}
