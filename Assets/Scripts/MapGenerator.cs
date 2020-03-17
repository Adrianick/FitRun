﻿using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private Transform playerTransform;
    private float spawnZ = 0.0f;
    private float tileLength = 7.62f;
    private float safeZone = 10.0f;
    private int amountOfTilesOnScreen = 50;
    private int lastPrefabIndex = 0;
    private float spawnInterval = 5f;
    private float spawnTimer;

    private List<GameObject> activeTiles = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            if (i < 5)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile();
            }
        }

    }

    // Update is called once per frame

    private void FixedUpdate()
    {

    }
    private void Update()
    {
        //if (playerTransform.position.z - safeZone > (spawnZ - amountOfTilesOnScreen * tileLength))// && Time.time - spawnTimer > spawnInterval)
        //{
        //    SpawnTile();
        //    DeleteTile();
        //    //spawnTimer = Time.time;
        //}
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;

        if (prefabIndex == -1)
        {
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        }
        else
        {
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        }

        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ = spawnZ + tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        int randomIndex = lastPrefabIndex;

        if (tilePrefabs.Length <= 1)
        {
            return 0;
        }

        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;

        return randomIndex;
    }
}
