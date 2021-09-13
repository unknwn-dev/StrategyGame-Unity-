using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GameSettings"), SerializeField]
public class Settings : ScriptableObject
{
    public static Settings Instance;

    [Header("World Settings")]
    public Sprite BaseTileSprite;
    public Tile GroundTile;
    public Tile ForestTile;
    public Tile CivilTile;
    public Tile WarriorTile;
    [Range(0, 0.5f)]
    public float ForestChance;
    public Tile MountainTile;
    [Range(0, 0.3f)]
    public float MounatinChance;

    [Header("Prefabs")]
    public GameObject UnitsCountPrefab;

    [Header("PlayerNameSettings")]
    public Vector2 PlNamePosOffset;

    [Header("Camera Parameters")]
    public float CameraSpeed;
    public float MaxZoom;
    public float MinZoom;
    public Vector2 MaxCamPos;
    public Vector2 MinCamPos;

    [Header("ShopSettings")]
    public int CitizenCost;
    public int WarriorCost;

    private void OnEnable()
    {
        Instance = this;
    }
}
