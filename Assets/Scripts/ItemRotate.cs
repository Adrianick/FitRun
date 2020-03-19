using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform burgerTransform;
    public ItemGenerator itemGenerator;

    private float rotationSpeed = 130.0f;

    void Start()
    {
        //burgerTransform = GameObject.FindGameObjectWithTag("Item").transform;
        itemGenerator = GameObject.FindGameObjectWithTag("ItemGenerator").GetComponent<ItemGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(this);
        this.gameObject.SetActive(false);
        itemGenerator.DestroyInactiveItems();
        //this.gameObject.inde
        //activeItems.RemoveAt(0)
    }
}
