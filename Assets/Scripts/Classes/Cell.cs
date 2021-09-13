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
    private GameObject UnitsCount;
    public Recources Rec;
    public Player Owner;


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
        if (Units != null)
        {
            Owner = Units.Owner;
            MainScript.Instance.GroundTilemap.SetTile(CellPos, Owner.PlayerTile);

            if(Units.Type == Unit.UnitType.Citizen)
                MainScript.Instance.UnitsTilemap.SetTile(CellPos, MainScript.Instance.Settings.CivilTile);
            if(Units.Type == Unit.UnitType.Warrior)
                MainScript.Instance.UnitsTilemap.SetTile(CellPos, MainScript.Instance.Settings.WarriorTile);
        }
        else if(Owner != null)
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

    public Cell(Unit units, Vector3Int pos, bool isGround, GameObject text, Recources recources)
    {
        Units = units;
        CellPos = pos;
        IsGround = isGround;
        UnitsCount = text;
        Rec = recources;
    }

    public void AddUnits(Unit OtherUnits)
    {
        if (Units == null)
        {
            Units = OtherUnits;
        }
        else if (OtherUnits.Owner != Units.Owner && (int)Units.Type < (int)OtherUnits.Type)
        {
            Units = OtherUnits;
        }
        UpdateOwn();
    }

    public Unit GetUnits()
    {
        Unit outUnits = new Unit(Units.Owner, Units.Type);
        Units = null;
        UpdateOwn();
        return outUnits;
    }
}
