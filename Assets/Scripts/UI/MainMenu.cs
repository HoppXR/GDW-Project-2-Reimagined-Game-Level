using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{

    public AudioSource buttonClickSound;

    public void PlayGame()
    {
        SceneManager.LoadScene("IntroScene");
        PlayButtonClickSound();
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
}
