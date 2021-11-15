using System.Collections;
using System.Collections.Generic;
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
        if(menu == Panel)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
