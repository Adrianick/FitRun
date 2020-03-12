using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform burgerTransform;

    private float rotationSpeed = 65.0f;

    void Start()
    {
        burgerTransform = GameObject.FindGameObjectWithTag("Item").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
