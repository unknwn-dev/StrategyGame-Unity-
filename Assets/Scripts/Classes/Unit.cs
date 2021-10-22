using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public class Unit {
    public enum UnitType {
        Citizen = 0,
        Warrior = 1,
        Settlers = 2,
        Null = -1,
    }

    [HideInInspector]
    public Player Owner;
    public Tile UnitTile = (Tile)Tile.CreateInstance(typeof(Tile));
    public UnitType Type;
    public int HP;
    public int Damage;
    public int MaintenanceCost;
    public int MovePoints;
    public int BuyCost;
    public List<Cell> MovementPath = new List<Cell>();
    public Cell CurrentCell;

    public Unit(Player own, UnitType type) {
        Owner = own;
        Type = type;

        Settings sett = MainScript.Instance.Settings;
        HP = sett.UnitTypes[(int)type].HP;
        Damage = sett.UnitTypes[(int)type].Damage;
        UnitTile.sprite = sett.UnitTypes[(int)type].UnitTile.sprite;
        MaintenanceCost = sett.UnitTypes[(int)type].MaintenanceCost;
        MovePoints = sett.UnitTypes[(int)type].MovePoints;

        Color UnitColor = new Color();
        UnitColor.r = Owner.PlayerColor.r - 0.2f;
        UnitColor.g = Owner.PlayerColor.g - 0.2f;
        UnitColor.b = Owner.PlayerColor.b - 0.2f;
        UnitColor.a = 1;

        UnitTile.color = UnitColor;
    }

    public void MPToMax() {
        Settings sett = MainScript.Instance.Settings;
        MovePoints = sett.UnitTypes[(int)Type].MovePoints;
    }

    public void BuildCity(Cell cell) {
        cell.Building = new Building(Building.BuildType.Castle, Owner);
        cell.Units = null;
        cell.UpdateOwn();

        MainScript MSC = MainScript.Instance;

        MSC.CellModsTilemap.SetTile(cell.CellPos, MSC.Settings.CastleTile);

        foreach (var neighbor in cell.Neighbors()) {
            if (cell.Owner == null) {
                cell.Owner = Owner;
                cell.UpdateOwn();
            }
            if (MSC.World[neighbor].Owner == null && MSC.World[neighbor].IsGround) {
                MSC.World[neighbor].Owner = Owner;
                MSC.World[neighbor].UpdateOwn();
            }
        }

        MSC.ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
    }

    public void MoveThroughtPath() {
        while (MovePoints > 0) {
            if (MovementPath != null && MovementPath.Count > 0) {
                MovementPath[0].AddUnits(CurrentCell);
                MovementPath.Remove(CurrentCell);
                MovePoints -= CurrentCell.Rec.MPForMove;
            }
            else {
                break;
            }
        }
    }
}
