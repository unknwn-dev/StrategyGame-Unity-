using UnityEngine;
using UnityEngine.UI;

public class GUIController
{
    private static readonly MainScript mscr = MainScript.Instance;

    public static void UpdateGui()
    {
        mscr.GameGui.GetComponentInChildren<Text>().text = $"{mscr.Players[mscr.PlayerStep].PlayerName}\nSteps:{mscr.Steps}";
        mscr.Money.GetComponentInChildren<Text>().text = mscr.Players[mscr.PlayerStep].Money.ToString();
        mscr.PlayerColor.GetComponent<Image>().color = mscr.Players[mscr.PlayerStep].PlayerColor;
    }
}
