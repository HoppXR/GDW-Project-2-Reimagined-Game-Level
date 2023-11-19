using UnityEngine;

public class InhalableObject : MonoBehaviour
{
    private const string InhalableTag = "Inhalable";

    // Rigidbody attached to the object
    private Rigidbody2D rb;

    private Collider2D collider;

    // Initialization
    void Start()
    {
        // Ensure the object has a Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Inhalable object must have a Rigidbody2D.");
            enabled = false; // Disable the script
        }

        // Assign the tag
        gameObject.tag = InhalableTag;
    }

    // Method to destroy the object
    public void DestroyObject()
    {
        // Add any specific destruction effects or logic here
        // For example, you can play a particle effect before destroying the object

        // Destroy the game object
        Destroy(gameObject);
    }
}
