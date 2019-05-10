using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveable : MonoBehaviour
{
	public static MouseMoveable currentlyHolding; // The object that is currently being held
	public bool canPickup; // Whether this item can currently be picked up
	public bool pickedUp; // Whether this item is currently picked up
	[SerializeField]
	float moveSpeed = 300.0f; // The speed this item moves towards the mouse, note: 300 is perfect from my testing
	float lastPickup = 0.0f;
	List<Collider2D> touching; // Everything that is currently touching the object

	void Start(){
		touching = new List<Collider2D>(); // Create an empty list
	}

    // Update is called once per frame
    void LateUpdate()
    {
		if(pickedUp){
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // We work out where the mouse is in the game area

			GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(worldPos - transform.position) * moveSpeed; // Push the object towards the mouse at the appropriate speed
			// note: Vector2.Normalize causes all sorts of problems

			// Check to see if the player releases the mouse, also check if it's been more than 1 second since the object was picked up
			if(Input.GetMouseButtonDown(0) && Time.time - lastPickup > 0.1f){
				Drop(); // Drop the object
			}
		}
    }

	// This will make the player pick up the object
	public void PickUp(){
		lastPickup = Time.time;
		// Check if the player is already holding something
		if(currentlyHolding != null){
			currentlyHolding.Drop(); // Drop the object the player is already holding
		}
		currentlyHolding = this; // Tell the class this is the object being held
		pickedUp = true; // Pick up the object
		gameObject.layer = 2; // Make it so OnMouseDown wont trigger
		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>(); // Add a rigidbody
		rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Unfreeze the position of the object, but freeze the rotation
		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Ensuring continuous collision detection ensures players wont glitch it out of the map
	}

	// This will make hte player drop the object
	void Drop(){
		bool canDrop = true; // Whether the object can be dropped

		// Check through everything the object is touching
		foreach(Collider2D collider in touching){
			// Check if the collider still exists and that it doesn't allow objects to be dropped on it
			if(collider != null && collider.gameObject.tag == "No Drop"){
				canDrop = false; // Tell the object that it can't be dropped
				break; // No need to check if anything else is blocking the object
			}
		}

		// Check if the object can be dropped
		if(canDrop){
			// Check to see if the player is holding this object
			if(currentlyHolding != null && currentlyHolding == this){
				currentlyHolding = null; // Tell the class that the player is no longer holding anything
			}
			pickedUp = false; // Drop the object
			gameObject.layer = 0; // Reset the layer
			Destroy(GetComponent<Rigidbody2D>());

			// Check to see if the object can't be picked up again
			if(!canPickup){
				Destroy(this); // If the object can't be picked up again, there is no need for this script to be attached
			}
		}
	}

	// Called when the player clicks above this object
	void OnMouseDown(){
		// Check if the object can be picked up
		if(canPickup){
			PickUp(); // Pick up the object
		}
	}

	// This is called every tick the obejct is touching another
	void OnTriggerStay2D(Collider2D collision){
		// Check if the object is on top of a bin, and the mouse is released
		if(collision.gameObject.tag == "Disposal" && Input.GetMouseButtonUp(0)){
			Destroy(gameObject); // Remove the object
		}
	}

	// Check for an entry trigger 
	void OnTriggerEnter2D(Collider2D collider){
		touching.Add(collider); // Add the trigger to the touching list

		// Check if we are hovering over a no drop zone
		if(collider.gameObject.tag == "No Drop" && pickedUp){
			NoDropScript.currentOffset = 1.0f; // Reset the offset of the no drop cross
			NoDropScript.isActive = true; // Enable the no drop script
		}
	}

	// Check for an exit trigger
	void OnTriggerExit2D(Collider2D collider){
		touching.Remove(collider); // Remove the trigger from the touching list

		bool disableNoDrop = true; // Whether we will disable the no drop cross

		// Check through everything still being touched
		foreach(Collider2D col in touching){
			// Check if the player is still hovering over a no drop zone
			if(col != null && col.gameObject.tag == "No Drop"){
				disableNoDrop = false; // We will not disable the no drop cross
			}
		}

		// Check if we need to disable the no drop cross
		if(disableNoDrop){
			NoDropScript.isActive = false; // Disable the no drop cross
			Color color = NoDropScript.main.color; // Get the current color of the no drop cross
			NoDropScript.main.color = new Color(color.r,color.g,color.b, 0.0f); // Make the no drop cross invisible again
		}
	}
}
