using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class itemPrebas2
{
    public GameObject itemPrefab;
    public bool isGood;
    public AudioClip audioClip;
}
public class ItemGenerator : MonoBehaviour
{

    public itemPrebas2[] iPrefabs;

    private MapGenerator mapGenerator;
    public GameObject[] itemPrefabs;
    private GameManager gameManager;

    public List<GameObject> activeItems = new List<GameObject>();
    public bool[] isGood;

    void Start()
    {
        mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        GameObject car1 = Instantiate(iPrefabs[0].itemPrefab) as GameObject;
        car1.transform.SetParent(transform);
        car1.transform.position = new Vector3(0, 0 + 0.65f, 100);
        car1.AddComponent<ItemRotate>();
        car1.GetComponent<ItemRotate>().SetIsGood(iPrefabs[0].isGood);
        car1.AddComponent<BoxCollider>();
        car1.GetComponent<BoxCollider>().isTrigger = true;
        car1.AddComponent<AudioSource>();

        car1.GetComponent<AudioSource>().clip = iPrefabs[0].audioClip;



        GameObject car2 = Instantiate(iPrefabs[1].itemPrefab) as GameObject;
        car2.transform.SetParent(transform);
        car2.transform.position = new Vector3(0, 0 + 0.65f, 200);
        car2.AddComponent<ItemRotate>();
        car2.GetComponent<ItemRotate>().SetIsGood(iPrefabs[1].isGood);
        car2.AddComponent<BoxCollider>();
        car2.GetComponent<BoxCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyInactiveItems()
    {
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (!activeItems[i].gameObject.activeInHierarchy)
            {
                Destroy(activeItems[i]);
                activeItems.RemoveAt(i);
                //gameManager.UpdateHighScore(25);
            }
        }
    }


    public void SpawnItems(string tileTypeName, Vector3 tilePosition)
    {
        //print(tileTypeName);
        GameObject go;
        int itemIndex = Random.Range(0, itemPrefabs.Length);
        if (tileTypeName.Equals("Tile_2Large1Small")) // 3 1 3
        {
            go = Instantiate(itemPrefabs[itemIndex]) as GameObject;
        }
        else if (tileTypeName.Equals("Tile_3SmallBridges")) // 1 1 1
        {
            go = Instantiate(itemPrefabs[itemIndex]) as GameObject;
        }
        else /// 3 3 3
        {
            go = Instantiate(itemPrefabs[itemIndex]) as GameObject;
        }

        go.transform.SetParent(transform);
        go.transform.position = new Vector3(tilePosition.x, tilePosition.y + 0.65f, tilePosition.z + 40.24f);

        // Scale * 3 for banana
        go.transform.localScale *= 5.5f;


        // Added ItemRotateScript
        go.AddComponent<ItemRotate>();
        go.GetComponent<ItemRotate>().SetIsGood(isGood[itemIndex]);
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().clip = iPrefabs[0].audioClip;
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
