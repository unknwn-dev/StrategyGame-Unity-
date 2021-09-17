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

    //Finder values
    public int g;
    public int h;
    public int F
    {
        get
        {
            if (Rec != null) 
                return g + h + Rec.MPForMove;

            return g + h;
        }
    }


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

    public List<Cell> GetNeighborCells()
    {
        List<Cell> result = new List<Cell>();
        MainScript MScrInst = MainScript.Instance;

        foreach (var neighbor in Neighbors())
        {
            if (MScrInst.World.ContainsKey(neighbor) && MScrInst.World[neighbor].IsGround)
            {
                //MScrInst.MoveFieldTilemap.SetTile(neighbor, MScrInst.Settings.MvUnitFieldTile);
                result.Add(MScrInst.World[neighbor]);
            }
        }
        return result;
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
        if (OtherCell.Units.MovePoints <= 0) return;

        if(Building != null && Building.Type == Building.BuildType.Castle && Building.Owner != OtherCell.Units.Owner && OtherCell.Units.MovePoints > 0)
        {
            Building.HP -= OtherCell.Units.Damage;
            OtherCell.Rec.MPForMove = 0;
            if(Building.HP <= 0)
            {
                Owner = OtherCell.Units.Owner;
                Building = null;
                foreach (var neighbor in Neighbors())
                {
                    MainScript.Instance.World[neighbor].Owner = null;
                    MainScript.Instance.World[neighbor].UpdateOwn();
                }
                Units.BuildCity(this);
            }
            return;
        }

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
            OthCellResultHP = OtherCell.Units.HP - Mathf.RoundToInt(Units.Damage * 0.7f);
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
            OtherCell.Rec.MPForMove = 0;
        }
        else if (ResultHP <= 0 && OthCellResultHP > 0)
        {
            OtherCell.Rec.MPForMove = 0;
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
