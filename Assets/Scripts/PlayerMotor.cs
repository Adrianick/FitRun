using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private float verticalVelocity = 0.0f;
    private float playerRunningSpeed = 3.0f;
    private float gravity = 13.0f;
    private float animationDuration = 2.3f;


    private Vector3 movePlayer;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
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
}
