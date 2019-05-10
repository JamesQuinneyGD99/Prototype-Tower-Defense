using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
	public static int amount;
	public static Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
		moneyText = GetComponent<Text>();
    }

	// This gives the player money (or takes, if negative)
	public static void Add(int increase){
		amount += increase; // Increase the player's money
		// Check to see if there is a HUD element for text
		if(moneyText != null){
			moneyText.text = "Money: " + amount; // Update the HUD text display
		}
	}
}
