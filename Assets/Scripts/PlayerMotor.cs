using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(Vector3.forward * 4.0f * Time.deltaTime);
    }
}
