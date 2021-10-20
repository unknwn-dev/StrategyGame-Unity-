using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    public List<GameObject> MenuPanels;
    [HideInInspector]
    public string[] Maps;
    public int SelectedMap;
    public static int PlayersCount = 4;

    private int CurrentMenu = 0;
    [SerializeField]
    private GameObject NameField;
    [SerializeField]
    private TMP_InputField NewMapName;

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
        NameField.SetActive(true);
    }

    public void CreateMap() {
        File.CreateText(Settings.Instance.MapsFolder + "/" + NewMapName.text + ".gmps");

        NameField.SetActive(false);
    }
}
