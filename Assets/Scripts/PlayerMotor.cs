using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private Vector3 movePlayer;
    private Vector3 beforeSideMovePosition;

    private readonly float startingAnimationDuration = 0.5f;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 13.0f;
    private float jumpForceMultiplier = 5.0f;
    private float gravity = 24.0f;
    private float rollingAnimationDuration = 0.3f;
    private float jumpingDuration = 0.4f;
    private float moveRightLeftTransitionSpeed = 2f;
    private float moveRightLeftDistance = 2f;
    private float startedRollingTime;
    private float startedJumpingTime;

    private bool isJumping = false;
    private bool isRolling = false;
    private bool wantsToRoll = false;
    private bool wantsToJump = false;
    private bool wantsToGoRight = false;
    private bool wantsToGoLeft = false;

    private int currentLane = 1;


    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
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

        //controller.Move(Vector3.Lerp(beforeSideMovePosition, movePlayer * Time.fixedDeltaTime, 1f));
    }
    void Update()
    {
        if (Time.time - startedRollingTime > rollingAnimationDuration && isRolling)
        {
            SetDefaultControllerCenter();
            RollEnded();
            isRolling = false;
        }
        if (Time.time - startedJumpingTime > jumpingDuration && isJumping)
        {
            //SetDefaultControllerCenter();
            JumpEnded();
            //isJumping = false;
        }
        if (Time.time < startingAnimationDuration)
        {
            controller.Move(Vector3.up * verticalVelocity);
            controller.Move(Vector3.forward * playerRunningSpeed * Time.deltaTime);
            return;
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Running"))
        {
            if (wantsToJump)// && !isRolling)
            {

                if (isRolling)
                {
                    //PlayJumpFromRoll();
                    PlayJump();
                    isRolling = false;
                    RollEnded();
                }
                else
                {
                    PlayJump();
                }
                //PlayJump();
                wantsToJump = false;

                //MovePlayer();
                //return;
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
        }

        if (Input.GetKeyDown("d"))
        {
            wantsToGoRight = true;
            wantsToGoLeft = false;
            MoveToSide(true);
        }

        if (Input.GetKeyDown("a"))
        {
            wantsToGoLeft = true;
            wantsToGoRight = false;
            MoveToSide(false);
        }

        if (isJumping == true)
        {
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
        }



        //if (wantsToGoRight)
        //{
        //    GoRight();
        //}
        //else if (wantsToGoLeft)
        //{
        //    GoLeft();
        //}

        //transform.position = Vector3.Lerp(beforeSideMovePosition, movePlayer, 2f * Time.deltaTime);
        //controller.Move(Vector3.Lerp(beforeSideMovePosition, movePlayer * Time.fixedDeltaTime, 2f));

        MovePlayer();

    }

    void MoveToSide(bool goingRight)
    {
        currentLane += goingRight ? 1 : -1;
        currentLane = Mathf.Clamp(currentLane, 0, 2);
        print("Current lane : " + currentLane);
    }

    void MovePlayer()
    {
        Vector3 targetVector = Vector3.forward * transform.position.z;

        if (currentLane == 0)
        {
            targetVector += Vector3.left * moveRightLeftDistance;
        }
        else if (currentLane == 2)
        {
            targetVector += Vector3.right * moveRightLeftDistance;
        }

        movePlayer = Vector3.zero;
        movePlayer.x = (targetVector - transform.position).normalized.x * playerRunningSpeed;
        movePlayer.y = verticalVelocity;
        movePlayer.z = playerRunningSpeed;

        controller.Move(movePlayer * Time.deltaTime);
    }

    void GoRight()
    {
        if (currentLane == 3)
        {
            // DEAD
            //controller.enabled = false;
        }
        currentLane++;
        movePlayer.x = moveRightLeftDistance;
        wantsToGoRight = false;
    }

    void GoLeft()
    {
        if (currentLane == 1)
        {
            // DEAD
            //controller.enabled = false;
        }
        currentLane--;
        movePlayer.x = -moveRightLeftDistance;
        wantsToGoLeft = false;
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

        //SetDefaultControllerCenter();
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
        animator.SetBool("WantsToJumpFromRoll", false);
        animator.SetBool("Landing", true);
        isJumping = false;
        //controller.center = new Vector3(controller.center.x, 1.3f, controller.center.z);
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
}