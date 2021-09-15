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
    public Tile CastleTile;
    [Range(0, 0.5f)]
    public float ForestChance;
    public Tile MountainTile;
    [Range(0, 0.3f)]
    public float MounatinChance;

    [Header("Prefabs")]
    public GameObject UnitsCountPrefab;
    public GameObject UnitsActionButtonPrefab;

    [Header("PlayerNameSettings")]
    public Vector2 PlNamePosOffset;

    [Header("Camera Parameters")]
    public float CameraSpeed;
    public float MaxZoom;
    public float MinZoom;
    public Vector2 MaxCamPos;
    public Vector2 MinCamPos;


    [Header("CitizenParams")]
    public Sprite CitizSpr;
    public int CitizenCost;
    public int CitizenHP;
    public int CitizenDmg;
    public int CitizenMaintenanceCost;

    [Header("WarriorParams")]
    public Sprite WarriorSpr;
    public int WarriorCost;
    public int WarriorHP;
    public int WarriorDmg;
    public int WarriorMaintenanceCost;

    [Header("SettlersParams")]
    public Sprite SettlersSpr;
    public int SettlersCost;
    public int SettlersHP;
    public int SettlersMaintenanceCost;

    [Header("BattleParams")]
    public int TerritoryHPBonus;

    [Header("CastleParams")]
    public int CastleHP;

    [Header("FarmParams")]
    public int FarmHP;

    private void OnEnable()
    {
        Instance = this;
    }
}
