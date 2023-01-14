using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region vars
    [SerializeField] private GameObject escMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject pauseMenu;
    #endregion

    public void ContinueGame()
    {
        Debug.Log("resume");
        escMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoOptionsMenu()
    {
        Debug.Log("options");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
