using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "GameSettings"), SerializeField]
public class Settings : ScriptableObject
{
    public static Settings Instance;

    [Header("World Settings")]
    public int StartMoney;
    public Sprite BaseTileSprite;
    public Tile GroundTile;
    public Tile CastleTile;
    public Tile MvUnitFieldTile;
    public int FlatMPForMove;
    public int ForestMPForMove;
    public Tile ForestTile;
    [Range(0, 0.5f)]
    public float ForestChance;
    public Tile MountainTile;
    [Range(0, 0.3f)]
    public float MounatinChance;
    [SerializeField]
    public List<Color> PlayersColors;

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
    public int CitizenMP;

    [Header("WarriorParams")]
    public Sprite WarriorSpr;
    public int WarriorCost;
    public int WarriorHP;
    public int WarriorDmg;
    public int WarriorMaintenanceCost;
    public int WarriorMP;

    [Header("SettlersParams")]
    public Sprite SettlersSpr;
    public int SettlersCost;
    public int SettlersHP;
    public int SettlersMaintenanceCost;
    public int SettlersMP;

    [Header("BattleParams")]
    public int TerritoryHPBonus;

    [Header("CastleParams")]
    public int CastleHP;

    [Header("FarmParams")]
    public int FarmHP;

    [Header("Settings")]
    public string MapsFolder;

    private void OnEnable()
    {
        Instance = this;
        MapsFolder = Application.dataPath + "/Maps";
    }
}
