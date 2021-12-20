using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class SerializibleUnit 
{
    public int OwnerID;
    public Unit.UnitType Type;
    public int HP;
    public int Damage;
    public int MaintenanceCost;
    public int MovePoints;
    public int BuyCost;
    public List<SerializibleV3Int> MovementPath = new List<SerializibleV3Int>();
    public SerializibleV3Int CurrentCell;

    public SerializibleUnit(Unit unit) {
        OwnerID = unit.Owner.ID;
        Type = unit.Type;
        HP = unit.HP;
        Damage = unit.Damage;
        MaintenanceCost = unit.MaintenanceCost;
        MovePoints = unit.MovePoints;
        BuyCost = unit.BuyCost;
        CurrentCell = new SerializibleV3Int(unit.CurrentCell.CellPos);
        foreach(var pos in unit.MovementPath) {
            MovementPath.Add(new SerializibleV3Int(pos));
        }
    }

    public Unit ToUnit(List<Player> players) {
        Unit unit = new Unit();
        unit.Type = Type;
        unit.HP = HP;
        unit.Damage = Damage;
        unit.MaintenanceCost = MaintenanceCost;
        unit.MovePoints = MovePoints;
        unit.BuyCost = BuyCost;

        foreach (var p in players) {
            if (p.ID == OwnerID) {
                unit.Owner = p;
                break;
            }
        }
        foreach(var c in MovementPath) {
            unit.MovementPath.Add(c.ToUnityVector());
        }

        unit.UnitTile = (Tile)Tile.CreateInstance("Tile");
        unit.UnitTile.sprite = Settings.Instance.UnitTypes[(int)Type].UnitTile.sprite;

        Color UnitColor = new Color();
        UnitColor.r = unit.Owner.PlayerColor.r - 0.2f;
        UnitColor.g = unit.Owner.PlayerColor.g - 0.2f;
        UnitColor.b = unit.Owner.PlayerColor.b - 0.2f;
        UnitColor.a = 1;

        unit.UnitTile.color = UnitColor;

        return unit;
    }
}
