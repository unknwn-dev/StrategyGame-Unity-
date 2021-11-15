using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public enum MenuPanel
{
    MainMenu,
    NewGame,
    LoadGame,
    MapEditor,
    Settings,
    Exit
}

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    public static Action<MenuPanel> OpenMenu;

    public Settings Settings;

    public List<GameObject> MenuPanels;
    [HideInInspector]
    public string[] Maps;
    public string[] Saves;
    public int SelectedMap;
    public int SelectedSave;
    public static int PlayersCount = 4;

    private int CurrentMenu = 0;
    [SerializeField]
    private GameObject NameField;
    [SerializeField]
    private TMP_InputField NewMapName;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    public void CallMenuElement(int num) {
        CurrentMenu = num;
        MenuPanels[0].SetActive(false);
        MenuPanels[num].SetActive(true);
    }

    public void OnSelectMap(int num) {
        SelectedMap = num;
    }

    public void OnSelectSave(int num) {
        SelectedSave = num;
    }

    public void OnLoadGame() {
        Settings.Game.SaveName = Settings.Instance.Saves[SelectedSave].Replace(Application.dataPath + "/Saves\\", "").Replace(".gmsv", "");
        SceneManager.LoadScene("MainScene");
    }

    public void SetPlayersCount(int num) {
        PlayersCount = num+2;
    }

    public void ReturnToMenu() {
        MenuPanels[0].SetActive(true);
        MenuPanels[CurrentMenu].SetActive(false);
    }

    public void Play() {
        Settings.Game = new Game();
        SceneManager.LoadScene("MainScene");
    }

    public void NewMap() {
        NameField.SetActive(true);
    }

    public void CreateMap() {

        string mapPath = Settings.Instance.MapsFolder + "/" + NewMapName.text + ".gmps";

        if (!File.Exists(mapPath)){
            File.CreateText(mapPath);
            NameField.SetActive(false);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
