using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

	//Method to call the main menu scene when the player clicks on the button linked to the method
	public void LoadMainMenu(){
		SceneManager.LoadScene(0);
	}
}
