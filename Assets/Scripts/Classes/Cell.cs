using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Cell
{
    public Unit Units;
    public Vector3Int CellPos;
    public bool IsGround;
    public Recources Rec;
    public Player Owner;
    public Building Building;


    private static Vector3Int
        LEFT = new Vector3Int(-1, 0, 0),
        RIGHT = new Vector3Int(1, 0, 0),
        DOWN = new Vector3Int(0, -1, 0),
        DOWNLEFT = new Vector3Int(-1, -1, 0),
        DOWNRIGHT = new Vector3Int(1, -1, 0),
        UP = new Vector3Int(0, 1, 0),
        UPLEFT = new Vector3Int(-1, 1, 0),
        UPRIGHT = new Vector3Int(1, 1, 0);

    public static Vector3Int[] directions_when_y_is_even =
      { LEFT, RIGHT, DOWN, DOWNLEFT, UP, UPLEFT };
    public static Vector3Int[] directions_when_y_is_odd =
      { LEFT, RIGHT, DOWN, DOWNRIGHT, UP, UPRIGHT };

    public IEnumerable<Vector3Int> Neighbors()
    {
        Vector3Int[] directions = (CellPos.y % 2) == 0 ?
             directions_when_y_is_even :
             directions_when_y_is_odd;
        foreach (var direction in directions)
        {
            Vector3Int neighborPos = CellPos + direction;
            yield return neighborPos;
        }
    }


    public void UpdateOwn()
    {
        if (Units != null && Owner == null)
        {
            MainScript.Instance.UnitsTilemap.SetTile(CellPos, Units.UnitTile);
        }
        else if (Units != null && Owner != null)
        {
            MainScript.Instance.GroundTilemap.SetTile(CellPos, Owner.PlayerTile);

            MainScript.Instance.UnitsTilemap.SetTile(CellPos, Units.UnitTile);
        }
        else if (Owner != null)
        {
            MainScript.Instance.GroundTilemap.SetTile(CellPos, Owner.PlayerTile);
            MainScript.Instance.UnitsTilemap.SetTile(CellPos, null);
        }
        else
        {

            MainScript.Instance.GroundTilemap.SetTile(CellPos, MainScript.Instance.Settings.GroundTile);
            MainScript.Instance.UnitsTilemap.SetTile(CellPos, null);
        }
    }

    public Cell(Unit units, Vector3Int pos, bool isGround, Recources recources)
    {
        Units = units;
        CellPos = pos;
        IsGround = isGround;
        Rec = recources;
    }

    public void AddUnits(Cell OtherCell)
    {
        if (Units == null)
        {
            Units = OtherCell.Units;
            OtherCell.Units = null;
            OtherCell.UpdateOwn();
            UpdateOwn();
            return;
        }

        bool IsUnitOnHisCell = Units.Owner == Owner;

        int ResultHP, OthCellResultHP;

        if (IsUnitOnHisCell)
        {
            ResultHP = (Units.HP + MainScript.Instance.Settings.TerritoryHPBonus) - OtherCell.Units.Damage;
            OthCellResultHP = OtherCell.Units.HP - Mathf.RoundToInt(Units.Damage * 0.4f);
        }
        else
        {
            ResultHP = Units.HP - OtherCell.Units.Damage;
            OthCellResultHP = OtherCell.Units.HP - Units.Damage;
        }

        if(ResultHP > 0 && OthCellResultHP > 0)
        {
            Units.HP = ResultHP;
            OtherCell.Units.HP = OthCellResultHP;
        }
        else if (ResultHP <= 0 && OthCellResultHP > 0)
        {
            Units = OtherCell.Units;
            OtherCell.Units = null;
            Units.HP = OthCellResultHP;
        }
        else if (ResultHP > 0 && OthCellResultHP <= 0)
        {
            OtherCell.Units = null;
            Units.HP = ResultHP;
        }

        else if (ResultHP <= 0 && OthCellResultHP <= 0)
        {
            OtherCell.Units = null;
            Units = null;
        }


        OtherCell.UpdateOwn();
        UpdateOwn();
    }
}
