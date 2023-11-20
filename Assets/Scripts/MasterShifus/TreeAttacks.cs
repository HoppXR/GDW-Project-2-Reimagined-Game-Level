using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttacks : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector3 lTreeAirAttack = new Vector3(-42.16f, -9.04f, 0f);
    public Vector3 rTreeAirAttack = new Vector3(8.43f, -9.04f, 0f);
    [SerializeField] private float AirTravelSpeed;
    [SerializeField] private Rigidbody2D AirPrefab;
    [SerializeField] private float AirDelay = 0.3f;
    private WaitForSeconds delay;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            LAirAttack();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            RAirAttack();
        }
    }

    //Instantiate at a set location. Add force
    public void LAirAttack()
    {
        delay = new WaitForSeconds(AirDelay);
        StartCoroutine(lTProjectile());
    }

    private IEnumerator lTProjectile()
    {
        for (int i = 0; i < 15; i++)
        {
            Rigidbody2D AirProjectiles =
                Instantiate(AirPrefab, lTreeAirAttack, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Rigidbody2D>();
            
            if (AirProjectiles != null)
            {
                float randomAngle = UnityEngine.Random.Range(-30f, 30f);
                Vector2 direction = Quaternion.Euler(0f, 0f, randomAngle) * Vector2.right;
                direction.Normalize();
                AirProjectiles.AddForce(AirTravelSpeed * direction, ForceMode2D.Impulse);
                Destroy(AirProjectiles.gameObject, 5);
            }

            yield return delay;
        }
    }
    
    public void RAirAttack()
    {
        delay = new WaitForSeconds(AirDelay);
        StartCoroutine(rTProjectile());
    }

    private IEnumerator rTProjectile()
    {
        for (int i = 0; i < 15; i++)
        {
            Rigidbody2D AirProjectiles =
                Instantiate(AirPrefab, rTreeAirAttack, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Rigidbody2D>();
        
            if (AirProjectiles != null)
            {
                // Generate a random angle within a range for the spread
                float randomSpreadAngle = UnityEngine.Random.Range(-30f, 30f);

                // Adjust the X component to make the projectile move towards the left
                Vector2 leftDirection = Quaternion.Euler(0f, 0f, randomSpreadAngle) * Vector2.left;

                // Normalize the vector to ensure consistent speed in all directions
                leftDirection.Normalize();

                AirProjectiles.AddForce(AirTravelSpeed * leftDirection, ForceMode2D.Impulse);
                Destroy(AirProjectiles.gameObject, 5);
            }

            yield return delay;
        }
    }


    
    //Instantiate randomly at a range. Drop 3
    public void DropApples()
    {
        
    }
    
    
    //Instantiate randomly at a range
    public void DropAppleSpike()
    {
        
    }
    
    
    //Instantiate at a set location. Transform or add force
    public void TrunkAttack()
    {
        
    }
    
    
    public void TripleTrunkAttack()
    {
        
    }

    
    //Instantiate randomly at a range. Drop 6
    public void SpinAttack()
    {
        
    }
    
    
    
}
