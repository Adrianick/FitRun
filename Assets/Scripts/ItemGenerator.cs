using System.Collections.Generic;
using UnityEngine;


public class ItemGenerator : MonoBehaviour
{

    private MapGenerator mapGenerator;
    private GameManager gameManager;
    private PlayerMotor player;

    public GameObject[] goodItems;
    public GameObject[] badItems;
    public GameObject[] obstacles;

    private float distanceBetweenItems = 9f;
    private float distanceMultiplier = 0.4f;

    private float lastSpawnedRowPosition = 30f;
    private float safeDestroyZone = 3f;
    private float mapTileLength;
    private float endOfLastTile;
    private float initialTilesTotalLength;
    private float playerRunningSpeed;

    private float sideLanesOffset = 1.6f;

    private List<GameObject> activeItems = new List<GameObject>();

    private int numberOfLanes = 3;
    private int typesOfObjects = 3; // 0 is Good, 1 is Bad, 2 is Obstacle

    void Start()
    {
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        playerRunningSpeed = player.GetRunningSpeed();

        mapTileLength = mapGenerator.TileLength();

        initialTilesTotalLength = mapTileLength * mapGenerator.AmountOfTiles();

        SpawnInitialItems();
    }


    void Update()
    {

    }
    void FixedUpdate()
    {
        if (player.GetZPosition() > (activeItems[0].transform.position.z + safeDestroyZone))
        {
            DeleteRow();
        }
    }
    public void DestroyInactiveItems()
    {
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (activeItems[i].gameObject.transform.position.z + playerRunningSpeed < player.GetZPosition())
            {
                Destroy(activeItems[i]);
                activeItems.RemoveAt(i);
            }
            //if (!activeItems[i].gameObject.activeInHierarchy)
            //{
            //    Destroy(activeItems[i]);
            //    activeItems.RemoveAt(i);
            //    //gameManager.UpdateHighScore(25);
            //}
        }
    }

    public void SpawnNextTileRows()
    {
        endOfLastTile += mapTileLength;
        int numberOfRowsToSpawn = Mathf.FloorToInt((endOfLastTile - lastSpawnedRowPosition) / playerRunningSpeed);
        for (int i = 0; i < numberOfRowsToSpawn; i++)
        {
            SpawnRow();
        }
    }
    public void SpawnRow()
    {
        int numberOfObjectsToSpawn = DecideHowManyObjectsToSpawn();
        float nextSpawnRowPosition = CalculateNextSpawnRowPosition();
        //print("Spawn distance : " + (nextSpawnRowPosition - lastSpawnedRowPosition));
        lastSpawnedRowPosition = nextSpawnRowPosition;

        if (numberOfObjectsToSpawn == 1)
        {
            SpawnOneObject();
        }
        else if (numberOfObjectsToSpawn == 2)
        {
            SpawnTwoObjects();
        }
        else
        {
            SpawnThreeObjects();
        }
    }

    public void DeleteRow()
    {
        Destroy(activeItems[0]);
        activeItems.RemoveAt(0);
    }

    public void SpawnInitialItems()
    {
        int numberOfInitialRowsToSpawn = Mathf.FloorToInt((initialTilesTotalLength - lastSpawnedRowPosition) / playerRunningSpeed);
        endOfLastTile = initialTilesTotalLength;
        for (int i = 0; i < numberOfInitialRowsToSpawn; i++)
        {
            SpawnRow();
        }
    }
    public float CalculateNextSpawnRowPosition()
    {
        float newPlayerRunningSpeed = player.GetRunningSpeed();
        if (newPlayerRunningSpeed > playerRunningSpeed)
        {
            distanceBetweenItems += distanceBetweenItems * distanceMultiplier;
            playerRunningSpeed = newPlayerRunningSpeed;
        }
        //return lastSpawnedRowPosition + playerRunningSpeed;
        return lastSpawnedRowPosition + distanceBetweenItems;
    }

    public void SpawnOneObject()
    {
        int choice = Random.Range(0, typesOfObjects);
        int lane = Random.Range(0, numberOfLanes);
        if (choice == 0)
        {
            SpawnObject(0, lane);
        }
        else if (choice == 1)
        {
            SpawnObject(1, lane);
        }
        else
        {
            SpawnObject(2, lane);
        }
    }

    public void SpawnTwoObjects()
    {
        int choice = Random.Range(0, 8);
        int lanes = Random.Range(0, 2);

        if (choice == 0)      // Good & Obstacle 0 1 lanes
        {
            SpawnObject(0, 0);
            SpawnObject(2, 1);
        }
        else if (choice == 1)  // Good & Obstacle 1 2 lanes
        {
            SpawnObject(0, 1);
            SpawnObject(2, 2);
        }
        else if (choice == 2) // Obstacle & Good 0 1 lanes
        {
            SpawnObject(2, 0);
            SpawnObject(0, 1);
        }
        else if (choice == 3)  // Obstacle & Good 1 2 lanes
        {
            SpawnObject(2, 1);
            SpawnObject(0, 2);
        }
        else if (choice == 4) // Bad & Obstacle 0 1 lanes
        {
            SpawnObject(1, 0);
            SpawnObject(2, 1);
        }
        else if (choice == 5) // Bad & Obstacle 1 2 lanes
        {
            SpawnObject(1, 1);
            SpawnObject(2, 2);
        }
        else if (choice == 6) // Obstacle & Bad 0 1 lanes
        {
            SpawnObject(2, 0);
            SpawnObject(1, 1);
        }
        else                  // Obstacle & Bad 1 2 lanes
        {
            SpawnObject(2, 1);
            SpawnObject(1, 2);
        }

    }

    public void SpawnThreeObjects()
    {
        int choice = Random.Range(0, 18);
        if (choice == 0)       // Good, Bad, Obstacle
        {
            SpawnObject(0, 0);
            SpawnObject(1, 1);
            SpawnObject(2, 2);
        }
        else if (choice == 1)  // Good, Obstacle, Bad
        {
            SpawnObject(0, 0);
            SpawnObject(2, 1);
            SpawnObject(1, 2);
        }
        else if (choice == 2)   // Bad, Good, Obstacle
        {
            SpawnObject(1, 0);
            SpawnObject(0, 1);
            SpawnObject(2, 2);
        }
        else if (choice == 3)  // Bad, Obstacle, Good
        {
            SpawnObject(1, 0);
            SpawnObject(2, 1);
            SpawnObject(0, 2);
        }
        else if (choice == 4)  // Obstacle, Good, Bad
        {
            SpawnObject(2, 0);
            SpawnObject(0, 1);
            SpawnObject(1, 2);
        }
        else if (choice == 5)  // Obstacle, Bad, Good
        {
            SpawnObject(2, 0);
            SpawnObject(1, 1);
            SpawnObject(0, 2);
        }
        else if (choice == 6)  // Bad, Bad, Good
        {
            SpawnObject(1, 0);
            SpawnObject(1, 1);
            SpawnObject(0, 2);
        }
        else if (choice == 7)  // Bad, Good, Bad
        {
            SpawnObject(1, 0);
            SpawnObject(0, 1);
            SpawnObject(1, 2);
        }
        else if (choice == 8)  // Good, Bad, Bad
        {
            SpawnObject(0, 0);
            SpawnObject(1, 1);
            SpawnObject(1, 2);
        }
        else if (choice == 9)  // Obstacle, Obstacle, Good
        {
            SpawnObject(2, 0);
            SpawnObject(2, 1);
            SpawnObject(0, 2);
        }
        else if (choice == 10) // Obstacle, Good, Obstacle
        {
            SpawnObject(2, 0);
            SpawnObject(0, 1);
            SpawnObject(2, 2);
        }
        else                   // Good, Obstacle, Obstacle
        {
            SpawnObject(0, 0);
            SpawnObject(2, 1);
            SpawnObject(2, 2);
        }
    }

    public int DecideHowManyObjectsToSpawn()
    {
        return Random.Range(0, numberOfLanes) + 1;
    }


    public void SpawnObject(int whichObject, int lane)
    {
        float spawnPositionX = 0f;
        if (lane == 0)
        {
            spawnPositionX -= sideLanesOffset;
        }
        else if (lane == 2)
        {
            spawnPositionX += sideLanesOffset;
        }

        GameObject go;
        if (whichObject == 0) // Good
        {
            int itemIndex = Random.Range(0, goodItems.Length);
            go = Instantiate(goodItems[itemIndex]) as GameObject;

            // Increase object size
            go.transform.localScale *= 5.5f;

            // Rotate Random
            go.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);

            // Added ItemRotateScript
            go.AddComponent<ItemRotate>();
            go.GetComponent<ItemRotate>().SetIsGood(true);

            go.transform.SetParent(transform);
            go.transform.position = new Vector3(spawnPositionX, 0.65f, lastSpawnedRowPosition); // previous became next already

            go.AddComponent<BoxCollider>();
            go.GetComponent<BoxCollider>().isTrigger = true;
        }
        else if (whichObject == 1) // Bad
        {
            int itemIndex = Random.Range(0, badItems.Length);
            go = Instantiate(badItems[itemIndex]) as GameObject;

            // Increase object size
            go.transform.localScale *= 5.5f;

            // Rotate Random
            go.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);

            // Added ItemRotateScript
            go.AddComponent<ItemRotate>();
            go.GetComponent<ItemRotate>().SetIsGood(false);

            go.transform.SetParent(transform);
            go.transform.position = new Vector3(spawnPositionX, 0.65f, lastSpawnedRowPosition); // previous became next already

            go.AddComponent<BoxCollider>();
            go.GetComponent<BoxCollider>().isTrigger = true;
        }
        else // Obstacle
        {
            int itemIndex = Random.Range(0, obstacles.Length);
            go = Instantiate(obstacles[itemIndex]) as GameObject;

            go.transform.SetParent(transform);
            go.transform.position = new Vector3(spawnPositionX, 0.1f, lastSpawnedRowPosition); // previous became next already

            foreach (Transform child in go.transform)
            {
                child.gameObject.AddComponent<ObstacleManager>();
                child.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            }

            //go.AddComponent<ObstacleManager>();
        }



        go.AddComponent<AudioSource>();

        activeItems.Add(go);
    }


    //public void SpawnItems(string tileTypeName, Vector3 tilePosition)
    //{
    //    //print(tileTypeName);
    //    GameObject go;
    //    int itemIndex = Random.Range(0, itemPrefabs.Length);
    //    if (tileTypeName.Equals("Tile_2Large1Small")) // 3 1 3
    //    {
    //        go = Instantiate(itemPrefabs[itemIndex]) as GameObject;
    //    }
    //    else if (tileTypeName.Equals("Tile_3SmallBridges")) // 1 1 1
    //    {
    //        go = Instantiate(itemPrefabs[itemIndex]) as GameObject;
    //    }
    //    else /// 3 3 3
    //    {
    //        go = Instantiate(itemPrefabs[itemIndex]) as GameObject;
    //    }

    //    go.transform.SetParent(transform);
    //    go.transform.position = new Vector3(tilePosition.x, tilePosition.y + 0.65f, tilePosition.z + 40.24f);

    //    // Scale * 3 for banana
    //    go.transform.localScale *= 5.5f;


    //    // Added ItemRotateScript
    //    go.AddComponent<ItemRotate>();
    //    go.GetComponent<ItemRotate>().SetIsGood(isGood[itemIndex]);
    //    go.AddComponent<BoxCollider>();
    //    go.GetComponent<BoxCollider>().isTrigger = true;
    //    go.AddComponent<AudioSource>();
    //    //go.GetComponent<AudioSource>().clip = iPrefabs[0].audioClip;
    //    // Rotate Random
    //    go.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);

    //    activeItems.Add(go);
    //}

}
