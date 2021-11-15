using System.IO;

public class MapEditorMenu : MenuBase
{
    public void NewMap()
    {
        MenuController.Instance.NameField.SetActive(true);
    }

    public void CreateMap()
    {

        string mapPath = Settings.Instance.MapsFolder + "/" + MenuController.Instance.NewMapName.text + ".gmps";

        if (!File.Exists(mapPath))
        {
            File.CreateText(mapPath);
            MenuController.Instance.NameField.SetActive(false);
        }

    }
}
