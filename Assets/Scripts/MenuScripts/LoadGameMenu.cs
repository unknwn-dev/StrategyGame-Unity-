using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameMenu : MenuBase
{
    public void OnLoadGame()
    {
        Settings.Game.SaveName = Settings.Instance.Saves[MenuController.Instance.SelectedSave].Replace(Application.dataPath + "/Saves\\", "").Replace(".gmsv", "");
        SceneManager.LoadScene("MainScene");
    }
}
