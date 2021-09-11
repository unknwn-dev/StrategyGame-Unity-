using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScript : MonoBehaviour
{
    public static MainScript Instance;

    public Tilemap Tilemap;
    public Canvas MainCanvas;
    private int Step;

    public List<Player> Players;
    public Dictionary<Vector3Int, Cell> World = new Dictionary<Vector3Int, Cell>();

    private Cell SelectedCell;

    void Start()
    {
        Instance = this;

        Players.Add(new Player("test", 0, Color.red));


        BoundsInt bounds = Tilemap.cellBounds;

        for (int x = bounds.xMin; x <= bounds.xMax; x++) {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Vector3 TextPos = Tilemap.CellToWorld(pos);
                GameObject Text = Instantiate(Settings.Instance.UnitsCountPrefab, TextPos, new Quaternion(), MainCanvas.transform);
                Cell creatingCell = new Cell(null, pos, Tilemap.HasTile(pos), Text);
                World.Add(pos,creatingCell);
                creatingCell.UpdateUnitsCount();
            }
        }

        World[new Vector3Int(0,0,0)].AddUnits(new Units(Players[0], 100));

    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int ClickedCell = Tilemap.WorldToCell(mousePos);
            ClickedCell.z = 0;

            if (SelectedCell == null && World[ClickedCell].Units != null && World[ClickedCell].Units.Number > 0)
            {
                SelectedCell = World[ClickedCell];
            }
            else if(SelectedCell != null)
            {
                foreach(var neighbor in SelectedCell.Neighbors())
                {
                    if (World[ClickedCell].IsGround && neighbor == ClickedCell)
                    {
                        World[ClickedCell].AddUnits(SelectedCell.GetUnits(10));
                        SelectedCell = null;
                        Step++;
                    }
                }
            }
        }
    }
}
