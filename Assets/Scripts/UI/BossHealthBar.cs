using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Enemy[] enemies; 

    private Image healthBarImage;

    [SerializeField] private Slider slider;

    void Start()
    {
        if (enemies == null || enemies.Length == 0)
        {
            return;
        }

        if (slider == null)
        {
            return;
        }
    }

    void Update()
    {
        if (enemies != null && enemies.Length > 0 && slider != null)
        {
            float totalMaxHealth = 0f;
            float totalCurrentHealth = 0f;

            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    totalMaxHealth += enemy.maxHealth;
                    totalCurrentHealth += enemy.currentHealth;
                }
            }

            float fillValue = totalCurrentHealth / totalMaxHealth;
            slider.value = fillValue;
        }
    }
}
