using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 3.0f;
    private float gravity = 13.0f;

    private Vector3 movePlayer;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        movePlayer = Vector3.zero;

        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        movePlayer.x = Input.GetAxis("Horizontal") * playerRunningSpeed;
        movePlayer.y = verticalVelocity;
        movePlayer.z = playerRunningSpeed;


        controller.Move(movePlayer * Time.deltaTime);
    }
}
