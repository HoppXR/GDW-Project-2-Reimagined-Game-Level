using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Enemy enemy;

    private Image healthBarImage;

    [SerializeField] private Slider slider;


    void Start()
    {
        Debug.Log("HealthBar Start()");


        if (enemy == null)
        {
            Debug.LogError("HealthBar: Enemy reference not set!");
        }

        if (slider == null)
        {
            Debug.LogError("HealthBar: Slider reference not set!");
        }
    }


    void Update()
    {
        if (enemy != null && slider != null)
        {
            // temp healthbar
            float fillValue = enemy.currentHealth / enemy.maxHealth;
            slider.value = fillValue;
        }
        else
        {

        }
    }
}
