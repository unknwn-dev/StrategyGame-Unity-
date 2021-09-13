using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public static ShopScript Instance;
    
    public bool ShopIsOpened;
    public bool IsBuying;

    [SerializeField]
    private GameObject ShopPanel;
    private Unit.UnitType SelectedType = Unit.UnitType.Null;

    public void OpenShop()
    {
        ShopPanel.SetActive(!ShopPanel.activeInHierarchy);
        ShopIsOpened = ShopPanel.activeInHierarchy;
        IsBuying = ShopPanel.activeInHierarchy;
    }

    public void BuyUnit(int type)
    {
        SelectedType = (Unit.UnitType)type;
        OpenShop();
        IsBuying = true;
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!ShopIsOpened && SelectedType != Unit.UnitType.Null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int ClickedCell = MainScript.Instance.GroundTilemap.WorldToCell(mousePos);
                ClickedCell.z = 0;

                Dictionary<Vector3Int, Cell> World = MainScript.Instance.World;

                Player MovingPlayer = MainScript.Instance.Players[MainScript.Instance.PlayerStep];

                if (World[ClickedCell].Units == null && World[ClickedCell].Owner == MovingPlayer)
                {

                    if (SelectedType == Unit.UnitType.Citizen && MovingPlayer.Money >= MainScript.Instance.Settings.CitizenCost)
                    {
                        MainScript.Instance.Players[MainScript.Instance.PlayerStep].Money -= MainScript.Instance.Settings.CitizenCost;
                        World[ClickedCell].AddUnits(new Unit(MovingPlayer, SelectedType));
                        SelectedType = Unit.UnitType.Null;
                        IsBuying = false;
                    }

                    else if(SelectedType == Unit.UnitType.Warrior && MovingPlayer.Money >= MainScript.Instance.Settings.WarriorCost)
                    {
                        MainScript.Instance.Players[MainScript.Instance.PlayerStep].Money -= MainScript.Instance.Settings.WarriorCost;
                        World[ClickedCell].AddUnits(new Unit(MovingPlayer, SelectedType));
                        SelectedType = Unit.UnitType.Null;
                        IsBuying = false;
                    }
                }
            }
        }
    }
}
