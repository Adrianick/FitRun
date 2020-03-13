using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 9.0f;
    private float jumpForceMultiplier = 7.0f;
    private float gravity = 13.0f;
    private float animationDuration = 1.5f;

    private bool isJumping = false;
    private bool wantsToRoll = false;

    private Vector3 movePlayer;


    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (Time.time < animationDuration)
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
        animator.SetBool("Roll", true);
        //animator.speed = 0.8f;
        controller.height = 1.5f;
        movePlayer.x = 0;
        movePlayer.y = 0.8f;
        movePlayer.z = 0;
        controller.center = movePlayer;
    }
    void RollEnded()
    {
        animator.speed = 1;
        animator.SetBool("Roll", false);
        controller.height = 2.5f;
        movePlayer.x = 0;
        movePlayer.y = 1.3f;
        movePlayer.z = 0;
        controller.center = movePlayer;
        wantsToRoll = false;
    }
    void JumpEnded()
    {
        //animator.speed = 1;
        //animator.SetBool("Jump", false);
        animator.SetBool("Landing", true);
        isJumping = false;
        controller.center = new Vector3(controller.center.x, 1.3f, controller.center.z);


    }
}