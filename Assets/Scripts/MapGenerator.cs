using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public ItemGenerator itemGenerator;
    private Transform playerTransform;

    public GameObject[] tilePrefabs;
    public GameObject oceanPrefab;
    public GameObject[] backgroundPrefabs;

    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeOceans = new List<GameObject>();
    private List<GameObject> activeBackgroundObjectsLeft = new List<GameObject>();
    private List<GameObject> activeBackgroundObjectsRight = new List<GameObject>();

    private float spawnZ = 0.0f;
    private float oceanSpawnZ = 0.0f;
    private float oceanDestroyZ = 0.0f;
    private float oceanLength = 750.0f; // 1 tile  - 7.62 ; 3 - 22.86
    private float tileLength = 22.86f; // 1 tile  - 7.62 ; 3 - 22.86
    private float safeZone = 25.0f; // 1 tile - 10 ; 3 - 30
    private float oceanSpawnSafeZone = 550.0f;
    private float oceanDestroySafeZone = 50.0f; // 50 after the end of the ocean

    private int amountOfTilesOnScreen = 6;
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
            itemGenerator.SpawnNextTileRows();
        }
        if (playerTransform.position.z - oceanSpawnSafeZone > (oceanSpawnZ - oceanLength))
        {
            SpawnBackgroundObjects();
            SpawnOcean(); 
        }
        if (playerTransform.position.z - oceanDestroySafeZone > (oceanDestroyZ + oceanLength))
        {
            DeleteOcean();
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
    private void SpawnOcean()
    {
        GameObject ocean = Instantiate(oceanPrefab) as GameObject;
        ocean.transform.SetParent(transform);
        ocean.transform.position = Vector3.forward * oceanSpawnZ;
        oceanSpawnZ = oceanSpawnZ + oceanLength;
        activeOceans.Add(ocean);
    }
    private void DeleteOcean()
    {
        Destroy(activeOceans[0]);
        activeOceans.RemoveAt(0);
        oceanDestroyZ += oceanLength;
        DeletePreviousBackgroundObjects();
    }
    private void SpawnBackgroundObjects()
    {
        for (int i = 0; i < 15; i++)
        {
            
            int mustSpawn;

            // Stanga, 70% de a se spawna
            mustSpawn = Random.Range(1, 101);
            if(mustSpawn <= 70)
            {
                GameObject go = Instantiate(GenerateRandomBackgroundObject(0)) as GameObject;
                go.transform.SetParent(transform);

                if (go.gameObject.name.Equals("Island(Clone)"))
                {   
                    go.transform.position = new Vector3(Random.Range(-100, -50), 0.5f, oceanSpawnZ + (50 * i));
                }
                else
                {
                    go.transform.position = new Vector3(Random.Range(-50, -10), 0.5f, oceanSpawnZ + (50 * i));
                }

                activeBackgroundObjectsLeft.Add(go);
            }
            else
            {
                activeBackgroundObjectsLeft.Add(null);
            }

            // Dreapta, 70% de a se spawna
            mustSpawn = Random.Range(1, 101);
            if (mustSpawn <= 70)
            {
                GameObject go = Instantiate(GenerateRandomBackgroundObject(1)) as GameObject;
                go.transform.SetParent(transform);

                if (go.gameObject.name.Equals("Island(Clone)"))
                {
                    Debug.Log("Insula");
                    go.transform.position = new Vector3(Random.Range(50, 100), 0.5f, oceanSpawnZ + (50 * i));
                }
                else
                {
                    go.transform.position = new Vector3(Random.Range(10, 50), 0.5f, oceanSpawnZ + (50 * i));
                }

                
                activeBackgroundObjectsRight.Add(go);
            }
            else
            {
                activeBackgroundObjectsRight.Add(null);
            }


        }
    }
    private GameObject GenerateRandomBackgroundObject(int side)
    {
        /*
         * 0 = BG_Ship_1
         * 1 = BG_Ship_2
         * 2 = Island
         */

        bool validGeneration = true;

        while (validGeneration)
        {
            int randomNumber = Random.Range(1, 101);

            // 40% sansa pentru BG_Ship_1
            if (1 <= randomNumber && randomNumber <= 40)
            {
                return backgroundPrefabs[0];
            }

            // 40% sansa pentru BG_Ship_2
            if (41 <= randomNumber && randomNumber <= 80)
            {
                return backgroundPrefabs[1];
            }

            // 20% sansa pentru Island
            if (81 <= randomNumber && randomNumber <= 100 && NoIslandCollide(side))
            {
                return backgroundPrefabs[2];
            }
        }
        // Nu compileaza daca nu exista cel putin un return in afara if-urilor
        // Acest return ar trebui sa nu se execute niciodata
        return backgroundPrefabs[0];
    }

    private bool NoIslandCollide(int side)
    {
        if (activeBackgroundObjectsLeft.Count == 0 || activeBackgroundObjectsLeft.Count == 1 || activeBackgroundObjectsRight.Count == 0 || activeBackgroundObjectsRight.Count == 1)
        {
            return false;
        }

        // 0 = stanga, 1 = dreapta
        if (side == 0 && activeBackgroundObjectsLeft.Count >= 2)
        {
            int indexUltimul = activeBackgroundObjectsLeft.Count - 1;
            int indexPenultimul = activeBackgroundObjectsLeft.Count - 2;
      
            if (activeBackgroundObjectsLeft[indexUltimul] == null
                || activeBackgroundObjectsLeft[indexPenultimul] == null
                || activeBackgroundObjectsLeft[indexUltimul].gameObject.name.Equals("Island(Clone)") 
                || activeBackgroundObjectsLeft[indexPenultimul].gameObject.name.Equals("Island(Clone)"))
            {
                return false;
            }
        }
        else if ((side == 1 && activeBackgroundObjectsRight.Count >= 2))
        {
            int indexUltimul = activeBackgroundObjectsRight.Count - 1;
            int indexPenultimul = activeBackgroundObjectsRight.Count - 2;

            if (activeBackgroundObjectsRight[indexUltimul] == null
                || activeBackgroundObjectsRight[indexPenultimul] == null
                || activeBackgroundObjectsRight[indexUltimul].gameObject.name.Equals("Island(Clone)") 
                || activeBackgroundObjectsRight[indexPenultimul].gameObject.name.Equals("Island(Clone)"))
            {
                return false;
            }
        }
        return true;
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
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
    private void DeletePreviousBackgroundObjects()
    {
        for (int i = 0; i < 15; i++)
        {
            Destroy(activeBackgroundObjectsLeft[0]);
            Destroy(activeBackgroundObjectsRight[0]);

            activeBackgroundObjectsLeft.RemoveAt(0);
            activeBackgroundObjectsRight.RemoveAt(0);
        }
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

    public float AmountOfTiles()
    {
        return amountOfTilesOnScreen;
    }
    public float TileLength()
    {
        return tileLength;
    }


}
