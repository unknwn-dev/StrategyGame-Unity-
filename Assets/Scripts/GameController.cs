using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using TMPro;

public class GameController : MonoBehaviour {
    public static GameController Instance;
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
    public GameObject GameOverPanel;
    public bool IsCanMove;

    public Cell SelectedCell;
    public List<Cell> SelectedCellNeighbors = new List<Cell>();

    private BoundsInt bounds;
    private bool IsInfoPanelOpened;


    void Start() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) {
            Destroy(gameObject);
        }

        if (!Settings.Game.IsLoaded) {
            GeneratePlayers();
            InitMap();
        }
        else {
            Settings.Game.LoadGame(Settings.Game.SaveName);
        }
    }

    void Update() {
        MoveUnit();
        GUIController.UpdateGui();
    }

    public void GeneratePlayers() {
        List<Color> plCols = new List<Color>(Settings.Instance.PlayersColors);
        for (int i = 0; i < MenuController.PlayersCount; i++) {
            int randCol = Random.Range(0, plCols.Count - 1);
            Settings.Game.Players.Add(new Player("p" + i, plCols[randCol]));
            plCols.Remove(plCols[randCol]);
        }
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

                Settings.Game.World.Add(pos, creatingCell);

                if (rec.Type == Recources.CellType.Forest && creatingCell.IsGround) {
                    CellModsTilemap.SetTile(pos, GameController.Instance.Settings.ForestTile);
                }
                else if (rec.Type == Recources.CellType.Mountain && creatingCell.IsGround) {
                    CellModsTilemap.SetTile(pos, GameController.Instance.Settings.MountainTile);
                }
            }
        }

        List<Vector3Int> poses = new List<Vector3Int>();
        foreach (var wp in Settings.Game.World) {
            if (wp.Value.IsGround) poses.Add(wp.Key);
        }

        foreach (var pl in Settings.Game.Players) {
            pl.Money = Settings.StartMoney;
            int p = Random.Range(0, poses.Count);
            Settings.Game.World[poses[p]].Units = new Unit(pl, Unit.UnitType.Settlers);
            Settings.Game.World[poses[p]].UpdateOwn();
            poses.RemoveAt(p);
        }
    }

    private void MoveUnit() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !ShopScript.Instance.IsBuying) {
            Vector3Int ClickedCellPos = GroundTilemap.WorldToCell(mousePos);
            ClickedCellPos.z = 0;

            Cell ClickedCell;

            if (Settings.Game.World.ContainsKey(ClickedCellPos))
                ClickedCell = Settings.Game.World[ClickedCellPos];
            else
                return;

            if (SelectedCell == null && ClickedCell.Units != null && ClickedCell.Units.Owner == Settings.Game.Players[Settings.Game.PlayerStep]) {
                SelectedCell = ClickedCell;


                ClearMoveFieldTilemap();

                if (SelectedCell.Units.MovementPath.Count > 0) {
                    foreach (var cell in SelectedCell.Units.MovementPath) {
                        if (cell != null)
                            MoveFieldTilemap.SetTile(cell, Settings.MvUnitFieldTile);
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

            else if (SelectedCell != null && SelectedCell.Units.MovementPath.Count > 0 && Settings.Game.World[SelectedCell.Units.MovementPath[SelectedCell.Units.MovementPath.Count - 1]] == ClickedCell) {
                SelectedCell.Units.MoveThroughtPath();

                SelectedCell = null;

                ClearMoveFieldTilemap();

                ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
            }

            else if (SelectedCell != null && ClickedCell.IsGround) {
                ClearMoveFieldTilemap();
                SelectedCell.Units.MovementPath = PathFinder.Find(SelectedCell, ClickedCell);


                foreach (var cell in SelectedCell.Units.MovementPath) {
                    if (cell != null)
                        MoveFieldTilemap.SetTile(cell, Settings.MvUnitFieldTile);
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

        Settings.Game.PlayerStep++;

        Animator GOPAnimator = GameOverPanel.GetComponent<Animator>();
        if (GOPAnimator.GetBool("Out") == true)
        {
            GOPAnimator.SetBool("Out", false);
        }

        if (Settings.Game.PlayerStep > Settings.Game.Players.Count - 1) {
            List<Player> players = new List<Player>(Settings.Game.Players);
            Settings.Game.Steps++;
            Settings.Game.PlayerStep = 0;


            foreach (var pl in players) {

                List<Cell> PlayerCells = new List<Cell>();
                List<Unit> PlayerUnits = new List<Unit>();
                foreach (var cell in Settings.Game.World) {
                    if (cell.Value.Units != null) {
                        if (cell.Value.Units.Owner.ID == pl.ID) {
                            PlayerUnits.Add(cell.Value.Units);
                        }
                    }

                    if (cell.Value.Owner != null && cell.Value.Owner.ID == pl.ID) {
                        PlayerCells.Add(cell.Value);
                    }
                }
                if(PlayerCells.Count == 0 && PlayerUnits.Count == 0)
                {
                    Settings.Game.Players.Remove(pl);
                    GameOverPanel.GetComponentInChildren<TMP_Text>().text = pl.PlayerName + " is lose";
                    GOPAnimator.SetBool("Out", true);
                }
                pl.MakeStep(PlayerCells, PlayerUnits);
            }
        }
    }
}

