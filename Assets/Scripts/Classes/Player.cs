using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string PlayerName;
    public int PlayerID;
    [HideInInspector]
    public Tile PlayerTile = new Tile();

    public Player(string name, int id, Color color)
    {
        PlayerName = name;
        PlayerID = id;
        PlayerTile.sprite = Settings.Instance.BaseTileSprite;
        PlayerTile.color = color;
    }
}
