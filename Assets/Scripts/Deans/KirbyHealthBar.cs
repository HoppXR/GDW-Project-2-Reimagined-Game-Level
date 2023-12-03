using UnityEngine;
using UnityEngine.UI;

public class KirbyHealthBar : MonoBehaviour
{
    public Player player; 
    public Slider healthSlider; 

    void Start()
    {
        if (player == null)
        {
            return;
        }

        if (healthSlider == null)
        {
            return;
        }
    }

    void Update()
    {
        if (player != null && healthSlider != null)
        {
            float normalizedHealth = player.GetCurrentHealth() / player.GetMaxHealth();
            healthSlider.value = normalizedHealth;
        }
    }
}
