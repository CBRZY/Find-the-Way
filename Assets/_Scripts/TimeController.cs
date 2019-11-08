using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour {

	//This class was created to simulate a timer countdown
	//I used this for the second level
	//But found it to be too boring and really didn't mean much in the lines of gameplay
	[SerializeField]
	private int _timerDuration;
	[SerializeField]
	private Canvas _timeCanvas;

	private TextMeshProUGUI _timeText;

	private float _timeLeft;
	// Use this for initialization
	void Start () {
		_timeText = _timeCanvas.GetComponentInChildren<TextMeshProUGUI>();
		_timeText.text = _timerDuration.ToString();

		_timeLeft = _timerDuration;
	}
	
	// Update is called once per frame
	void Update () {
		//Countdown every second from timeleft
		_timeLeft -= Time.deltaTime;
		//Update textmeshpro on canvas to indicate time counting down
		_timeText.text = Mathf.RoundToInt(_timeLeft).ToString();
		

		//When the timer has run out, reload the current scene
		if (_timeLeft <= 0){
			GameManager.instance.Respawn();
		}
	}
}
