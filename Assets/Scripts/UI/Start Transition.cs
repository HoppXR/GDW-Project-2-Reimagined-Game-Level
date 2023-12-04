using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTransition : MonoBehaviour
{
    public Animator transition;

    void Start()
    {
        transition.SetTrigger("End");
    }
}
