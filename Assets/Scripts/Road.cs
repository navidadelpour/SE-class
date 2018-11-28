using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

	private GameObject sector;
	private int tiles_in_sector;
	private int angle;
	private int limit = 50;
	private Dictionary<int, int> tile_chances;

	void Start () {
		tile_chances = new Dictionary<int, int> {
			{4, 1},
			{5, 4},
			{6, 10},
			{7, 20},
		};

		tiles_in_sector = GetKeyByChance(tile_chances);
		angle = Random.Range(1, 90);

		MakeSector ();
		for (int i = 1; i < limit; i++) {
			IncreaseSector ();
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
	
	void MakeSector () {
		sector = new GameObject ();
		sector.name = "Sector";
		sector.transform.parent = this.transform;
		sector.transform.position = Vector3.zero;
		
		float temp = tiles_in_sector / 2 - .5f;
		for (int i = 0; i < tiles_in_sector; i++) {
			Instantiate (GameManager.instance.tile, Vector3.right * temp, Quaternion.identity, sector.transform);
			temp--;
		}
		sector.transform.Rotate (Vector3.up, angle, Space.Self);
	}

	void IncreaseSector() {
		GameObject sector_created = Instantiate (sector);
		sector_created.name = "Sector";
		sector_created.transform.parent = this.transform;
		sector_created.transform.position = sector.transform.position + new Vector3 (Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
		this.sector = sector_created;
	}
}
