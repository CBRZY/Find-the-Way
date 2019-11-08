using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	//Singleton to ensure that there is only one static instance of the GameManager
	//This makes interacting with the GameManager a lot easier
	#region Singleton
	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}
	#endregion

	public GameObject player;
	public Canvas healthCanvas;

	private Image[] hearts;

	[SerializeField]
	private GameObject _spawnPoint;
	private void Start()
	{
		//Always start a scene with the player on the spawn point, just to ensure that the player doesn't start somewhere weird
		//Also helps because this will fail if there is no player in the scene
		player.transform.position = _spawnPoint.transform.position;
		hearts = healthCanvas.GetComponentsInChildren<Image>();
	}

	//Method that is called when the health is changed.
	//Pass the current amount of health
	public void CurrentHealth(int health){
		int count = 0;
		int max = 3;
		//While count is smaller than the current amount of health
		while (count < health){
			//Colour the hearts white from left to right on the canvas
			hearts[count].color = new Color(255,255,255,255);
			count++;
		}
		//Now loop through the amount of hearts still left
		for (int i = count; i < max; i++){
			//And colour them black to indicate that they are missing
			hearts[i].color = new Color(0,0,0,255);
		}

		//Example
		//Method is called with 2, meaning that 1 heart has been lost
		//count = 0 and while 0 is smaller than 2 - Loop
		//Colour heart[count] white which is 0
		//count++
		//Colour heart[count] white which is 1
		//count++
		//count = 2 which isn't smaller than 2 so break loop
		//For loop
		//i = count which is 2; if i is smaller than max which is 3 then loop
		//Colour heart[i] black which is 2
		//i++
		//i is 3 which is not smaller than max which is 3 so break loop
	}

	public void Respawn(){
		//Just reload the current level
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
	}

	public void ChangeLevel(){
		//Progress to the next level
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}

	private IEnumerator LoadLevel(int levelIndex){
		//Change the fade direction and get the fade speed
		//yield for that amount of time
		//load level with levelIndex
		float fadeTime = this.GetComponent<FadeController>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(levelIndex);
	}
}
