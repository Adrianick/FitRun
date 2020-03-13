using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 9.0f;
    private float gravity = 13.0f;
    private float animationDuration = 2.3f;


    private Vector3 movePlayer;


    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Runningg"))
        {
            controller.enabled = false;

        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).nameHash
            == Animator.StringToHash("Base Layer.Running"))
        {
            if (Input.GetKeyDown("s"))
            {
                animator.SetBool("Roll", true);
                animator.speed = 0.5f;
                controller.height = 1.5f;
                movePlayer.x = 0;
                movePlayer.y = 0.8f;
                movePlayer.z = 0;
                controller.center = movePlayer;
            }
        }


        if (controller.isGrounded)
        {
            verticalVelocity = -0.2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (Time.time < animationDuration)
        {
            controller.Move(Vector3.up * verticalVelocity);
            controller.Move(Vector3.forward * playerRunningSpeed * Time.deltaTime);
            return;
        }
        movePlayer = Vector3.zero;

        movePlayer.x = Input.GetAxis("Horizontal") * playerRunningSpeed;
        movePlayer.y = verticalVelocity;
        movePlayer.z = playerRunningSpeed;


        controller.Move(movePlayer * Time.deltaTime);
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
    }
}
