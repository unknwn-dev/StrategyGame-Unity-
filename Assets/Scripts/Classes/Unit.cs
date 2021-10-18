using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit
{
    public enum UnitType
    {
        Null = -1,
        Citizen = 0,
        Warrior = 1,
        Settlers = 2
    }

    public Player Owner;
    public Tile UnitTile = new Tile();
    public UnitType Type;
    public int HP;
    public int Damage;
    public int MaintenanceCost;
    public int MovePoints;
    public List<Cell> MovementPath = new List<Cell>();
    public Cell CurrentCell;

    public Unit(Player own, UnitType type)
    {
        Owner = own;
        Type = type;

        Settings sett = MainScript.Instance.Settings;

        if (type == UnitType.Citizen)
        {
            HP = sett.CitizenHP;
            Damage = sett.CitizenDmg;
            UnitTile.sprite = sett.CitizSpr;
            MaintenanceCost = sett.CitizenMaintenanceCost;
            MovePoints = sett.CitizenMP;
        }
        else if (type == UnitType.Warrior)
        {
            HP = sett.WarriorHP;
            Damage = sett.WarriorDmg;
            UnitTile.sprite = sett.WarriorSpr;
            MaintenanceCost = sett.WarriorMaintenanceCost;
            MovePoints = sett.WarriorMP;
        }
        else if (type == UnitType.Settlers)
        {
            HP = sett.SettlersHP;
            UnitTile.sprite = sett.SettlersSpr;
            Damage = 0;
            MaintenanceCost = sett.SettlersMaintenanceCost;
            MovePoints = sett.SettlersMP;
        }

        Color UnitColor = new Color();
        UnitColor.r = Owner.PlayerColor.r - 0.2f ;
        UnitColor.g = Owner.PlayerColor.g - 0.2f;
        UnitColor.b = Owner.PlayerColor.b - 0.2f;
        UnitColor.a = 1;

        UnitTile.color = UnitColor;
    }

    public void MPToMax()
    {
        Settings sett = MainScript.Instance.Settings;

        if (Type == UnitType.Citizen)
        {
            MovePoints = sett.CitizenMP;
        }
        else if (Type == UnitType.Warrior)
        {
            MovePoints = sett.WarriorMP;
        }
        else if (Type == UnitType.Settlers)
        {
            MovePoints = sett.SettlersMP;
        }
    }

    public void BuildCity(Cell cell)
    {
        cell.Building = new Building(Building.BuildType.Castle, Owner);
        cell.Units = null;
        cell.UpdateOwn();

        MainScript MSC = MainScript.Instance;

        MSC.CellModsTilemap.SetTile(cell.CellPos, MSC.Settings.CastleTile);

        foreach (var neighbor in cell.Neighbors())
        {
            if (cell.Owner == null)
            {
                cell.Owner = Owner;
                cell.UpdateOwn();
            }
            if (MSC.World[neighbor].Owner == null && MSC.World[neighbor].IsGround)
            {
                MSC.World[neighbor].Owner = Owner;
                MSC.World[neighbor].UpdateOwn();
            }
        }

        MSC.ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
    }

    public void MoveThroughtPath()
    {
        while (MovePoints > 0)
        {
            if (MovementPath != null && MovementPath.Count > 0)
            {
                MovementPath[0].AddUnits(CurrentCell);
                MovementPath.Remove(CurrentCell);
                MovePoints -= CurrentCell.Rec.MPForMove;
            }
            else
            {
                break;
            }
        }
    }
}
