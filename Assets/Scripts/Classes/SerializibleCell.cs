using System.Collections.Generic;

[System.Serializable]
public class SerializibleCell
{
    public SerializibleV3Int CellPos;
    public bool IsGround;
    public Recources Rec;
    public int OwnerID;
    public SerializibleUnit Unit;
    public Building Building;

    public SerializibleCell(Cell cell) {
        CellPos = new SerializibleV3Int(cell.CellPos);
        IsGround = cell.IsGround;
        Rec = cell.Rec;
        if(cell.Units != null)  Unit = new SerializibleUnit(cell.Units);
        if(cell.Owner != null) OwnerID = cell.Owner.ID;
        if (cell.Building != null) Building = cell.Building;
    }

    public Cell ToCell(List<Player> players) {
        Cell result = new Cell();

        result.CellPos = CellPos.ToUnityVector();
        result.IsGround = IsGround;
        result.Rec = Rec;
        if(Unit != null)result.Units = Unit.ToUnit(players);
        foreach(var p in players) {
            if(p.ID == OwnerID) {
                result.Owner = p;
            }
        }
        result.Building = Building;

        return result;
    }
}
