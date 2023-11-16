using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the Left Shift key is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Player.isRunning = true;
        }
        else
        {
            // Reset isRunning if the Left Shift key is not pressed
            Player.isRunning = false;
        }
    }
}
