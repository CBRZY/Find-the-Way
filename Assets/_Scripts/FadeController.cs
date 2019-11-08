using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour {

	//I got this code from Brackeys on YouTube
	//https://www.youtube.com/watch?v=0HwZQt94uHQ

	//It fades the screen to black when loading a scene
	//and fades back when a new scene is loaded

	//I used this when transitioning from one scene to another
	//which was very important when dying and the scene needs to reload

	//Create a 2d texture, in my case I used a a plain black image
	public Texture2D fadeTexture;

	//The speed at which the fading should happen before a scene is loaded.
	//This is enforced by calling a coroutine and yielding for the fadespeed
	public float fadeSpeed = 0.8f;

	//Draw depth is the ordering of GUI elements, the lowest depth values will appear on top of elements with higher values
	//A value of -1000 is to ensure that it draws on top of everything.
	//There really was no reason to go this low though
	private int _drawDepth = -1000;

	//Alpha of the texture. This is what we change to simulate the fading
	private float _alpha = 1.0f;
	//Fade Direction: -1 in and 1 out
	//This is to indicate if we are fading from an alpha of 0 to 1 or from 1 to 0
	private int _fadeDirection = -1;

	private void OnGUI()
	{
		_alpha += _fadeDirection * fadeSpeed * Time.deltaTime;
		_alpha = Mathf.Clamp01(_alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
		GUI.depth = _drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}

	public float BeginFade(int direction){
		//Change the fade direction and return the fade speed
		_fadeDirection = direction;
		return (fadeSpeed);
	}

	public void SceneLoaded(){
		BeginFade(-1);
	}
}
