using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	//Method linked to buttons on the main meny
	public void StartGame(){
		SceneManager.LoadScene(1);
	}

	public void Exit(){
		Application.Quit();
	}
}
