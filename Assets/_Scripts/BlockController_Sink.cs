using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController_Sink : MonoBehaviour {

	private bool _sink = false;
	private float _speed = 5f;
	private void Update()
	{
		//Check to see if the block should sink
		if (_sink){
			//Move block downwards
			this.transform.Translate(new Vector3(0,-1,0) * 1.5f * _speed * Time.deltaTime);
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player")){
			_sink = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player")){
			_sink = false;
		}
	}
}
