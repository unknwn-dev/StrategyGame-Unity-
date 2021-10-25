using UnityEngine.UI;
using TMPro;

public class GUIController {

    public static void UpdateGui() {
        GameController mscr = GameController.Instance;
        mscr.GameGui.GetComponentInChildren<TMP_Text>().text = $"{mscr.Players[mscr.PlayerStep].PlayerName}\nSteps:{mscr.Steps}";
        mscr.Money.GetComponentInChildren<TMP_Text>().text = mscr.Players[mscr.PlayerStep].Money.ToString();
        mscr.PlayerColor.GetComponent<Image>().color = mscr.Players[mscr.PlayerStep].PlayerColor;
    }
}
