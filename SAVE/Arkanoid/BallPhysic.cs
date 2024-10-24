using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall2D : MonoBehaviour
{
    public float speed = 5f; // Vitesse de la balle
    private Rigidbody2D rb; // Référence au Rigidbody2D
    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Désactive la gravité
        rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;

        // Calculer les limites de l'écran en unités du monde
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
    }

    void Update()
    {
        CheckScreenLimits();
    }

    void CheckScreenLimits()
    {
        Vector2 ballPosition = transform.position;

        // Vérifier les limites gauche et droite
        if (ballPosition.x <= -screenWidth || ballPosition.x >= screenWidth)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y); // Inverser la direction x
            ballPosition.x = Mathf.Clamp(ballPosition.x, -screenWidth, screenWidth);
        }

        // Vérifier les limites haut et bas
        if (ballPosition.y <= -screenHeight || ballPosition.y >= screenHeight)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y); // Inverser la direction y
            ballPosition.y = Mathf.Clamp(ballPosition.y, -screenHeight, screenHeight);
        }

        // Appliquer la position corrigée à la balle
        transform.position = ballPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 incomingVelocity = rb.velocity;
        Vector2 newVelocity = Vector2.Reflect(incomingVelocity, normal).normalized * speed;

        // Garder la balle en mouvement perpétuel avec un ajustement pour le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            // Améliorer le rebond en fonction de la position de la collision sur le joueur
            float hitFactor = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 direction = new Vector2(hitFactor, 1).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = newVelocity;
        }
    }
}
