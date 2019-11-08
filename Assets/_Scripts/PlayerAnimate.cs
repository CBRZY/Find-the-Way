using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour {

	private int _lastDamage = -1;
	private int _lookDirection = 0;// 0 - Right; 1 - Left;
	private Animator _animator;
	private Rigidbody2D _rb;

	private PlayerController _pc;
	// Use this for initialization
	void Start () {
		_animator = this.GetComponent<Animator>();
		_rb = this.GetComponent<Rigidbody2D>();
		_pc = this.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		bool running = false;
		float horizontalInput = Input.GetAxis("Horizontal");

		if (_pc.isDead()){
			_animator.SetBool("Dead", true);
			NoJumping();
			NoWalkingOrRunning();
		}

		if (Input.GetKey(KeyCode.LeftShift)){
			running = true;
		}else{
			running = false;
			_animator.SetBool("Run_Right", false);
			_animator.SetBool("Run_Left", false);
		}

		if (horizontalInput == 0){
			NoWalkingOrRunning();
		}else if (horizontalInput > 0){
			_lookDirection = 0;
			//Positive horizontal movement means RIGHT
			if (running){
				_animator.SetBool("Run_Right", true);
			}else{
				_animator.SetBool("Walk_Right", true);
			}
		}else if (horizontalInput < 0){
			_lookDirection = 1;
			//Negative horizontal movement means LEFT
			if (running){
				_animator.SetBool("Run_Left", true);
			}else{
				_animator.SetBool("Walk_Left", true);
			}
		}

		//Check to see if player is jumping or falling by looking at velocity of the Y axis of the RigidBody
		//If so, turn off all walking and running animations
		//and turn on animations for Jumping Right or Left depending on keys pressed
		//
		//Jump_Right and Jump_Left is also used when taking damage and falling
		//This helps to change direction of damage sprite mid air
		if (_rb.velocity.y > 0.01 || _rb.velocity.y < -0.01){
			NoWalkingOrRunning();
			if (_lookDirection == 0){
				_animator.SetBool("Jump_Right", true);
				_animator.SetBool("Jump_Left", false);
			}else if (_lookDirection == 1){
				_animator.SetBool("Jump_Right", false);
				_animator.SetBool("Jump_Left", true);
			}
		}else{
			_animator.SetInteger("Damage", -1);
			_animator.SetBool("Jump_Right", false);
			_animator.SetBool("Jump_Left", false);
		}
	}

	private void NoWalkingOrRunning(){
		//Remove all walking and running animations
		_animator.SetBool("Idle", true);
		_animator.SetBool("Walk_Right", false);
		_animator.SetBool("Walk_Left", false);
		_animator.SetBool("Run_Right", false);
		_animator.SetBool("Run_Left", false);
	}

	private void NoJumping(){
		//Remove jumping animations
		_animator.SetBool("Jump_Right", false);
		_animator.SetBool("Jump_Left", false);
	}

	public void Damage(){
		//Take away all animations
		NoWalkingOrRunning();
		NoJumping();

		//Get a random value 0 to 2
		int rand = Random.Range(0,3);
		//While the random number is the same as the previous one when the player took damage
		//Get a random value again and do the same test
		//This is to ensure that the same damage animations can't happen consecutively, just to juice it up a bit :P
		while (rand == _lastDamage){
			rand = Random.Range(0,3);
		}
		//Save the random number for later use
		_lastDamage = rand;
		//Set the death animation to the random value
		_animator.SetInteger("Damage", rand);
	}
}
