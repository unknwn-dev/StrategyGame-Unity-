using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameMenu : MenuBase
{
    public void OnLoadGame()
    {
        Settings.Game.SaveName = Settings.Instance.Saves[MenuController.Instance.SelectedSave].Replace(MenuController.Instance.Settings.SaveFolder + "/", "").Replace(".gmsv", "");
        SceneManager.LoadScene("MainScene");
    }
}
