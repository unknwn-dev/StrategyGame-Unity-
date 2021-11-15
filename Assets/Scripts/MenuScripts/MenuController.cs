using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

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
    public GameObject NameField;
    public TMP_InputField NewMapName;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) {
            Destroy(gameObject);
        }
        StartCoroutine(waitForMenusInit());
    }

    IEnumerator waitForMenusInit()
    {
        while(OpenMenu == null || OpenMenu.GetInvocationList().Length < Enum.GetNames(typeof(MenuPanel)).Length)
        {
            yield return null;
        }

        OpenMenu.Invoke(MenuPanel.MainMenu);
    }

    public void CallMenuElement(int menu) {
        OpenMenu.Invoke((MenuPanel)menu);
    }

    public void CallMenuElementEnum(MenuPanel menu)
    {
        OpenMenu.Invoke((MenuPanel)menu);
    }

    public void OnSelectMap(int num) {
        SelectedMap = num;
    }

    public void OnSelectSave(int num) {
        SelectedSave = num;
    }

    public void SetPlayersCount(int num) {
        PlayersCount = num+2;
    }

    public void ReturnToMenu() {
        OpenMenu.Invoke(MenuPanel.MainMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
