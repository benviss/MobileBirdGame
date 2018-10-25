using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> spawnedUnits = new List<GameObject>();

    public GameObject spawnPrefab;
    public float maxSpawned = 0;

    public float maxSpawnRate;
    private float nextSpawnTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //I think this is unsafe
        foreach (var spawnedUnit in spawnedUnits) {
            if (spawnedUnit == null) {
                spawnedUnits.Remove(spawnedUnit);
            }
        }
        //check if alive enemies < max enemies && next spawn time available
        if (spawnedUnits.Count < maxSpawned && nextSpawnTime < Time.time) {
            SpawnUnit();
            nextSpawnTime = Time.time + Random.Range(0,maxSpawnRate);
        }
	}

    void SpawnUnit()
    {
        //for spawned unit on ground, they will most likely need to get raised up by half of their height
        int selectedSpawnPoint = Random.Range(0, spawnPoints.Count);
        GameObject newSpawnedUnit = Instantiate(spawnPrefab, spawnPoints[selectedSpawnPoint].transform.position, this.transform.rotation, this.transform);
        spawnedUnits.Add(newSpawnedUnit);
    }
}
