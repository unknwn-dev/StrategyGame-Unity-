using System.Collections.Generic;
using UnityEngine;

public class Cell {
    private Unit units;
    public Unit Units {
        get {
            return units;
        }
        set {
            units = value;
            if (value != null)
                value.CurrentCell = this;
        }
    }
    public Vector3Int CellPos;
    public bool IsGround;
    public Recources Rec;
    public Player Owner;
    public Building Building;

    //Finder values
    public int g;
    public int h;
    public int F {
        get {
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

    public IEnumerable<Vector3Int> Neighbors() {
        Vector3Int[] directions = (CellPos.y % 2) == 0 ?
             directions_when_y_is_even :
             directions_when_y_is_odd;
        foreach (var direction in directions) {
            Vector3Int neighborPos = CellPos + direction;
            yield return neighborPos;
        }
    }

    public static List<Cell> GetNeighborCellsInRange(Cell currCell, int range) {
        List<Cell> result = new List<Cell>();

        foreach (var cell in currCell.GetNeighborCells()) {
            int tempRange = range - cell.Rec.MPForMove;

            if (tempRange > 0 && cell.IsGround) {
                result.Add(cell);
                result.AddRange(Cell.GetNeighborCellsInRange(cell, tempRange));
            }
            if (tempRange == 0 && cell.IsGround) {
                result.Add(cell);
            }
        }

        return result;
    }

    public List<Cell> GetNeighborCells() {
        List<Cell> result = new List<Cell>();
        GameController MScrInst = GameController.Instance;

        foreach (var neighbor in Neighbors()) {
            if (MScrInst.World.ContainsKey(neighbor) && MScrInst.World[neighbor].IsGround) {
                result.Add(MScrInst.World[neighbor]);
            }
        }
        return result;
    }

    public void UpdateOwn() {
        if (Units != null && Owner == null) {
            GameController.Instance.UnitsTilemap.SetTile(CellPos, Units.UnitTile);
        }
        else if (Units != null && Owner != null) {
            GameController.Instance.GroundTilemap.SetTile(CellPos, Owner.PlayerTile);

            GameController.Instance.UnitsTilemap.SetTile(CellPos, Units.UnitTile);
        }
        else if (Owner != null) {
            GameController.Instance.GroundTilemap.SetTile(CellPos, Owner.PlayerTile);
            GameController.Instance.UnitsTilemap.SetTile(CellPos, null);
        }
        else {

            GameController.Instance.GroundTilemap.SetTile(CellPos, GameController.Instance.Settings.GroundTile);
            GameController.Instance.UnitsTilemap.SetTile(CellPos, null);
        }
    }

    public Cell(Unit units, Vector3Int pos, bool isGround, Recources recources) {
        Units = units;
        CellPos = pos;
        IsGround = isGround;
        Rec = recources;
    }

    public void AddUnits(Cell OtherCell) {
        if (OtherCell.Units.MovePoints <= 0) return;

        if (Building != null && Building.Type == Building.BuildType.Castle && Building.Owner != OtherCell.Units.Owner && OtherCell.Units.MovePoints > 0) {
            Building.HP -= OtherCell.Units.Damage;
            OtherCell.Rec.MPForMove = 0;
            if (Building.HP <= 0) {
                Owner = OtherCell.Units.Owner;
                Building = null;
                foreach (var neighbor in Neighbors()) {
                    GameController.Instance.World[neighbor].Owner = null;
                    GameController.Instance.World[neighbor].UpdateOwn();
                }
                Units.BuildCity(this);
            }
            return;
        }

        if (Units == null) {
            Units = OtherCell.Units;
            OtherCell.Units = null;
            OtherCell.UpdateOwn();
            UpdateOwn();
            Units.CurrentCell = this;
            return;
        }

        bool IsUnitOnHisCell = Units.Owner == Owner;

        int ResultHP, OthCellResultHP;


        if (IsUnitOnHisCell) {
            ResultHP = (Units.HP + GameController.Instance.Settings.TerritoryHPBonus) - OtherCell.Units.Damage;
            OthCellResultHP = OtherCell.Units.HP - Mathf.RoundToInt(Units.Damage * 0.7f);
        }
        else {
            ResultHP = Units.HP - OtherCell.Units.Damage;
            OthCellResultHP = OtherCell.Units.HP - Units.Damage;
        }

        if (ResultHP > 0 && OthCellResultHP > 0) {
            Units.HP = ResultHP;
            OtherCell.Units.HP = OthCellResultHP;
            OtherCell.Rec.MPForMove = 0;
        }
        else if (ResultHP <= 0 && OthCellResultHP > 0) {
            OtherCell.Rec.MPForMove = 0;
            Units = OtherCell.Units;
            OtherCell.Units = null;
            Units.HP = OthCellResultHP;
            Units.CurrentCell = this;
        }
        else if (ResultHP > 0 && OthCellResultHP <= 0) {
            OtherCell.Units = null;
            Units.HP = ResultHP;
        }

        else if (ResultHP <= 0 && OthCellResultHP <= 0) {
            OtherCell.Units = null;
            Units = null;
        }


        OtherCell.UpdateOwn();
        UpdateOwn();
    }
}
