using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	private int _health = 3;

	private float _walkingSpeed = 2.0f;
	private float _runningSpeed = 4.0f;

	[SerializeField]
	[Range(1,10)]
	private float _jumpVelocity;
	private float _fallMultiplier = 2.75f;
	private bool _jumpRequest = false;
	public Rigidbody2D _rb;

	private AudioSource _audioSource;

	private bool _died = false;
	
	void Awake () {
		_rb = this.GetComponent<Rigidbody2D>();
		_audioSource = this.GetComponent<AudioSource>();
	}

	void Update()
	{
		//Get axis input 
		//Changed horizontal axis to only be left and right arrows
		float horizontalInput = Input.GetAxis("Horizontal");
		if (horizontalInput != 0){
			Move(horizontalInput);
		}

		if (_died){
			Respawn();
		}

		//If player presses the Space key and the player isn't falling or jumping do a jump request
		if (Input.GetKeyDown(KeyCode.Space) && (_rb.velocity.y < 0.01 && _rb.velocity.y > -0.01)){
			_jumpRequest = true;
		}
	}
	
	void FixedUpdate () {
		//If jump request true do the jump. 
		//This is in fixedupdate because I am adding a force to a rigidbody of the player
		if (_jumpRequest){
			Jump();
			_jumpRequest = false;
		}

		//If player is falling down then speed up (or actually down :P) the falling
		if (_rb.velocity.y < 0){
			/*
			 * Vector2.up 				Velocity of a Rigidbody is a Vector, so final value needs to be a Vector
			 							Up so that the final value is positive
			 * Physics2D.gravity.y		Type of force and axis
			 							Gravity (9.8m/s^2) and we are only interested in the Y axis of the Vector
			 * _fallMultiplier			The amount by which we want to accelerate gravity with
			 */
			 //Explicitly altering the velocity of the Rigibody as I don't want realistic behaviour when falling
			// _rb.velocity += Vector2.up * Physics2D.gravity.y * _fallMultiplier;
			//Easier is to change the gravity scale of the rigidbody
			_rb.gravityScale = _fallMultiplier;
		}else if (_rb.velocity.y == 0){
			//This is to ensure that when the player is not falling that the gravity is normal
			_rb.gravityScale = 1;
		}
	}

	private void Move(float horizontalInput){
		//Move right or left
		if (Input.GetKey(KeyCode.LeftShift)){
			//Run
			this.transform.Translate(new Vector3(1,0,0) * horizontalInput * _runningSpeed * Time.deltaTime);
		}else{
			//Walk
			this.transform.Translate(new Vector3(1,0,0) * horizontalInput * _walkingSpeed * Time.deltaTime);
		}
		
	}
	private void Jump(){
		_rb.AddForce(Vector2.up * _jumpVelocity, ForceMode2D.Impulse);
	}

	public void Damage(){
		_audioSource.Play();
		_health--;

		if (_health <= 0){
			_died = true;
		}

		PlayerAnimate playerAnimate = this.GetComponent<PlayerAnimate>();
		playerAnimate.Damage();

		GameManager.instance.CurrentHealth(_health);
	}

	public void Bounce(float direction){
		//Explicitly altering the velocity of the Rigibody as I don't want realistic behaviour when falling
		//Bouncing needs to take preference over all other forces, that is why I set the velocity and not add it
		//Bouncing needs to be higher then jumping to give the player time to get away from the situation
		if (_health > 0){
			if (direction == 0){
				_rb.velocity = Vector2.up * _jumpVelocity * 1.75f;
			}else{
				//This is to push the player in a direction.
				//The ghost moves in a direction and this value is amplified to simulate a push
				if (direction > 0){
					_rb.velocity = new Vector2(0.5f,0.35f) * _jumpVelocity * 1.75f;
				}else if (direction < 0){
					_rb.velocity = new Vector2(-0.5f,0.35f) * _jumpVelocity * 1.75f;
				}
				
			}
			
		}
		
	}

	private void Respawn(){
		_died = false;
		GameManager.instance.Respawn();
	}

	public bool isDead(){
		return _died;
	}
}
