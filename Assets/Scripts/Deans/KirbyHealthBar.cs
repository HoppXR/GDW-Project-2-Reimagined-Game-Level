using UnityEngine;
using UnityEngine.UI;

public class KirbyHealthBar : MonoBehaviour
{
    public Player player;  // Reference to the Player script
    public Slider healthSlider;  // Reference to the Slider component

    void Start()
    {
        // Ensure that the references are set correctly
        if (player == null)
        {
            Debug.LogError("Player reference not set in KirbyHealthBar script!");
            return;
        }

        if (healthSlider == null)
        {
            Debug.LogError("Slider reference not set in KirbyHealthBar script!");
            return;
        }
    }

    void Update()
    {
        // Update the slider value based on the player's health
        if (player != null && healthSlider != null)
        {
            float normalizedHealth = player.GetCurrentHealth() / player.GetMaxHealth();
            healthSlider.value = normalizedHealth;
        }
    }
}
