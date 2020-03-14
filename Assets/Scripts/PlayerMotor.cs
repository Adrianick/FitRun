using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private Vector3 movePlayer;

    private readonly float startingAnimationDuration = 0.5f;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 13.0f;
    private float jumpForceMultiplier = 6.5f;
    private float gravity = 24.0f;
    private float rollingAnimationDuration = 0.1f;
    private float startedRollingTime;

    private bool isJumping = false;
    private bool isRolling = false;
    private bool wantsToRoll = false;
    private bool wantsToJump = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Time.time - startedRollingTime > (rollingAnimationDuration * animator.speed) && isRolling)
        {
            //SetDefaultControllerCenter();
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
            if (wantsToJump)
            {
                PlayJump();
                wantsToJump = false;
            }
        }

        if (Input.GetKeyDown("space") && !isRolling)
        {
            wantsToJump = true;
            wantsToRoll = false;
        }

        if (Input.GetKeyDown("s"))
        {
            wantsToRoll = true;
            wantsToJump = false;
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

        //SetRollingControllerCenter();
    }
    void RollEnded()
    {
        animator.SetBool("Roll", false);

        //SetDefaultControllerCenter();
        animator.speed = 1;
    }

    void PlayJump()
    {
        animator.SetBool("Jump", true);
        isJumping = true;
        animator.speed = 1.5f;
    }
    void JumpEnded()
    {

        //animator.SetBool("Jump", false);
        animator.SetBool("Landing", true);
        isJumping = false;
        controller.center = new Vector3(controller.center.x, 1.3f, controller.center.z);
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