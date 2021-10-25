using UnityEngine.UI;
using TMPro;

public class GUIController
{
    private static readonly MainScript mscr = MainScript.Instance;

    public static void UpdateGui()
    {
        if (!mscr) {
            mscr.GameGui.GetComponentInChildren<TMP_Text>().text = $"{mscr.Players[mscr.PlayerStep].PlayerName}\nSteps:{mscr.Steps}";
            mscr.Money.GetComponentInChildren<TMP_Text>().text = mscr.Players[mscr.PlayerStep].Money.ToString();
            mscr.PlayerColor.GetComponent<Image>().color = mscr.Players[mscr.PlayerStep].PlayerColor;
        }
    }   
}
