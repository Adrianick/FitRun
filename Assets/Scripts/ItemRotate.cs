using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    private Transform burgerTransform;
    public ItemGenerator itemGenerator;
    public GameManager gameManager;
    public SoundManager soundManager;

    private float rotationSpeed = 130.0f;
    private const int goodItem = 10;
    private const int badItem = -25;
    private bool isGood = false;


    void Start()
    {
        itemGenerator = GameObject.FindGameObjectWithTag("ItemGenerator").GetComponent<ItemGenerator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }


    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (this.isGood)
        {
            this.gameManager.UpdateHighScore(goodItem);
        }
        else
        {
            this.gameManager.UpdateHighScore(badItem);
        }
        soundManager.PlaySound(this.isGood);
        this.gameObject.SetActive(false);
        //itemGenerator.DestroyInactiveItems();
    }


    public void SetIsGood(bool isGood)
    {
        this.isGood = isGood;
    }
}
