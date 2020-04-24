using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public DeathMenu deathMenu;
    public PlayerMotor player;
    public Text scoreText;

    public int highScore = 0;

    public int startSpeedUp;
    public int endSpeedUp;
    public int speedUpInterval;
    public int speedUpIncrease;

    private float increaseHighscoreEverySeconds = 0.4f;
    private float startTime;

    //private bool isAlive = true;

    void Start()
    {
        scoreText.text = "Hello";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        deathMenu = GameObject.FindGameObjectWithTag("UserInterface").GetComponentInChildren<DeathMenu>(true);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - increaseHighscoreEverySeconds > startTime)
        {
            highScore += 1;
            startTime = Time.time;
        }

        scoreText.text = highScore.ToString();

        if (highScore >= startSpeedUp + speedUpInterval && highScore < endSpeedUp)
        {
            print("Speedup: " + startSpeedUp + speedUpInterval);
            startSpeedUp += speedUpInterval;
            speedUpInterval += speedUpIncrease;
            player.UpdateRunningSpeed();

        }
    }

    public void UpdateHighScore(int score)
    {
        highScore += score;


        if (highScore < 0)
        {
            player.animator.SetBool("GotHit", true);
            //deathMenu.GameOver();
        }
    }
}
