using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEditorController : MonoBehaviour
{
    public static MapEditorController Instance;

    public Tilemap MainTilemap;
    public Tilemap CellModsTilemap;
    public bool IsGenerateCellMods;

    [SerializeField]
    private GameObject CellButtonPref;
    private Cell SelectedCell;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int ClickedCellPos = MainTilemap.WorldToCell(mousePos);
            ClickedCellPos.z = 0;

            Settings.Game.World[ClickedCellPos] = SelectedCell;
        }
    }

}
