using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController_Move : MonoBehaviour {


	[SerializeField]
	private GameObject _startPoint;

	[SerializeField]
	private GameObject _endPoint;

	[SerializeField]
	private float _moveSpeed = 5f;

	private int moveDirection = 1; //1 to EndPoint -1 to StartPoint

 	[Tooltip("Boolean value that indicates if the block should move or not")]
	public bool move;

	// Use this for initialization
	void Start () {
		this.transform.position = _startPoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//Check to see if the block should be moving
		if (move){
			float step = _moveSpeed * Time.deltaTime;
			if (moveDirection == 1){
				//Move towards endpoint
				this.transform.position = Vector2.MoveTowards(this.transform.position, _endPoint.transform.position, step);
			}else if (moveDirection == -1){
				//Move towards start point
				this.transform.position = Vector2.MoveTowards(this.transform.position, _startPoint.transform.position, step);
			}

			//When reaching the endpoint and startpoint, change direction
			if (this.transform.position == _endPoint.transform.position){
				moveDirection = -1;
			}else if (this.transform.position == _startPoint.transform.position){
				moveDirection = 1;
			}
		}
	}

	private void OnDrawGizmos()
	{
		//Draw a line from startPoint to endPoint to see the path better in editor
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(_startPoint.transform.position, _endPoint.transform.position);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//When the player enters the collider on top of the block, make the player a child of the block
		//This helps with keeping the player on the block when moving it around
		if (other.tag.Equals("Player")){
			move = true;
			other.transform.parent = this.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//Remember to remove the player as a child when jumping off of the block
		if (other.tag.Equals("Player")){
			other.transform.parent = null;
			
		}
	}
}
