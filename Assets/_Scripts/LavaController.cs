using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour {

	[SerializeField]
	private GameObject _lavaSplashPrefab;

	private AudioSource _audioSource;
	private void Start()
	{
		_audioSource = this.GetComponent<AudioSource>();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		//When player enter the edge collider of the lava
		//Play lava splash
		_audioSource.Play();
		if (other.tag.Equals("Player")){
			PlayerController player = other.GetComponent<PlayerController>();

			//Instantiate lava splash prefab (particle effect) that destroys itself at end of loop
			Instantiate(_lavaSplashPrefab, player.transform.position - new Vector3(0,1,0), Quaternion.identity);

			//Damage the player and bounce it in the air
			player.Damage();	
			player.Bounce(0);
		}
	}

	
}
