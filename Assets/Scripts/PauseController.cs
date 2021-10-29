using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private GameObject SettingsMenu;
    [SerializeField]
    private GameObject SaveNamePanel;
    [SerializeField]
    private GameObject LoadSavePanel;
    [SerializeField]
    private TMP_InputField SaveNameInpFld;
    public int SelectedSave;

    public void OpenMenu() {
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }

    public void OpenLoadSave() {
        LoadSavePanel.SetActive(!LoadSavePanel.activeInHierarchy);
    }

    public void SaveGame() {
        Settings.Game.SaveGame(SaveNameInpFld.text);
        OpenSaveName();
    }

    public void OnSelectedSave(int num) {
        SelectedSave = num;
    }

    public void OpenSaveName() {
        SaveNamePanel.SetActive(!SaveNamePanel.activeInHierarchy);
    }

    public void LoadGame() {
        Settings.Game.LoadGame(Settings.Instance.Saves[SelectedSave].Replace(Application.dataPath + "/Saves\\", "").Replace(".gmsv", ""));
        OpenLoadSave();
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
