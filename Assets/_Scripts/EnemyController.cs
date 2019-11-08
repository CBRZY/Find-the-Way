using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour {

	private GameObject _target;

	[SerializeField]
	[Range(1,5)]
	private float _moveSpeed;

	private float _movingDirection;

	private float _distanceToTarget;

	[SerializeField]
	[Range(1,20)]
	private float _soundDistance;

	private AudioSource _ghostNoise;
	private bool _madeNoise;

	// Use this for initialization
	void Start () {
		_target = GameManager.instance.player;
		_ghostNoise = this.GetComponent<AudioSource>();
		_madeNoise = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Find the distance between the enemy ghost and the player
		_distanceToTarget = Vector2.Distance(this.transform.position, _target.transform.position);

		//When there is a certain distance between the enemy ghost and player, make a noise
		if (_distanceToTarget <= _soundDistance && !_madeNoise){
			MakeNoise();
		}

		//Move ghost towards the player
		//Capture position on X Axis before and after the Translate to identify in which direction the ghost moves
		//This is used for changing the sprite but also pushing the player in a certain direction
		float oldX = this.transform.position.x;
		float step = _moveSpeed * Time.deltaTime;
		this.transform.position = Vector2.MoveTowards(this.transform.position, _target.transform.position, step);
		float newX = this.transform.position.x;

		_movingDirection = newX - oldX;

		//Quick and simple way to flip the sprite without changing to a different image or needing animations
		if (_movingDirection >= 0){
			this.GetComponent<SpriteRenderer>().flipX = true;
		}else{
			this.GetComponent<SpriteRenderer>().flipX = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//When the enemy touches the player, damage the player and make the player bounce in the direction the ghost is moving
		if (other.tag.Equals("Player")){
			PlayerController player = other.GetComponent<PlayerController>();
			player.Damage();
			player.Bounce(_movingDirection);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		//When the player stays inside the enemy ghost, damage the player and make the player bounce in the direction the ghost is moving
		//This is to ensure that when the ghost moves really quick and the player and the ghost is in the same position that the player still gets damaged
		if (other.tag.Equals("Player")){
			PlayerController player = other.GetComponent<PlayerController>();
			player.Damage();
			player.Bounce(_movingDirection);
		}
	}

	private void OnDrawGizmos()
	{
		//Draw a circle around the enemy ghost to see on the editor when a noise will be made
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, _soundDistance);
	}

	private void MakeNoise(){
		_madeNoise = true;
		_ghostNoise.Play();
	}
	
}
