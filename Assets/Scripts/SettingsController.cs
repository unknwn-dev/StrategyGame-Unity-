using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public List<GameObject> MenuPanels;

    private int CurrentMenu = 0;
    private int PreviousMenu = 0;
    public void CallMenuElement(int num)
    {
        PreviousMenu = CurrentMenu;
        CurrentMenu = num;
        MenuPanels[PreviousMenu].SetActive(false);
        MenuPanels[num].SetActive(true);
    }

}
