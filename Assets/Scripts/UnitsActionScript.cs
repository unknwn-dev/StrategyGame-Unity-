using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitsActionScript : MonoBehaviour
{
    public enum ActTypes
    {
        MoveUnit=0,
        BuildCity=1,
        DeleteUnit=2
    }

    public GameObject ButtonsPanel;
    public List<GameObject> ActButtons;
    public Cell SelectedCell;

    public void InitMenu(Cell cell)
    {
        SelectedCell = cell;

        gameObject.SetActive(true);

        if(cell.Units.Type == Unit.UnitType.Citizen || cell.Units.Type == Unit.UnitType.Warrior)
        {
            ActButtons.Add(CreateButtonInstance(ActTypes.MoveUnit));
            ActButtons.Add(CreateButtonInstance(ActTypes.DeleteUnit));
        }
        else if (cell.Units.Type == Unit.UnitType.Settlers)
        {
            ActButtons.Add(CreateButtonInstance(ActTypes.MoveUnit));
            ActButtons.Add(CreateButtonInstance(ActTypes.DeleteUnit));
            ActButtons.Add(CreateButtonInstance(ActTypes.BuildCity));
        }
    }
    public GameObject CreateButtonInstance(ActTypes type)
    {
        GameObject Button = Instantiate(MainScript.Instance.Settings.UnitsActionButtonPrefab, ButtonsPanel.transform);

        if (type == ActTypes.MoveUnit) 
        {
            Button.GetComponent<Button>().onClick.AddListener(OnMoveUnitPressed);
            Button.GetComponentInChildren<TextMeshProUGUI>().SetText("Move\nUnit");
        }
        else if (type == ActTypes.BuildCity)
        {
            Button.GetComponent<Button>().onClick.AddListener(OnBuildCityPressed);
            Button.GetComponentInChildren<TextMeshProUGUI>().SetText("Build\nCity");
        }
        else if (type == ActTypes.DeleteUnit)
        {
            Button.GetComponent<Button>().onClick.AddListener(OnDeleteUnitPressed);
            Button.GetComponentInChildren<TextMeshProUGUI>().SetText("Delete\nUnit");
        }

        gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText($"{SelectedCell.Units.Type}\nHP:{SelectedCell.Units.HP}  Dmg:{SelectedCell.Units.Damage}  Mtnc cost:{SelectedCell.Units.MaintenanceCost}");

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
    }

    public void OnBuildCityPressed()
    {
        SelectedCell.Units.BuildCity(SelectedCell);
    }

    public void OnDeleteUnitPressed()
    {
        SelectedCell.Units = null;
        SelectedCell.UpdateOwn();
        CloseMenu();
    }

}
