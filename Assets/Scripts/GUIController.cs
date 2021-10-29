using UnityEngine.UI;
using TMPro;

public class GUIController {

    public static void UpdateGui() {
        GameController MSC = GameController.Instance;
        MSC.GameGui.GetComponentInChildren<TMP_Text>().text = $"{Settings.Game.Players[Settings.Game.PlayerStep].PlayerName}\nSteps:{Settings.Game.Steps}";
        MSC.Money.GetComponentInChildren<TMP_Text>().text = Settings.Game.Players[Settings.Game.PlayerStep].Money.ToString();
        MSC.PlayerColor.GetComponent<Image>().color = Settings.Game.Players[Settings.Game.PlayerStep].PlayerColor;
    }
}
