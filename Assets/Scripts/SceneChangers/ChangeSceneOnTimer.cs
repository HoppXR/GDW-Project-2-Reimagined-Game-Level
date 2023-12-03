using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{
    public Animator transition;

    public float changeTime;

    private bool transitionStarted = false;

    private void Start()
    {
        transition.SetTrigger("End");
    }
    
    private void Update()
    {
        changeTime -= Time.deltaTime;

        if (changeTime <= 0 && !transitionStarted)
        {
            transitionStarted = true;
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(ELoadLevel(SceneManager.GetActiveScene().buildIndex +1));
    }

    IEnumerator ELoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(levelIndex);
    }
}
