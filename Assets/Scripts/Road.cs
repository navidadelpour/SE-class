using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

	private GameObject sector;
	private GameObject hole;

	private int tiles_in_sector;
	private int angle;
	private int threshold = 30;
	private int limit = 50;
	private int min_hole = 10;
	private int max_hole = 40;
	private Dictionary<int, int> tile_chances;

	void Start () {
		tile_chances = new Dictionary<int, int> {
			{4, 1},
			{5, 4},
			{6, 10},
			{7, 20},
		};

		tiles_in_sector = GetKeyByChance(tile_chances);
		angle = GetRandomAngle();

		MakeSector ();
		for (int i = 1; i < limit; i++) {
			IncreaseSector ();
		}

		if(this.name == "UpperRoad") {
			MakeHole ();
			GameObject.Find ("Player").GetComponent<BallController> ().SetMovementAngle (angle);
		}
	}

	// 0 - 35
	// 0 => return 4
	// 1, 2, 3, 4 => return 5	1 < chance < 4 + 1
	// 5, 6, 7, 8, 9, 10, 11, 12, 13, 14	=> return 6		IMPORTANT NOTE: 4 < chance < 15 = 10 + 4 + 1 -> this is previous_chance

	int GetKeyByChance(Dictionary<int, int> chances) {
		int chances_sum = 0;
		foreach (KeyValuePair<int, int> item in chances) {
			chances_sum += item.Value;
		}

		int chance = Random.Range (0, chances_sum);
		int previous_chances = 0;
		foreach(KeyValuePair<int, int> item in chances) {
			if(chance < item.Value + previous_chances)
				return item.Key;
			previous_chances += item.Value;
		}
		return 0;
	}

	int GetRandomAngle() {
		int previous_angle = GameManager.instance.upper_road.GetComponent<Road> ().angle;
		if (previous_angle == 0)
			previous_angle = GameManager.instance.under_road.GetComponent<Road> ().angle;

		int new_angle;
		do {
			new_angle = Random.Range (1, 90);
		} while(Mathf.Abs (new_angle - previous_angle) < threshold);

		return new_angle;
	}
	
	void MakeSector () {
		sector = new GameObject ();
		sector.name = "Sector";
		sector.transform.parent = this.transform;
		sector.transform.position = Vector3.zero;
		
		float temp = tiles_in_sector / 2 - .5f;
		for (int i = 0; i < tiles_in_sector; i++) {
			GameObject tile_created = Instantiate (GameManager.instance.tile, Vector3.right * temp, Quaternion.identity, sector.transform);
			if(i == 0 || i == tiles_in_sector - 1) {
				EvacuateTile (tile_created);
				tile_created.name = "edge";
				tile_created.tag = "edge";
			}
			temp--;
		}
		sector.transform.Rotate (Vector3.up, angle, Space.Self);
	}

	public void IncreaseSector() {
		GameObject sector_created = Instantiate (sector);
		sector_created.name = "Sector";
		sector_created.transform.parent = this.transform;
		sector_created.transform.position = sector.transform.position + new Vector3 (Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
		this.sector = sector_created;
		if (this.transform.childCount > limit) {
			GameObject sector_to_delete = this.transform.GetChild (0).gameObject;
			if(sector_to_delete.transform.FindChild("Hole") != null) {
				HandlePassingHole ();
			}
			Destroy (sector_to_delete);
		}
	}

	void EvacuateTile(GameObject tile) {
		tile.GetComponent<MeshRenderer> ().enabled = false;
		tile.GetComponent<BoxCollider> ().isTrigger = true;
		tile.GetComponent<BoxCollider> ().size = Vector3.one * .9f;
	}

	void MakeHole() {
		hole = this.transform.GetChild(Random.Range(min_hole, max_hole)).GetChild(Random.Range(1, tiles_in_sector - 1)).gameObject;
		EvacuateTile (hole);
		hole.name = "Hole";
		Invoke ("SetRoadUnderHole", .2f);
	}

	void SetRoadUnderHole() {
		GameManager.instance.under_road.transform.position = hole.transform.position + Vector3.down * 10f;
	}

	void HandlePassingHole() {
		Destroy (GameManager.instance.under_road, .5f);

		GameObject under_road = new GameObject ();
		under_road.name = "UnderRoad";
		GameManager.instance.under_road = under_road;
		under_road.AddComponent<Road> ();

		MakeHole ();
	}
	public void DestroyRoad(){
		Destroy (this.gameObject);
		GameObject upper_road = GameObject.Find ("UnderRoad");
		upper_road.name = "UpperRoad";
		GameManager.instance.upper_road = upper_road;

		GameObject under_road = new GameObject ();
		under_road.name = "UnderRoad";
		GameManager.instance.under_road = under_road;
		under_road.AddComponent<Road> ();

		upper_road.GetComponent<Road> ().MakeHole ();
	}
}
