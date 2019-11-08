using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {

	private Canvas _canvas;

	private void Awake()
	{
		//Get the canvas which has the TextMeshPro object and disable it
		//This is so that the player doesnt see all the notification words
		_canvas = this.GetComponentInChildren<Canvas>();
		_canvas.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//When the player enters the collider, enable the canvas displaying the words
		if (other.tag.Equals("Player")){
			_canvas.enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//When the player exits the collider, diable the canvas to hide the words again
		if (other.tag.Equals("Player")){
			_canvas.enabled = false;
		}
	}
}
