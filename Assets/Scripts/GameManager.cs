using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int highScore = 0;
    private bool isAlive = true;
    public Text scoreText;
    public PlayerMotor player;
    public int startSpeedUp = 20;
    public int endSpeedUp = 1000;
    public int speedUpInterval = 0;
    public int speedUpIncrease = 100;
    public DeathMenu deathMenu;

    void Start()
    {
        scoreText.text = "Hello";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        deathMenu = GameObject.FindGameObjectWithTag("UserInterface").GetComponentInChildren<DeathMenu>(true);

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = highScore.ToString();
    }

    public void UpdateHighScore(int score)
    {
        highScore += score;

        if (highScore > startSpeedUp + speedUpInterval && highScore < endSpeedUp)
        {
            speedUpInterval += speedUpIncrease;
            player.UpdateRunningSpeed();

        } 
        else if (highScore < 0)
        {
            player.HitFinished();
            deathMenu.GameOver();
        }
    }
}
