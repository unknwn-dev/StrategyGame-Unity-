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
    [Range(0, 0.5f)]
    public float ForestChance;
    public Tile MountainTile;
    [Range(0, 0.3f)]
    public float MounatinChance;

    [Header("Prefabs")]
    public GameObject UnitsCountPrefab;

    [Header("Camera Parameters")]
    public float CameraSpeed;
    public float MaxZoom;
    public float MinZoom;
    public Vector2 MaxCamPos;
    public Vector2 MinCamPos;

    private void OnEnable()
    {
        Instance = this;
    }
}
