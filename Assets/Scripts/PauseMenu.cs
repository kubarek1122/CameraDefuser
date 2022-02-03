using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    bool menuOpen = false;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuOpen)
        {
            menuOpen = !menuOpen;
            OpenMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuOpen)
        {
            menuOpen = !menuOpen;
            CloseMenu();
        }
    }
    void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        panel.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        menuOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        panel.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    public void Return()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
