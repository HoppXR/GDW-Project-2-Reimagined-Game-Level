using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Enemy enemy;

    private Image healthBarImage;
    
    // temp slider game object for health bar
    public Image fillImage;
    private Slider slider;

    void Start()
    {
        // temp
        healthBarImage = GetComponent<Image>();
        slider = GetComponent<Slider>();

        if (enemy == null)
        {
            Debug.LogError("HealthBar: Enemy reference not set!");
        }
    }


    void Update()
    {
        if (enemy != null)
        {
            // temp healthbar
            float fillValue = enemy.currentHealth / enemy.maxHealth;
            slider.value = fillValue;

            //UpdateHealthBar(enemy.GetHealthPercentage());
        }
    }

    /*
    void UpdateHealthBar(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);

        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = percentage;
        }
        else
        {
            Debug.LogError("HealthBar: Image component not found!");
        }
    }
    */
}
