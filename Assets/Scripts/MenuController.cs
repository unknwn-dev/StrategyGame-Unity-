using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    public List<GameObject> MenuPanels;
    public string[] Maps;
    public int SelectedMap;
    public static int PlayersCount = 4;

    private int CurrentMenu = 0;

    private void Start() {
        Instance = this;
    }

    public void CallMenuElement(int num) {
        CurrentMenu = num;
        MenuPanels[0].SetActive(false);
        MenuPanels[num].SetActive(true);
    }

    public void OnSelectMap(int num) {
        SelectedMap = num;
        Debug.Log(num);
    }

    public void SetPlayersCount(int num) {
        PlayersCount = num+2;
    }

    public void ReturnToMenu() {
        MenuPanels[0].SetActive(true);
        MenuPanels[CurrentMenu].SetActive(false);
    }

    public void Play() {
        SceneManager.LoadScene("MainScene");
    }

    public void NewMap() {

    }
}
