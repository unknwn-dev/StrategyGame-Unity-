using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {
    public static ShopScript Instance;

    public bool ShopIsOpened;
    public bool IsBuying;

    [SerializeField]
    private GameObject ShopPanel;
    [SerializeField]
    private GameObject ShopSrcollViewContent;
    [SerializeField]
    private GameObject ButtonPrefab;
    [SerializeField]
    private GameObject BlockingPanel;

    private Unit.UnitType SelectedType = Unit.UnitType.Null;

    public void OpenShop() {
        ShopPanel.SetActive(!ShopPanel.activeInHierarchy);
        ShopIsOpened = ShopPanel.activeInHierarchy;
        IsBuying = ShopPanel.activeInHierarchy;
    }

    public void BuyUnit(int type) {
        SelectedType = (Unit.UnitType)type;
        OpenShop();
        IsBuying = true;
    }

    private void Start() {
        Instance = this;


        foreach (var unitType in Settings.Instance.UnitTypes) {
            GameObject button = Instantiate(ButtonPrefab, ShopSrcollViewContent.transform);
            button.GetComponentInChildren<TMP_Text>().text = $"{unitType.Type}:{unitType.BuyCost}";
            button.GetComponent<Button>().onClick.AddListener(() => { BuyUnit((int)unitType.Type); });
        }
    }


    private void Update() {
        if (!ShopIsOpened && SelectedType != Unit.UnitType.Null) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0)) {
                Vector3Int ClickedCell = MainScript.Instance.GroundTilemap.WorldToCell(mousePos);
                ClickedCell.z = 0;

                Dictionary<Vector3Int, Cell> World = MainScript.Instance.World;

                Player MovingPlayer = MainScript.Instance.Players[MainScript.Instance.PlayerStep];

                if (World[ClickedCell].Units == null && World[ClickedCell].Owner == MovingPlayer) {
                    MainScript.Instance.Players[MainScript.Instance.PlayerStep].Money -= MainScript.Instance.Settings.UnitTypes[(int)SelectedType].BuyCost;
                    World[ClickedCell].Units = new Unit(MovingPlayer, SelectedType);
                    World[ClickedCell].UpdateOwn();
                    SelectedType = Unit.UnitType.Null;
                    IsBuying = false;
                }
            }
        }
    }
}
