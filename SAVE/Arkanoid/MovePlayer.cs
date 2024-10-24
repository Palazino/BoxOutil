using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement
    public float leftLimit = -5f; // Limite gauche de la scène
    public float rightLimit = 5f; // Limite droite de la scène

    private bool movingRight = true; // Direction actuelle du mouvement
    private bool isMoving = false; // Indique si l'objet doit bouger

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movingRight = false;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movingRight = true;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            MOVE();
        }
    }

    private void MOVE()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightLimit)
            {
                transform.position = new Vector2(rightLimit, transform.position.y);
                isMoving = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftLimit)
            {
                transform.position = new Vector2(leftLimit, transform.position.y);
                isMoving = false;
            }
        }
    }
}
