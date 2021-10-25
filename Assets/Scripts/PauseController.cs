using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private GameObject SettingsMenu;

    public void OpenMenu() {
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }


    public void OpenSettings() {
        SettingsMenu.SetActive(!SettingsMenu.activeInHierarchy);
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }

    public void BackToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
