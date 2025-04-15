using UnityEngine;

public class Delivery : MonoBehaviour
{
    bool hasChicken = false;
    SpriteRenderer spriteRenderer;

    [SerializeField] float destroyDelay = 1f;
    [SerializeField] Color noChickenColor = new Color(1, 1, 1, 1);
    [SerializeField] Color hasChickenColor = new Color(0.9f, 0.5f, 0.0f, 1);

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chicken") && !hasChicken)
        {
            Debug.Log("Ä¡Å² ÇÈ¾÷µÊ");
            hasChicken = true;
            spriteRenderer.color = hasChickenColor;
            Destroy(collision.gameObject, destroyDelay);
        }

        if (collision.gameObject.CompareTag("Customer") && hasChicken)
        {
            Debug.Log("Ä¡Å² ¹è´ÞµÊ");
            hasChicken = false;
            spriteRenderer.color = noChickenColor;
        }

    }
}
