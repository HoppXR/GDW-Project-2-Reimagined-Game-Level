using UnityEngine;

public class InhalableObject : MonoBehaviour
{
    private const string InhalableTag = "Inhalable";

    private Rigidbody2D rb;

    private Collider2D collider;

    // Initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            enabled = false; 
        }

        gameObject.tag = InhalableTag;
    }

    public void DestroyObject()
    {

        Destroy(gameObject);
    }
}
