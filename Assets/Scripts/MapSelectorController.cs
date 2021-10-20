using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class MapSelectorController : MonoBehaviour
{

    private void OnEnable() {

        gameObject.GetComponent<TMP_Dropdown>().options = new List<TMP_Dropdown.OptionData>();

        if (!Directory.Exists(Settings.Instance.MapsFolder)) {
            Directory.CreateDirectory(Settings.Instance.MapsFolder);
        }

        MenuController.Instance.Maps = Directory.GetFiles(Settings.Instance.MapsFolder, "*.gmps");

        List<TMP_Dropdown.OptionData> dropDData = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < MenuController.Instance.Maps.Length; i++) {
            dropDData.Add(new TMP_Dropdown.OptionData(MenuController.Instance.Maps[i].Replace(Application.dataPath + "/Maps\\", "")));
        }

        if(dropDData != null)
            gameObject.GetComponent<TMP_Dropdown>().AddOptions(dropDData);
    }
}
