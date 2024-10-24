using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float hoverHeight = 2f; 

    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isHovering = false;

    void Update()
    {
        
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
        }

       // MOUVEMENT AVANT ARRIERE GAUCHE DROITE
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // SAUT
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount = 1;
            }
            else if (jumpCount == 1)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount = 2;
            }
        }
        // DOUBLE SAUT
        if (Input.GetButton("Jump") && jumpCount == 2)
        {
            isHovering = true;
            velocity.y = Mathf.Lerp(velocity.y, hoverHeight, Time.deltaTime * 2f);
        }
        else
        {
            isHovering = false;
        }

        if (!isHovering)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
