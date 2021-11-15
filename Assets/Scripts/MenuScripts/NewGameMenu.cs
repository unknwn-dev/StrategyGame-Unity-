using UnityEngine.SceneManagement;

public class NewGameMenu : MenuBase
{
    public void Play()
    {
        Settings.Game = new Game();
        SceneManager.LoadScene("MainScene");
    }
}
