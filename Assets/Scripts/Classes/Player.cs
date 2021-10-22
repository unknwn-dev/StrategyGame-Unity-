using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string PlayerName;
    public Color PlayerColor;
    [HideInInspector]
    public Tile PlayerTile = (Tile)Tile.CreateInstance(typeof(Tile));
    public int Money;

    public Player(string name, Color color)
    {
        PlayerName = name;
        PlayerTile.sprite = MainScript.Instance.Settings.BaseTileSprite;
        PlayerTile.color = color;
        PlayerColor = color;
    }

    public void MakeStep(List<Cell> PlayerCells, List<Unit> PlayerUnits)
    {
        foreach(var cell in PlayerCells)
        {
            Money += (int)cell.Rec.Income;
        }

        foreach (var unit in PlayerUnits)
        {
            unit.MoveThroughtPath();
            if(PlayerCells.Count > 0)
                Money -= unit.MaintenanceCost;
            unit.MPToMax();
        }
    }
}
