using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class MainScript : MonoBehaviour
{
    public static MainScript Instance;
    public Settings Settings;
    public Tilemap GroundTilemap;
    public Tilemap CellModsTilemap;
    public Tilemap UnitsTilemap;
    public Tilemap MoveFieldTilemap;

    public Canvas MainCanvas;
    public GameObject GameGui;
    public GameObject PlayerColor;
    public GameObject Money;
    public GameObject ActionsPanel;
    public bool IsCanMove;

    public List<Player> Players;
    [SerializeField]
    public Dictionary<Vector3Int, Cell> World = new Dictionary<Vector3Int, Cell>();
    public int PlayerStep = -1;
    public Cell SelectedCell;
    public List<Cell> SelectedCellNeighbors = new List<Cell>();

    private int Steps;
    private BoundsInt bounds;
    private bool IsInfoPanelOpened;

    void Start()
    {
        Instance = this;

        Players.Add(new Player("Player1", Color.red));
        Players.Add(new Player("Player2", Color.gray));
        //Players.Add(new Player("Player3", Color.blue));
        //Players.Add(new Player("Player4", Color.magenta));

        bounds = GroundTilemap.cellBounds;

        for (int x = bounds.xMin; x <= bounds.xMax; x++) {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Recources rec = new Recources(Recources.GetRandomCellType());

                Cell creatingCell = new Cell(null, pos, GroundTilemap.HasTile(pos), rec);

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
            pl.Money = Settings.StartMoney;
            int p = Random.Range(0, poses.Count);
            World[poses[p]].Units = new Unit(pl, Unit.UnitType.Settlers);
            World[poses[p]].UpdateOwn();
            poses.RemoveAt(p);
        }

    }

    void Update()
    {
        MoveUnit();
        UpdateGui();
    }

    public void OnInfoClick()
    {
        IsInfoPanelOpened = !IsInfoPanelOpened;
        GameGui.GetComponentInParent<Animator>().SetBool("IsOpen", IsInfoPanelOpened);
    }

    private void UpdateGui()
    {
        GameGui.GetComponentInChildren<Text>().text = $"{Players[PlayerStep].PlayerName}\nSteps:{Steps}";
        Money.GetComponentInChildren<Text>().text = Players[PlayerStep].Money.ToString();
        PlayerColor.GetComponent<Image>().color = Players[PlayerStep].PlayerColor;
    }

    private void MoveUnit()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !ShopScript.Instance.IsBuying && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3Int ClickedCellPos = GroundTilemap.WorldToCell(mousePos);
            ClickedCellPos.z = 0;

            Cell ClickedCell;

            if (World.ContainsKey(ClickedCellPos))
                ClickedCell = World[ClickedCellPos];
            else
                return;

            if (SelectedCell == null && ClickedCell.Units != null && ClickedCell.Units.Owner == Players[PlayerStep])
            {
                SelectedCell = ClickedCell;


                ClearMoveFieldTilemap();

                foreach (var cell in Cell.GetNeighborCellsInRange(SelectedCell,SelectedCell.Units.MovePoints))
                {
                    if (cell != null)
                        MoveFieldTilemap.SetTile(cell.CellPos, Settings.MvUnitFieldTile);
                }

                ActionsPanel.GetComponent<UnitsActionScript>().InitMenu(ClickedCell, true);
            }

            else if (SelectedCell != null && ClickedCell == SelectedCell)
            {
                SelectedCell = null;

                ClearMoveFieldTilemap();

                ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
            }

            else if (SelectedCell != null && ClickedCell.IsGround)
            {

                ClearMoveFieldTilemap();

                SelectedCell.Units.MovementPath = FindPath(SelectedCell, ClickedCell);

                foreach (var cell in SelectedCell.Units.MovementPath)
                {
                    if (cell != null)
                        MoveFieldTilemap.SetTile(cell.CellPos, Settings.MvUnitFieldTile);
                }

                SelectedCell.Units.MoveThroughtPath();

                ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();

                SelectedCell = null;


            }

            else
            {
                ActionsPanel.GetComponent<UnitsActionScript>().InitMenu(ClickedCell, false);
            }
        }
    }

    public void ClearMoveFieldTilemap()
    {
        MoveFieldTilemap.ClearAllTiles();

        SelectedCellNeighbors = new List<Cell>();
    }

    public void NextStep()
    {
        SelectedCell = null;
        ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
        ClearMoveFieldTilemap();

        PlayerStep++;

        if (PlayerStep > Players.Count - 1)
        {
            Steps++;
            PlayerStep = 0;

            Dictionary<Vector3Int, Cell> TempWorld = new Dictionary<Vector3Int, Cell>(World);
            foreach (var pl in Players)
            {
                List<Cell> PlayerCells = new List<Cell>();
                List<Unit> PlayerUnits = new List<Unit>();
                for (int x = bounds.xMin; x <= bounds.xMax; x++)
                {
                    for (int y = bounds.yMin; y <= bounds.yMax; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);

                        if (TempWorld.ContainsKey(pos))
                        {
                            Cell cell = TempWorld[pos];

                            if (cell.Units != null)
                            {
                                if(cell.Units.Owner == pl)
                                {
                                    PlayerUnits.Add(cell.Units);
                                }
                            }

                            if (cell.Owner == pl)
                            {
                                PlayerCells.Add(cell);
                                TempWorld.Remove(pos);
                            }
                        }
                    }
                }
                pl.MakeStep(PlayerCells, PlayerUnits);
            }
        }
    }
    protected static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition)
    {
        return Mathf.Max(Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x), Mathf.Abs(startPosition.y - targetPosition.y), Mathf.Abs(startPosition.z - targetPosition.z)));
    }

    public List<Cell> FindPath(Cell from, Cell to)
    {
        List<Cell> opened = new List<Cell>();
        List<Cell> closed = new List<Cell>();

        Cell currentCell = from;

        currentCell.g = 0;
        currentCell.h = GetEstimatedPathCost(from.CellPos, to.CellPos);

        opened.Add(currentCell);

        while (opened.Count != 0)
        {
            // Sorting the open list to get the tile with the lowest F.
            opened = opened.OrderBy(x => x.F).ThenByDescending(x => x.g).ToList();
            currentCell = opened[0];

            // Removing the current tile from the open list and adding it to the closed list.
            opened.Remove(currentCell);
            closed.Add(currentCell);

            int g = currentCell.g + 1;

            // If there is a target tile in the closed list, we have found a path.
            if (closed.Contains(to))
            {
                break;
            }

            // Investigating each adjacent tile of the current tile.
            foreach (Cell adjacentTile in currentCell.GetNeighborCells())
            {

                // Ignore not walkable adjacent tiles.
                if (!adjacentTile.IsGround)
                {
                    continue;
                }

                // Ignore the tile if it's already in the closed list.
                if (closed.Contains(adjacentTile))
                {
                    continue;
                }

                // If it's not in the open list - add it and compute G and H.
                if (!(opened.Contains(adjacentTile)))
                {
                    adjacentTile.g = g;
                    adjacentTile.h = GetEstimatedPathCost(adjacentTile.CellPos, to.CellPos);
                    opened.Add(adjacentTile);
                }
                // Otherwise check if using current G we can get a lower value of F, if so update it's value.
                else if (adjacentTile.F > g + adjacentTile.h)
                {
                    adjacentTile.g = g;
                }
            }
        }

        List<Cell> finalPathTiles = new List<Cell>();
        // Backtracking - setting the final path.
        if (closed.Contains(to))
        {
            currentCell = to;
            finalPathTiles.Add(currentCell);

            for (int i = to.g - 1; i >= 0; i--)
            {
                currentCell = closed.Find(x => x.g == i && currentCell.GetNeighborCells().Contains(x));
                finalPathTiles.Add(currentCell);
            }

            finalPathTiles.Reverse();
        }

        return finalPathTiles;
    }
}
