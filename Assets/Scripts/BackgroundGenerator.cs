using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    private Transform playerTransform;

    public GameObject[] backgroundPrefabs;
    private float backgroundLength = 750.0f;
    private float spawnZ = 0.0f;
    private float safeZone = 400.0f;
    private int amountOfBackgroundOnScreen = 2;
    private List<GameObject> activeBackground = new List<GameObject>();

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < amountOfBackgroundOnScreen; i++)
        {
            SpawnBackground();
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - amountOfBackgroundOnScreen * backgroundLength))
        {
            SpawnBackground();
            DeleteBackground();
        }
    }

    private void SpawnBackground(int prefabIndex = -1)
    {
        GameObject go;
        
        go = Instantiate(backgroundPrefabs[0]) as GameObject;

        go.transform.SetParent(transform);
        go.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 3, spawnZ);
        spawnZ = spawnZ + backgroundLength;
        activeBackground.Add(go);
    }

    private void DeleteBackground()
    {
        Destroy(activeBackground[0]);
        activeBackground.RemoveAt(0);
    }
}
