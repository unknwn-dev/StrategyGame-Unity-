using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public static MainScript Instance;

    public Tilemap GroundTilemap;
    public Tilemap CellModsTilemap;

    public Canvas MainCanvas;
    public GameObject GameGui;
    public bool IsCamMove;

    public List<Player> Players;
    public Dictionary<Vector3Int, Cell> World = new Dictionary<Vector3Int, Cell>();
    public int PlayerStep = 0;

    private int Steps;
    private Cell SelectedCell;
    private BoundsInt bounds;

    void Start()
    {
        Instance = this;

        Players.Add(new Player("a", 0, Color.red));
        Players.Add(new Player("b", 0, Color.gray));


        bounds = GroundTilemap.cellBounds;

        for (int x = bounds.xMin; x <= bounds.xMax; x++) {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Vector3 TextPos = GroundTilemap.CellToWorld(pos);
                GameObject Text = Instantiate(Settings.Instance.UnitsCountPrefab, TextPos, new Quaternion(), MainCanvas.transform);
                Recources rec = new Recources(Recources.GetRandomCellType());

                Cell creatingCell = new Cell(null, pos, GroundTilemap.HasTile(pos), Text, rec);

                World.Add(pos,creatingCell);

                if(rec.Type == Recources.CellType.Forest && creatingCell.IsGround)
                {
                    CellModsTilemap.SetTile(pos, Settings.Instance.ForestTile);
                }
                else if (rec.Type == Recources.CellType.Mountain && creatingCell.IsGround)
                {
                    CellModsTilemap.SetTile(pos, Settings.Instance.MountainTile);
                }

                creatingCell.UpdateUnitsCount();
            }
        }

        foreach(var pl in Players)
        {
            Vector3Int randPos = new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0);
            while (!World[randPos].IsGround)
                randPos = new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0);
            World[randPos].AddUnits(new Unit(pl, Unit.UnitType.Citizen));
        }

    }

    void Update()
    {
        MoveUnit();
        UpdateGui();
    }


    
    private void UpdateGui()
    {
        GameGui.GetComponent<Text>().text = $"{Players[PlayerStep].PlayerName}\nSteps:{Steps}";
        GameObject.Find("Money").GetComponent<Text>().text = Players[PlayerStep].Money.ToString();
    }

    private void MoveUnit()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !ShopScript.Instance.IsBuying)
        {
            Vector3Int ClickedCell = GroundTilemap.WorldToCell(mousePos);
            ClickedCell.z = 0;

            if (SelectedCell == null && World[ClickedCell].Units != null && World[ClickedCell].Units.Owner == Players[PlayerStep] && !World[ClickedCell].Units.IsMakeStep)
            {
                SelectedCell = World[ClickedCell];
            }

            else if(SelectedCell != null && World[ClickedCell] == SelectedCell)
            {
                SelectedCell = null;
            }

            else if (SelectedCell != null)
            {
                foreach (var neighbor in SelectedCell.Neighbors())
                {
                    if (World[ClickedCell].IsGround && neighbor == ClickedCell)
                    {
                        World[ClickedCell].AddUnits(SelectedCell.GetUnits());
                        SelectedCell = null;
                        World[ClickedCell].Units.IsMakeStep = true;
                    }
                }
            }
        }
    }

    public void NextStep()
    {
        Steps++;
        PlayerStep++;

        if (PlayerStep > Players.Count - 1)
        {
            PlayerStep = 0;
        }

        Dictionary<Vector3Int, Cell> TempWorld = new Dictionary<Vector3Int, Cell>(World);
        foreach(var pl in Players)
        {
            List<Cell> PlayerCells = new List<Cell>();
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (TempWorld.ContainsKey(pos))
                    {
                        Cell cell = TempWorld[pos];
                        if (cell.Owner == pl)
                        {
                            PlayerCells.Add(cell);
                            TempWorld.Remove(pos);
                            if (cell.Units != null)
                            {
                                cell.Units.IsMakeStep = false;
                            }
                        }
                    }
                }
            }
            pl.MakeStep(PlayerCells);
        }
    }
}
