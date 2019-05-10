using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDropScript : MonoBehaviour
{
	public static bool isActive; // Whether the player currently can't drop an object
	public static float currentOffset; // The current opacity offset
	public static SpriteRenderer main; // The sprite renderer for the no drop cross

	void Start(){
		main = GetComponent<SpriteRenderer>(); // We store the sprite renderer
	}

    // Update is called once per frame
    void Update()
    {
		// Check if the sprite is active
		if(isActive){
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10); // Lock the object to the mouse, also unity handles 2D badly
			Color col = main.color; // Store the current colour
			currentOffset = (currentOffset + Time.deltaTime * 3.0f) % 2.0f; // Loop the offset between 0 and 2
			main.color = new Color(col.r,col.g,col.b, Mathf.Abs(currentOffset - 1.0f)); // This will loop the opacity
		}
	}
}
