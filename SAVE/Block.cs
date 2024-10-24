using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position; // Initialiser targetPosition avec la position de départ
    }

    void Update()
    {
        // Animer le mouvement vers la position cible
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
        }
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
