using UnityEngine;

public class MenuBase : MonoBehaviour
{
    public MenuPanel Panel;

    void Start()
    {
        MenuController.OpenMenu += OnOpen;
    }

    public void OnOpen(MenuPanel menu)
    {
        if (menu == Panel && !gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        else if(menu != Panel && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        MenuController.OpenMenu -= OnOpen;
    }
}


[System.Serializable]
public enum MenuPanel
{
    MainMenu,
    NewGame,
    LoadGame,
    MapEditor,
    Settings
}
