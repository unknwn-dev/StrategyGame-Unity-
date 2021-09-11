using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GameSettings"), SerializeField]
public class Settings : ScriptableObject
{
    public static Settings Instance;
    public Sprite BaseTileSprite;
    public Tile GroundTile;
    public GameObject UnitsCountPrefab;

    private void OnEnable()
    {
        Instance = this;
    }
}
