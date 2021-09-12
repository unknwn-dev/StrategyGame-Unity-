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
    public GameObject SelectUnitsNum;
    public bool IsCamMove;

    public List<Player> Players;
    public Dictionary<Vector3Int, Cell> World = new Dictionary<Vector3Int, Cell>();

    private int PlayerStep = 0;
    private int Steps;
    private Cell SelectedCell;
    private float NumSendUnits;
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
                Recources rec = new Recources(Recources.GetRandomCellType(), 10, 0.01f);

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
            World[randPos].AddUnits(new Units(pl, 100));
        }

    }

    void Update()
    {
        MakeStep();
        UpdateGui();
    }


    
    private void UpdateGui()
    {
        GameGui.GetComponent<Text>().text = $"{Players[PlayerStep].PlayerName}\nSteps:{Steps}";
        GameObject.Find("Money").GetComponent<Text>().text = Players[PlayerStep].Money.ToString();
    }

    private void MakeStep()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int ClickedCell = GroundTilemap.WorldToCell(mousePos);
            ClickedCell.z = 0;

            if (SelectedCell == null && World[ClickedCell].Units != null && World[ClickedCell].Units.Number > 0 && World[ClickedCell].Units.Owner == Players[PlayerStep])
            {
                SelectedCell = World[ClickedCell];

                SelectUnitsNum.SetActive(true);
                Slider slide = SelectUnitsNum.GetComponentInChildren<Slider>();
                slide.value = World[ClickedCell].Units.Number / 2;
                slide.maxValue = World[ClickedCell].Units.Number;
                OnSliderValueSet();
            }

            else if(SelectedCell != null && World[ClickedCell] == SelectedCell)
            {
                SelectedCell = null;

                SelectUnitsNum.GetComponentInChildren<Slider>().value = 1;
                SelectUnitsNum.SetActive(false);
            }

            else if (SelectedCell != null)
            {
                foreach (var neighbor in SelectedCell.Neighbors())
                {
                    if (World[ClickedCell].IsGround && neighbor == ClickedCell)
                    {
                        World[ClickedCell].AddUnits(SelectedCell.GetUnits((int)NumSendUnits));
                        SelectedCell = null;

                        SelectUnitsNum.GetComponentInChildren<Slider>().value = 1;
                        SelectUnitsNum.SetActive(false);

                        Steps++;
                        PlayerStep++;

                        if (PlayerStep > Players.Count-1)
                        {
                            PlayerStep = 0;
                        }

                        NextStep();
                    }
                }
            }
        }
    }

    private void NextStep()
    {
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
                        if (cell.Units != null && cell.Units.Owner == pl)
                        {
                            PlayerCells.Add(cell);
                            TempWorld.Remove(pos);
                        }
                    }
                }
            }
            pl.MakeStep(PlayerCells);
        }
    }

    public void OnSliderValueSet()
    {
        Cell SelCell = null;
        if (SelectedCell != null)
        {
            SelCell = SelectedCell;
            NumSendUnits = SelectUnitsNum.GetComponentInChildren<Slider>().value;
            SelectUnitsNum.GetComponentInChildren<Text>().text = $"Units:{NumSendUnits}, Pop:{SelCell.Rec.Population}, Hap:{SelCell.Rec.Happiness}";
        }
    }
}
