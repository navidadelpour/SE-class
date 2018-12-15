using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject player;
	private Vector3 offset;
	public float angle;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position = player.transform.position + offset;
		//transform.RotateAround (player.transform.position, Vector3.up, player.GetComponent<BallController> ().angle);
		transform.LookAt (player.transform);
		
	}
}
