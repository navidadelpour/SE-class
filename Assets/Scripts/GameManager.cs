using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public GameObject tile;
	public GameObject upper_road;
	public GameObject under_road;
	public int score;
	public bool gameover;

	void Awake() {
		instance = this;
	}

	void Start () {
		gameover = false;
		score += (int)Time.time;
		upper_road = GameObject.Find ("UpperRoad");
		under_road = GameObject.Find ("UnderRoad");
	}

}
