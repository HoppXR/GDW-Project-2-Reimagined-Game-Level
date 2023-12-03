using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuPrefab;
    private GameObject pauseMenuInstance;

    public GameObject controlMenuPrefab;
    private GameObject controlMenuInstance;

    private GameObject currentMenu;

    public CanvasGroup[] otherCanvases;
    public AudioSource[] audioSources;

    private float[] initialVolumes;

    public Player player;

    void Start()
    {
        initialVolumes = new float[audioSources.Length];
        for (int i = 0; i < audioSources.Length; i++)
        {
            initialVolumes[i] = audioSources[i].volume;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu != null && currentMenu.activeSelf)
            {
                CloseTopMenu();
            }
            else if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        CloseTopMenu();
        Time.timeScale = 1f;
        GameIsPaused = false;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = initialVolumes[i];
        }

        foreach (CanvasGroup canvasGroup in otherCanvases)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        player.TogglePlayerInput(true);
    }

    public void Pause()
    {
        OpenCanvas(pauseMenuPrefab);
        Time.timeScale = 0f;
        GameIsPaused = true;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = initialVolumes[i] * 0.5f;
        }

        foreach (CanvasGroup canvasGroup in otherCanvases)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        player.TogglePlayerInput(false);
    }

    public void OpenControlsMenu()
    {
        OpenTopMenu(controlMenuPrefab);
    }

    void OpenTopMenu(GameObject menuPrefab)
    {
        if (menuPrefab != null)
        {
            CloseTopMenu();

            currentMenu = Instantiate(menuPrefab);
            currentMenu.SetActive(true);
            Time.timeScale = 0f; 
            player.TogglePlayerInput(false);
        }
    }

    void CloseTopMenu()
    {
        if (currentMenu != null && currentMenu.activeSelf)
        {
            Destroy(currentMenu);
            Time.timeScale = 1f; 
            player.TogglePlayerInput(true);
        }
    }

    void OpenCanvas(GameObject menuPrefab)
    {
        if (menuPrefab != null)
        {
            CloseTopMenu();

            currentMenu = Instantiate(menuPrefab);
            currentMenu.SetActive(true);
            player.TogglePlayerInput(false);
        }
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
