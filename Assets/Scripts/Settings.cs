using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "GameSettings"), SerializeField]
public class Settings : ScriptableObject
{
    public static Settings Instance;

    public static Game Game = new Game();

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


    [Header("Units")]
    public List<Unit> UnitTypes;

    [Header("BattleParams")]
    public int TerritoryHPBonus;

    [Header("CastleParams")]
    public int CastleHP;

    [Header("FarmParams")]
    public int FarmHP;

    [Header("Settings")]
    public string MapsFolder;
    public string SaveFolder;
    [HideInInspector]
    public string[] Saves;
    [HideInInspector]
    public string[] Maps;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) {
            Destroy(this);
        }
        MapsFolder = Application.dataPath + "/Maps";
        SaveFolder = Application.dataPath + "/Saves";
    }
}
