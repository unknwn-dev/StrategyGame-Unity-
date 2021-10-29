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
    public Tile UnitTile;
    public UnitType Type;
    public int HP;
    public int Damage;
    public int MaintenanceCost;
    public int MovePoints;
    public int BuyCost;
    public List<Vector3Int> MovementPath = new List<Vector3Int>();
    public Cell CurrentCell;

    public Unit(Player own, UnitType type) {
        Owner = own;
        Type = type;
        UnitTile = (Tile)Tile.CreateInstance("Tile");
        Settings sett = GameController.Instance.Settings;
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

    public Unit() {
    }

    public void MPToMax() {
        Settings sett = GameController.Instance.Settings;
        MovePoints = sett.UnitTypes[(int)Type].MovePoints;
    }

    public void MoveThroughtPath() {
        while (MovePoints > 0) {
            if (MovementPath != null && MovementPath.Count > 1 && Settings.Game.World[MovementPath[1]].Building == null && Settings.Game.World[MovementPath[1]].Units == null) {
                Settings.Game.World[MovementPath[1]].AddUnits(CurrentCell);
                MovementPath.Remove(MovementPath[0]);
                MovePoints -= CurrentCell.Rec.MPForMove;
            }
            else {
                break;
            }
        }
    }
}
