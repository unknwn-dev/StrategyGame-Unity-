using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string PlayerName;
    public int PlayerID;
    public Color PlayerColor;
    [HideInInspector]
    public Tile PlayerTile = new Tile();
    public int Money;

    public Player(string name, int id, Color color)
    {
        PlayerName = name;
        PlayerID = id;
        PlayerTile.sprite = Settings.Instance.BaseTileSprite;
        PlayerTile.color = color;
        PlayerColor = color;
    }

    public void MakeStep(List<Cell> PlayerCells)
    {
        foreach(var cell in PlayerCells)
        {
            Money += (int)cell.Rec.Income;
            cell.Rec.MakeStep();
        }
    }
}
