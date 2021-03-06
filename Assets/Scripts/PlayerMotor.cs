﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerMotor : MonoBehaviour
{
    private GameManager gameManager;

    private CharacterController controller;
    public Animator animator;
    private DeathMenu deathMenu;
    private SoundManager soundManager;
    private Vector3 movePlayer;
    private Vector3 targetVector;


    private readonly float startingAnimationDuration = 2f;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 9.0f;
    private float jumpForceMultiplier = 6.00f;
    private float gravity = 22.0f;
    private float rollingAnimationDuration = 0.3f;
    private float jumpingDuration = 0.20f;
    private float moveRightLeftDistance = 1.6f;
    private float startedRollingTime;
    private float startedJumpingTime;
    private float increaseRunningSpeedMultiplier = 0.4f;

    private bool isJumping = false;
    private bool isRolling = false;
    private bool wantsToRoll = false;
    private bool wantsToJump = false;
    //private bool wantsToGoRight = false;
    //private bool wantsToGoLeft = false;
    //private bool isRollingFromJump = false;




    private int currentLane = 1;
    private float startTime;

    [SerializeField] private HighScores _HighScores;
    private List<HighScoreEntry> highScoreEntryList;


    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
        deathMenu = GameObject.FindGameObjectWithTag("UserInterface").GetComponentInChildren<DeathMenu>(true);
        //deathMenu = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<DeathMenu>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


    }

    void FixedUpdate()
    {
        //movePlayer = Vector3.zero;
        //if (wantsToGoRight)
        //{
        //    GoRight();
        //}
        //else if (wantsToGoLeft)
        //{
        //    GoLeft();
        //}
        //movePlayer.y = verticalVelocity;
        //movePlayer.z = playerRunningSpeed;
        //controller.Move(movePlayer * Time.fixedDeltaTime);

    }
    void Update()
    {
        if (Time.time - startTime < startingAnimationDuration)
        {
            //controller.Move(Vector3.up * verticalVelocity);
            //controller.Move(Vector3.forward * playerRunningSpeed);
            MovePlayerSmoothly();

            return;
        }
        if (Time.time - startedRollingTime > rollingAnimationDuration && isRolling)
        {
            //SetDefaultControllerCenter();
            RollEnded();
            isRolling = false;
        }
        if (Time.time - startedJumpingTime > jumpingDuration && isJumping)
        {
            //jumpingDuration += 0.1f;
            JumpEnded();
        }


        if (this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Running"))
        {
            if (wantsToJump)
            {

                if (isRolling)
                {
                    PlayJump();
                    isRolling = false;
                    RollEnded();
                }
                else
                {
                    PlayJump();
                }
                wantsToJump = false;
            }
        }

        if (Input.GetKeyDown("space") && !isJumping)//&& !isRolling)
        {
            wantsToJump = true;
            wantsToRoll = false;
        }

        if (Input.GetKeyDown("s"))
        {
            wantsToRoll = true;
            wantsToJump = false;
            isJumping = false;
            //jumpingDuration -= 0.1f;
        }

        if (transform.position == targetVector)
        {
            print("Da");
        }
        if (Input.GetKeyDown("d"))
        {
            //wantsToGoRight = true;
            //wantsToGoLeft = false;
            MoveToSide(true);
        }

        if (Input.GetKeyDown("a"))
        {
            //wantsToGoLeft = true;
            //wantsToGoRight = false;
            MoveToSide(false);
        }
        if (Input.GetKeyDown("f"))
        {
            animator.SetBool("Attack", true);
        }
        if (isJumping == true)
        {
            if (wantsToRoll)
            {
                animator.SetBool("WantsToRollFromJump", true);
            }
            verticalVelocity = jumpForceMultiplier;
        }
        else if (controller.isGrounded)
        {

            if (wantsToRoll)
            {
                PlayRoll();
                wantsToRoll = false;

            }
            verticalVelocity = -0.5f;

            animator.SetBool("Landing", false);
            animator.SetBool("Jump", false);
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if (wantsToRoll)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Landing", true);
                verticalVelocity -= 5f * gravity * Time.deltaTime;
            }
        }



        MovePlayerSmoothly();

        //MovePlayer();
    }

    private void LateUpdate()
    {
        if (transform.position.y <= -11)
        {
            //print("Ai murit ba!");
            Lost();
        }
    }
    void MovePlayerSmoothly()
    {
        targetVector = Vector3.forward * transform.position.z;

        if (currentLane == 0)
        {
            targetVector += Vector3.left * moveRightLeftDistance;
        }
        else if (currentLane == 2)
        {
            targetVector += Vector3.right * moveRightLeftDistance;
        }
        else if (currentLane == -1)
        {
            targetVector += 2f * Vector3.left * moveRightLeftDistance;
        }
        else if (currentLane == 3)
        {
            targetVector += 2f * Vector3.right * moveRightLeftDistance;
        }
        else if (currentLane == -2)
        {
            targetVector += 3f * Vector3.left * moveRightLeftDistance;
        }
        else if (currentLane == 4)
        {
            targetVector += 3f * Vector3.right * moveRightLeftDistance;
        }
        movePlayer = Vector3.zero;
        //movePlayer.x = (targetVector - transform.position).normalized.x * playerRunningSpeed;
        movePlayer.x = (targetVector - transform.position).x * playerRunningSpeed;
        movePlayer.y = verticalVelocity;
        movePlayer.z = playerRunningSpeed;

        controller.Move(movePlayer * Time.deltaTime);
    }
    void MovePlayer()
    {
        movePlayer = Vector3.zero;
        movePlayer.y = verticalVelocity;
        movePlayer.z = playerRunningSpeed;
        controller.Move(movePlayer * Time.deltaTime);
    }
    void MoveToSide(bool goingRight)
    {
        currentLane += goingRight ? 1 : -1;
        currentLane = Mathf.Clamp(currentLane, -2, 4);
        //print("Current lane : " + currentLane);
    }
    void GoRight()
    {
        if (currentLane == 3)
        {
            // DEAD
            //controller.enabled = false;
        }
        currentLane++;
        movePlayer.x = 77f;
        //wantsToGoRight = false;
    }

    void GoLeft()
    {
        if (currentLane == 1)
        {
            // DEAD
            //controller.enabled = false;
        }
        currentLane--;
        movePlayer.x = -77f;
        //wantsToGoLeft = false;
    }
    void PlayRoll()
    {

        startedRollingTime = Time.time;
        isRolling = true;
        animator.speed = 0.8f;
        animator.SetBool("Roll", true);

        SetRollingControllerCenter();
    }
    void RollEnded()
    {
        animator.SetBool("Roll", false);

        SetDefaultControllerCenter();
        animator.speed = 1;
    }

    void PlayJump()
    {
        startedJumpingTime = Time.time;
        animator.SetBool("Jump", true);
        isJumping = true;
        animator.speed = 1.5f;
    }

    void PlayJumpFromRoll()
    {
        isRolling = false;
        animator.SetBool("WantsToJumpFromRoll", true);
        isJumping = true;
        //animator.speed = 2.6f;
    }
    void JumpEnded()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Landing", true);
        isJumping = false;
        animator.speed = 1;

    }
    void SetRollingControllerCenter()
    {
        controller.height = 1.3f;
        movePlayer.x = 0;
        movePlayer.y = 0.7f;
        movePlayer.z = 0;
        controller.center = movePlayer;
    }
    void SetDefaultControllerCenter()
    {
        controller.height = 2.5f;
        movePlayer.x = 0;
        movePlayer.y = 1.3f;
        movePlayer.z = 0;
        controller.center = movePlayer;
    }
    void Hit()
    {
        animator.SetBool("Attack", false);
    }
    public void HitFinished()
    {

        animator.SetBool("GotHit", false);
        animator.SetBool("ReadyToDie", true);
        this.enabled = false;
    }

    public void PlayerDied()
    {
        animator.SetBool("ReadyToDie", false);
        Lost();
    }
    public void Lost()
    {
        //soundManager.PlaySound(false);
        this.enabled = false;
        Time.timeScale = 1;
        Debug.Log(gameManager.highScore);
        AddHighScoreEntry(gameManager.highScore, "Player");

        SceneManager.UnloadSceneAsync("WS10");
        SceneManager.LoadSceneAsync("RestartMenu");
   
        //deathMenu.GameOver();
    }
    public void UpdateRunningSpeed()
    {
        playerRunningSpeed += playerRunningSpeed * increaseRunningSpeedMultiplier;
    }
    public float GetZPosition()
    {
        return transform.position.z;
    }
    public float GetRunningSpeed()
    {
        return playerRunningSpeed;
    }

    private void AddHighScoreEntry(int score, string name)
    {
        //Create HighScoreEntry
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        var exista = System.IO.File.Exists(Application.persistentDataPath + "/TableData.json");
        Debug.Log(exista);

        if (!System.IO.File.Exists(Application.persistentDataPath + "/TableData.json"))
        {

            highScoreEntryList = new List<HighScoreEntry>()
            {
                new HighScoreEntry{ score = score, name = name}
            };

            _HighScores = new HighScores { highScoresEntryList = highScoreEntryList };

            string potion = JsonUtility.ToJson(_HighScores);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/TableData.json", potion);

        } else
        {
            //Load saved HighScores
            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/TableData.json");
            HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

            //Add new entry
            highScores.highScoresEntryList.Add(highScoreEntry);


            _HighScores = new HighScores { highScoresEntryList = highScores.highScoresEntryList };

            string list = JsonUtility.ToJson(_HighScores);
            Debug.Log(list);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/TableData.json", list);
        }


    }

    [System.Serializable]
    private class HighScores
    {
        public List<HighScoreEntry> highScoresEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}