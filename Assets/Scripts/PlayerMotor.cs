using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private Vector3 movePlayer;

    private readonly float startingAnimationDuration = 1.5f;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 7.0f;
    private float jumpForceMultiplier = 6.0f;
    private float gravity = 22.0f;
    private float rollingAnimationDuration = 0.3f;
    private float startedRollingTime;

    private bool isJumping = false;
    private bool isRolling = false;
    private bool wantsToRoll = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Time.time - startedRollingTime > (rollingAnimationDuration * animator.speed) && isRolling)
        {
            SetDefaultControllerCenter();
            isRolling = false;
        }
        if (Time.time < startingAnimationDuration)
        {
            controller.Move(Vector3.up * verticalVelocity);
            controller.Move(Vector3.forward * playerRunningSpeed * Time.deltaTime);
            return;
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Running"))
        {
            if (Input.GetKeyDown("space"))
            {
                animator.SetBool("Jump", true);
                isJumping = true;
            }
        }

        if (Input.GetKeyDown("s"))
        {
            wantsToRoll = true;
        }

        //if (this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Jump"))
        //{
        //    movePlayer = Vector3.zero;
        //    movePlayer.x = Input.GetAxis("Horizontal") * playerRunningSpeed;
        //    movePlayer.y = jumpForceMultiplier;
        //    movePlayer.z = playerRunningSpeed;
        //    controller.Move(movePlayer * Time.deltaTime);
        //    return;
        //}

        if (isJumping == true)
        {
            verticalVelocity = 0;

            //controller.center = new Vector3(controller.center.x, 3f, controller.center.z);

            movePlayer = Vector3.zero;
            movePlayer.x = Input.GetAxis("Horizontal") * playerRunningSpeed;
            movePlayer.y = jumpForceMultiplier;
            movePlayer.z = playerRunningSpeed;
            controller.Move(movePlayer * Time.deltaTime);
        }
        else if (controller.isGrounded)
        {
            if (wantsToRoll)
            {
                PlayRoll();
            }
            verticalVelocity = -0.5f;
            animator.SetBool("Landing", false);
            animator.SetBool("Jump", false);
        }
        else
        {
            //animator.SetBool("Landing", false);
            verticalVelocity -= gravity * Time.deltaTime;
        }


        movePlayer = Vector3.zero;
        movePlayer.x = Input.GetAxis("Horizontal") * playerRunningSpeed;
        movePlayer.y = verticalVelocity;
        movePlayer.z = playerRunningSpeed;
        controller.Move(movePlayer * Time.deltaTime);

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
        animator.speed = 1;
        animator.SetBool("Roll", false);
        wantsToRoll = false;

        //SetDefaultControllerCenter();

    }
    void JumpEnded()
    {
        //animator.speed = 1;
        //animator.SetBool("Jump", false);
        animator.SetBool("Landing", true);
        isJumping = false;
        controller.center = new Vector3(controller.center.x, 1.3f, controller.center.z);


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