using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject NewGamePanel;
    public static int PlayersCount = 4;

    public void NewGameClick(){
        MainPanel.SetActive(false);
        NewGamePanel.SetActive(true);
    }

    public void SetPlayersCount(int num)
    {
        PlayersCount = num+2;
    }

    public void ReturnToMenu()
    {
        MainPanel.SetActive(true);
        NewGamePanel.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }
}
