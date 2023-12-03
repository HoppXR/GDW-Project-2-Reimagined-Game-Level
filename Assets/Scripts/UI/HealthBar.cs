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


        if (enemy == null)
        {

        }

        if (slider == null)
        {

        }
    }


    void Update()
    {
        if (enemy != null && slider != null)
        {
            float fillValue = enemy.currentHealth / enemy.maxHealth;
            slider.value = fillValue;
        }
        else
        {

        }
    }
}
