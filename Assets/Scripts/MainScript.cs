using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScript : MonoBehaviour {
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
    public int Steps;

    private BoundsInt bounds;
    private bool IsInfoPanelOpened;


    void Start() {
        Instance = this;

        List<Color> plCols = new List<Color>(Settings.Instance.PlayersColors);

        for (int i = 0; i < MenuController.PlayersCount; i++) {
            int randCol = Random.Range(0, plCols.Count - 1);
            Players.Add(new Player("p" + i, plCols[randCol]));
            plCols.Remove(plCols[randCol]);
        }

        InitMap();
    }

    void Update() {
        MoveUnit();
        GUIController.UpdateGui();
    }

    public void OnInfoClick() {
        IsInfoPanelOpened = !IsInfoPanelOpened;
        GameGui.GetComponentInParent<Animator>().SetBool("IsOpen", IsInfoPanelOpened);
    }

    public void InitMap() {
        bounds = GroundTilemap.cellBounds;
        for (int x = bounds.xMin; x <= bounds.xMax; x++) {
            for (int y = bounds.yMin; y <= bounds.yMax; y++) {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Recources rec = new Recources(Recources.GetRandomCellType());

                Cell creatingCell = new Cell(null, pos, GroundTilemap.HasTile(pos), rec);

                World.Add(pos, creatingCell);

                if (rec.Type == Recources.CellType.Forest && creatingCell.IsGround) {
                    CellModsTilemap.SetTile(pos, MainScript.Instance.Settings.ForestTile);
                }
                else if (rec.Type == Recources.CellType.Mountain && creatingCell.IsGround) {
                    CellModsTilemap.SetTile(pos, MainScript.Instance.Settings.MountainTile);
                }
            }
        }

        List<Vector3Int> poses = new List<Vector3Int>();
        foreach (var wp in World) {
            if (wp.Value.IsGround) poses.Add(wp.Key);
        }

        foreach (var pl in Players) {
            pl.Money = Settings.StartMoney;
            int p = Random.Range(0, poses.Count);
            World[poses[p]].Units = new Unit(pl, Unit.UnitType.Settlers);
            World[poses[p]].UpdateOwn();
            poses.RemoveAt(p);
        }
    }

    private void MoveUnit() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !ShopScript.Instance.IsBuying && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            Vector3Int ClickedCellPos = GroundTilemap.WorldToCell(mousePos);
            ClickedCellPos.z = 0;

            Cell ClickedCell;

            if (World.ContainsKey(ClickedCellPos))
                ClickedCell = World[ClickedCellPos];
            else
                return;

            if (SelectedCell == null && ClickedCell.Units != null && ClickedCell.Units.Owner == Players[PlayerStep]) {
                SelectedCell = ClickedCell;


                ClearMoveFieldTilemap();

                if (SelectedCell.Units.MovementPath.Count > 0) {
                    foreach (var cell in SelectedCell.Units.MovementPath) {
                        if (cell != null)
                            MoveFieldTilemap.SetTile(cell.CellPos, Settings.MvUnitFieldTile);
                    }
                }
                else {
                    foreach (var cell in Cell.GetNeighborCellsInRange(SelectedCell, SelectedCell.Units.MovePoints)) {
                        if (cell != null)
                            MoveFieldTilemap.SetTile(cell.CellPos, Settings.MvUnitFieldTile);
                    }
                }

                ActionsPanel.GetComponent<UnitsActionScript>().InitMenu(ClickedCell, true);
            }

            else if (SelectedCell != null && ClickedCell == SelectedCell) {
                SelectedCell = null;

                ClearMoveFieldTilemap();

                ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
            }

            else if (SelectedCell != null && SelectedCell.Units.MovementPath.Count > 0 && SelectedCell.Units.MovementPath[SelectedCell.Units.MovementPath.Count - 1] == ClickedCell) {
                SelectedCell.Units.MoveThroughtPath();

                SelectedCell = null;

                ClearMoveFieldTilemap();

                ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
            }

            else if (SelectedCell != null && ClickedCell.IsGround) {
                ClearMoveFieldTilemap();

                if (ClickedCell.Units == null) {
                    SelectedCell.Units.MovementPath = PathFinder.Find(SelectedCell, ClickedCell);


                    foreach (var cell in SelectedCell.Units.MovementPath) {
                        if (cell != null)
                            MoveFieldTilemap.SetTile(cell.CellPos, Settings.MvUnitFieldTile);
                    }
                }
                else {
                    SelectedCell.AddUnits(ClickedCell);
                }
            }


            else {
                ActionsPanel.GetComponent<UnitsActionScript>().InitMenu(ClickedCell, false);
            }
        }
    }

    public void ClearMoveFieldTilemap() {
        MoveFieldTilemap.ClearAllTiles();

        SelectedCellNeighbors = new List<Cell>();
    }

    public void NextStep() {
        SelectedCell = null;
        ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
        ClearMoveFieldTilemap();

        PlayerStep++;

        if (PlayerStep > Players.Count - 1) {
            Steps++;
            PlayerStep = 0;

            Dictionary<Vector3Int, Cell> TempWorld = new Dictionary<Vector3Int, Cell>(World);
            foreach (var pl in Players) {

                List<Cell> PlayerCells = new List<Cell>();
                List<Unit> PlayerUnits = new List<Unit>();
                for (int x = bounds.xMin; x <= bounds.xMax; x++) {
                    for (int y = bounds.yMin; y <= bounds.yMax; y++) {
                        Vector3Int pos = new Vector3Int(x, y, 0);

                        if (TempWorld.ContainsKey(pos)) {
                            Cell cell = TempWorld[pos];

                            if (cell.Units != null) {
                                if (cell.Units.Owner == pl) {
                                    PlayerUnits.Add(cell.Units);
                                }
                            }

                            if (cell.Owner == pl) {
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

}
