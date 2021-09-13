using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public static MainScript Instance;
    public Settings Settings;
    public Tilemap GroundTilemap;
    public Tilemap CellModsTilemap;
    public Tilemap UnitsTilemap;

    public Canvas MainCanvas;
    public GameObject GameGui;
    public GameObject PlayerColor;
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
                TextPos = new Vector3(TextPos.x + Settings.PlNamePosOffset.x, TextPos.y + Settings.PlNamePosOffset.y, 0);
                GameObject Text = Instantiate(MainScript.Instance.Settings.UnitsCountPrefab, TextPos, new Quaternion(), MainCanvas.transform);
                Recources rec = new Recources(Recources.GetRandomCellType());

                Cell creatingCell = new Cell(null, pos, GroundTilemap.HasTile(pos), Text, rec);

                World.Add(pos,creatingCell);

                if(rec.Type == Recources.CellType.Forest && creatingCell.IsGround)
                {
                    CellModsTilemap.SetTile(pos, MainScript.Instance.Settings.ForestTile);
                }
                else if (rec.Type == Recources.CellType.Mountain && creatingCell.IsGround)
                {
                    CellModsTilemap.SetTile(pos, MainScript.Instance.Settings.MountainTile);
                }
            }
        }

        List<Vector3Int> poses = new List<Vector3Int>();
        foreach(var wp in World)
        {
            if (wp.Value.IsGround) poses.Add(wp.Key);
        }

        foreach(var pl in Players)
        {
            int p = Random.Range(0, poses.Count);
            World[poses[p]].AddUnits(new Unit(pl, Unit.UnitType.Citizen));
            poses.RemoveAt(p);
        }

    }

    void Update()
    {
        MoveUnit();
        UpdateGui();
    }


    
    private void UpdateGui()
    {
        GameGui.GetComponentInChildren<Text>().text = $"{Players[PlayerStep].PlayerName}\nSteps:{Steps}";
        GameObject.Find("Money").GetComponentInChildren<Text>().text = Players[PlayerStep].Money.ToString();
        PlayerColor.GetComponent<Image>().color = Players[PlayerStep].PlayerColor;
    }

    private void MoveUnit()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !ShopScript.Instance.IsBuying)
        {
            Vector3Int ClickedCellPos = GroundTilemap.WorldToCell(mousePos);
            ClickedCellPos.z = 0;

            Cell ClickedCell = World[ClickedCellPos];

            if (SelectedCell == null && ClickedCell.Units != null && ClickedCell.Units.Owner == Players[PlayerStep] && !ClickedCell.Units.IsMakeStep)
            {
                SelectedCell = ClickedCell;
            }

            else if(SelectedCell != null && ClickedCell == SelectedCell)
            {
                SelectedCell = null;
            }

            else if (SelectedCell != null)
            {
                foreach (var neighbor in SelectedCell.Neighbors())
                {
                    if (neighbor == ClickedCellPos && ClickedCell.Units != null && ClickedCell.Units.Owner != Players[PlayerStep] && SelectedCell.Units.Type != Unit.UnitType.Citizen || ClickedCell.IsGround && neighbor == ClickedCellPos)
                    {
                        ClickedCell.AddUnits(SelectedCell.GetUnits());
                        SelectedCell = null;
                        if(ClickedCell.Units != null)
                            ClickedCell.Units.IsMakeStep = true;
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
