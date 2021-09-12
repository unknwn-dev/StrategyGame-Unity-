using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
        MainScript.Instance.GroundTilemap.SetTile(CellPos, Units.Owner.PlayerTile);
        Owner = Units.Owner;
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
            UpdateOwn();
        }
        else if (OtherUnits.Owner != Units.Owner && (int)Units.Type < (int)OtherUnits.Type)
        {
            Units = OtherUnits;
            UpdateOwn();
        }
        else if (OtherUnits.Owner != Units.Owner && (int)Units.Type == (int)OtherUnits.Type)
        {
            Units = OtherUnits;
            UpdateOwn();
        }
        UpdateUnitsCount();
    }

    public Unit GetUnits()
    {
        Unit outUnits = new Unit(Units.Owner, Units.Type);
        Units = null;
        UpdateUnitsCount();
        return outUnits;
    }

    public void UpdateUnitsCount()
    {
        if (Units != null)
        {
            UnitsCount.SetActive(true);
            UnitsCount.GetComponentInChildren<Text>().text = $"{Units.Owner.PlayerName}\n{Units.Type}";
        }
        else
        {
            UnitsCount.SetActive(false);
        }
    }
}
