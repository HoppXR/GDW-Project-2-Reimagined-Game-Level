using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Enemy enemy;

    private Image healthBarImage;

    void Start()
    {
        healthBarImage = GetComponent<Image>();

        if (enemy == null)
        {
            Debug.LogError("HealthBar: Enemy reference not set!");
        }
    }


    void Update()
    {
        if (enemy != null)
        {
            UpdateHealthBar(enemy.GetHealthPercentage());
        }
    }

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
}
