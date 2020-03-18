using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{

    public MapGenerator mapGenerator;
    public GameObject[] itemPrefabs;

    private List<GameObject> activeItems = new List<GameObject>();
    void Start()
    {
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnItems(string tileTypeName, Vector3 tilePosition)
    {
        //print(tileTypeName);
        GameObject go;

        if (tileTypeName.Equals("Tile_2Large1Small")) // 3 1 3
        {
            go = Instantiate(itemPrefabs[0]) as GameObject;
        }
        else if (tileTypeName.Equals("Tile_3SmallBridges")) // 1 1 1
        {
            go = Instantiate(itemPrefabs[0]) as GameObject;
        }
        else /// 3 3 3
        {
            go = Instantiate(itemPrefabs[0]) as GameObject;
        }

        go.transform.SetParent(transform);
        go.transform.position = new Vector3(tilePosition.x, tilePosition.y + 1, tilePosition.z + 15.24f);
        // go.transform.position = Vector3.forward * spawnZ;
        //spawnZ = spawnZ + tileLength;
        go.transform.localScale *= 3;
        go.AddComponent<ItemRotate>();

        // Rotate Random
        go.transform.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);

        activeItems.Add(go);
    }

    private void DeleteItem()
    {
        ////ON HIT COLLISION
        //Destroy(activeItems[0]);
        //activeItems.RemoveAt(0);
    }
}
