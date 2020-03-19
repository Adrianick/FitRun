using UnityEngine;

public class FenceHit : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    private CharacterController controller;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        print("Te-ai lovit da gard wei!");
        animator = controller.GetComponent<Animator>();
        animator.SetBool("GotHit", true);
        this.enabled = false;
    }
}
