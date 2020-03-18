using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public ItemGenerator itemGenerator;
    public GameObject[] tilePrefabs;
    private Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();

    private float spawnZ = 0.0f;
    private float tileLength = 22.86f; // 1 tile  - 7.62 ; 3 - 22.86
    private float safeZone = 25.0f; // 1 tile - 10 ; 3 - 30
    private int amountOfTilesOnScreen = 5;
    private int lastPrefabIndex = 0;


    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        itemGenerator = GameObject.FindGameObjectWithTag("ItemGenerator").GetComponent<ItemGenerator>();

        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            if (i < 2)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile();
            }
        }

    }

    private void FixedUpdate()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - amountOfTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;

        if (prefabIndex == -1)
        {
            prefabIndex = RandomPrefabIndex();
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        }
        else
        {

            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        }

        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ = spawnZ + tileLength;
        activeTiles.Add(go);

        itemGenerator.SpawnItems(tilePrefabs[prefabIndex].gameObject.name, go.transform.position);
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


        //while (randomIndex == lastPrefabIndex)
        //{
        randomIndex = Random.Range(0, tilePrefabs.Length);
        //}
        while (tilePrefabs[lastPrefabIndex].gameObject.name.Equals("Tile_3SmallBridges") && tilePrefabs[randomIndex].gameObject.name.Equals("Tile_2Large1Small"))
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }
        while (tilePrefabs[lastPrefabIndex].gameObject.name.Equals("Tile_2Large1Small") && tilePrefabs[randomIndex].gameObject.name.Equals("Tile_3SmallBridges"))
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;

        return randomIndex;
    }
}
