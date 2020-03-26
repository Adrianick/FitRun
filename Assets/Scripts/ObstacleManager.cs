using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public PlayerMotor player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        player.animator.SetBool("GotHit", true);
    }

}
