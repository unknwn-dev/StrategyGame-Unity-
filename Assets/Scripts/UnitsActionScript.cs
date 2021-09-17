using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitsActionScript : MonoBehaviour
{
    public enum ActTypes
    {
        BuildCity=0,
        DeleteUnit=1
    }

    public GameObject ButtonsPanel;
    public List<GameObject> ActButtons;
    public Cell SelectedCell;

    public void InitMenu(Cell cell, bool isAct)
    {
        if (!cell.IsGround)
        {
            if (gameObject.activeInHierarchy)
            {
                CloseMenu();
            }
            return;
        }

        SelectedCell = cell;

        gameObject.SetActive(true);

        ButtonsPanel.SetActive(isAct);


        if(cell.Units == null || !isAct)
        {
            CellInfo();
        }
        else if(cell.Units.Type == Unit.UnitType.Citizen || cell.Units.Type == Unit.UnitType.Warrior)
        {
            ActButtons.Add(CreateButtonInstance(ActTypes.DeleteUnit));
        }
        else if (cell.Units.Type == Unit.UnitType.Settlers)
        {
            ActButtons.Add(CreateButtonInstance(ActTypes.DeleteUnit));
            ActButtons.Add(CreateButtonInstance(ActTypes.BuildCity));
        }
    }

    public void CellInfo()
    {
        if(SelectedCell.Units != null)
            gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText($"{SelectedCell.Units.Type}\nHP:{SelectedCell.Units.HP}  Dmg:{SelectedCell.Units.Damage}  Mtnc cost:{SelectedCell.Units.MaintenanceCost}");

        else if (SelectedCell.Building != null)
            gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText($"{SelectedCell.Building.Type}\nHP:{SelectedCell.Building.HP}  Type:{SelectedCell.Rec.Type}  Income:{SelectedCell.Rec.Income}");
        
        else
            gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText($"{SelectedCell.Rec.Type}\nIncome:{SelectedCell.Rec.Income}");
    }

    public GameObject CreateButtonInstance(ActTypes type)
    {
        GameObject Button = Instantiate(MainScript.Instance.Settings.UnitsActionButtonPrefab, ButtonsPanel.transform);

        if (type == ActTypes.BuildCity)
        {
            Button.GetComponent<Button>().onClick.AddListener(OnBuildCityPressed);
            Button.GetComponentInChildren<TextMeshProUGUI>().SetText("Build\nCity");
        }
        else if (type == ActTypes.DeleteUnit)
        {
            Button.GetComponent<Button>().onClick.AddListener(OnDeleteUnitPressed);
            Button.GetComponentInChildren<TextMeshProUGUI>().SetText("Delete\nUnit");
        }

        gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText($"{SelectedCell.Units.Type}\nHP:{SelectedCell.Units.HP}  Dmg:{SelectedCell.Units.Damage}  MP:{SelectedCell.Units.MovePoints}  Mtnc cost:{SelectedCell.Units.MaintenanceCost}");

        return Button;
    }

    public void CloseMenu()
    {
        foreach(GameObject gO in ActButtons)
        {
            Destroy(gO);
        }

        MainScript.Instance.IsCanMove = false;
        ActButtons = new List<GameObject>();

        gameObject.SetActive(false);
    }

    public void OnMoveUnitPressed()
    {
        MainScript.Instance.IsCanMove = true;
        gameObject.SetActive(false);
    }

    public void OnBuildCityPressed()
    {
        SelectedCell.Units.BuildCity(SelectedCell);
        MainScript.Instance.SelectedCell = null;
        CloseMenu();
        MainScript.Instance.ClearMoveFieldTilemap();
    }

    public void OnDeleteUnitPressed()
    {
        SelectedCell.Units = null;
        MainScript.Instance.SelectedCell = null;
        SelectedCell.UpdateOwn();
        CloseMenu();
        MainScript.Instance.ClearMoveFieldTilemap();
    }

}
