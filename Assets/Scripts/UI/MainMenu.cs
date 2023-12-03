using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public Animator transition;

    public AudioSource buttonClickSound;

    public void PlayGame()
    {
        PlayButtonClickSound();

        LoadNextLevel();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Clicked");
        PlayButtonClickSound();

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
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
