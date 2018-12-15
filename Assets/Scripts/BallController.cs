using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public float angle;
	private Rigidbody rigid_body;
	private float fall_time = 0;
	private bool is_falling = false;
	private float angle_road;
	public float min_speed;
	public float max_speed;
	private int delete_factor = 15;
	private bool should_get_combo;

	// Use this for initialization
	void Start () {
		rigid_body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void FixedUpdate(){
		if (is_falling) {
			rigid_body.velocity = Vector3.down * 10 * (Time.time - fall_time);
		} 
		else {
			Vector3 vec =  new Vector3 (Mathf.Sin ((angle) * Mathf.Deg2Rad), 0f, Mathf.Cos ((angle) * Mathf.Deg2Rad)) * 10f;
			Vector3 direction = new Vector3 (Mathf.Sin ((angle + 90) * Mathf.Deg2Rad), 0f, Mathf.Cos ((angle + 90) * Mathf.Deg2Rad)); 
			Vector3 input = direction * Input.GetAxis ("Horizontal");
			rigid_body.velocity = (vec + input * vec.magnitude);

		}
		if (GameManager.instance.upper_road.transform.position.y < transform.position.y &&
		   GameManager.instance.under_road.transform.position.y < transform.position.y) {
			GameManager.instance.gameover = true;

		}
	}
	void OnCollisionEnter(Collision other){
		GameObject road = other.transform.parent.parent.gameObject;
		if (road.transform.GetChild (delete_factor).gameObject == other.transform.parent.parent.gameObject) {
			road.GetComponent<Road> ().IncreaseSector ();
		}
		if (road.name == "UnderRoad") {
			GameObject.Find ("UpperRoad").GetComponent<Road> ().DestroyRoad ();
			is_falling = false;
		}

	}

	public void SetMovementAngle(float angle){
		this.angle = angle;
	}
}
