using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

	[SerializeField]
	private GameObject _holder;
	[SerializeField]
	private Canvas _canvas;
	[SerializeField]
	private GameObject _exitPoint;

	[SerializeField]
	private GameObject _teleportEffect;

	private Transform _target;
	[SerializeField]
	[Range(1,50)]
	private float _activeRadius = 5f;
	private bool _isActive = false;
	private bool _canTeleport = false;

	private AudioSource[] _audioSources;

	private bool _soundPlayed = false;

	[SerializeField]
	private bool _isExit;
	// Use this for initialization
	void Start () {
		_holder.SetActive(false);
		_canvas.enabled = false;
		_target = GameManager.instance.player.transform;
		_audioSources = this.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//Get distance to player
		float distanceToPlayer = Vector2.Distance(this.transform.position, _target.position);

		//When player is close enough, activate the portal, activating the gameobject holding the particle effects and play the portal soun
		if (distanceToPlayer < _activeRadius){
			_holder.SetActive(true);
			_isActive = true;
			if (!_soundPlayed)
			ActivationSound();
		}else{
			//When the player leaves activeRadius, ensure they can't teleport
			_canTeleport = false;
		}

		//When the portal is active, start rotating the holder for the particle effects
		if (_isActive){
			_holder.transform.Rotate(new Vector3(0,0,1) * 100f * Time.deltaTime, Space.Self);
		}

		//if canTeleport is true and player presses Enter teleport
		if (_canTeleport && Input.GetKeyDown(KeyCode.Return)){
			//When the player leaves activeRadius, ensure they can't teleport
			_canTeleport = false;
			if (_isExit){
				//If the portal is set as the Exit portal, load the next scene
				GameManager.instance.ChangeLevel();
			}else{
				//Teleport the player to the exitPoint of the portal and instaniate teleport effect and play sound
				_audioSources[1].Play();
				GameManager.instance.player.transform.position = _exitPoint.transform.position;
				Instantiate(_teleportEffect,_exitPoint.transform.position, Quaternion.identity);
			}
			
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		//When player is inside portal set canTeleport to true
		if (other.tag.Equals("Player") && _isActive){
			_canTeleport = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//When player leaves the portal set canTeleport to fals
		if (other.tag.Equals("Player") && _isActive){
			_canTeleport = false;
		}
	}

	private void OnDrawGizmos()
	{
		//Draw a circle so that you can see on the Editor the radius of portal activation
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, _activeRadius);
	}

	private void ActivationSound(){
		_soundPlayed = true;
		_audioSources[0].Play();
	}
}
