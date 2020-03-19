using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform burgerTransform;
    public ItemGenerator itemGenerator;
    public GameManager gameManager;
    private float rotationSpeed = 130.0f;
    private const int goodItem = 10;
    private const int badItem = -25;
    private bool isGood = false;

    //public ItemRotate(bool isGood)
    //{
    //    this.isGood = isGood;
    //}
    void Start()
    {
        //burgerTransform = GameObject.FindGameObjectWithTag("Item").transform;
        itemGenerator = GameObject.FindGameObjectWithTag("ItemGenerator").GetComponent<ItemGenerator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(this);
        if (this.isGood)
        {
            this.gameManager.UpdateHighScore(goodItem);
        } else
        {
            this.gameManager.UpdateHighScore(badItem);
        }
        this.gameObject.SetActive(false);
        itemGenerator.DestroyInactiveItems();
       
       
    }

    public void SetIsGood(bool isGood)
    {
        this.isGood = isGood;
    }
}
