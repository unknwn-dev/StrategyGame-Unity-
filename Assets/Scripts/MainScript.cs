using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public static MainScript Instance;

    public Tilemap Tilemap;
    public Canvas MainCanvas;
    public GameObject GameGui;
    public GameObject SelectUnitsNum;

    public List<Player> Players;
    public Dictionary<Vector3Int, Cell> World = new Dictionary<Vector3Int, Cell>();

    private int PlayerStep;
    private int Steps;
    private Cell SelectedCell;
    private float NumSendUnits;

    void Start()
    {
        Instance = this;

        Players.Add(new Player("a", 0, Color.red));
        Players.Add(new Player("b", 0, Color.gray));


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
    }

    private void MakeStep()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int ClickedCell = Tilemap.WorldToCell(mousePos);
            ClickedCell.z = 0;

            if (World.ContainsKey(ClickedCell))
            {
                if (SelectedCell == null && World[ClickedCell].Units != null && World[ClickedCell].Units.Number > 0 && World[ClickedCell].Units.Owner == Players[PlayerStep])
                {
                    Debug.Log("cellSelected");
                    SelectedCell = World[ClickedCell];
                    SelectUnitsNum.SetActive(true);
                    Slider slide = SelectUnitsNum.GetComponentInChildren<Slider>();
                    slide.value = World[ClickedCell].Units.Number/2;
                    slide.maxValue = World[ClickedCell].Units.Number;
                    OnSliderValueSet();
                }
                else if (SelectedCell != null)
                {
                    foreach (var neighbor in SelectedCell.Neighbors())
                    {
                        if (World[ClickedCell].IsGround && neighbor == ClickedCell)
                        {
                            Debug.Log("SecondCellSelected");
                            World[ClickedCell].AddUnits(SelectedCell.GetUnits((int)NumSendUnits));
                            SelectedCell = null;

                            SelectUnitsNum.GetComponentInChildren<Slider>().value = 1;
                            SelectUnitsNum.SetActive(false);

                            Steps++;
                            PlayerStep++;

                            if (PlayerStep > Players.Count - 1)
                            {
                                PlayerStep = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnSliderValueSet()
    {
        NumSendUnits = SelectUnitsNum.GetComponentInChildren<Slider>().value;
        SelectUnitsNum.GetComponentInChildren<Text>().text = NumSendUnits.ToString();
    }
}
