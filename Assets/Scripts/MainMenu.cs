using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu, credits;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.SetActive(true);
        credits.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }
}
