using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveSelectorController : MonoBehaviour
{
    private void OnEnable() {

        gameObject.GetComponent<TMP_Dropdown>().options = new List<TMP_Dropdown.OptionData>();

        if (!Directory.Exists(MenuController.Instance.Settings.SaveFolder)) {
            Directory.CreateDirectory(MenuController.Instance.Settings.SaveFolder);
        }

        MenuController.Instance.Settings.Saves = Directory.GetFiles(MenuController.Instance.Settings.SaveFolder + "/", "*.gmsv");

        List<TMP_Dropdown.OptionData> dropDData = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < MenuController.Instance.Settings.Saves.Length; i++) {
            dropDData.Add(new TMP_Dropdown.OptionData(MenuController.Instance.Settings.Saves[i].Replace(MenuController.Instance.Settings.SaveFolder + "/", "").Replace(".gmsv", "")));
        }

        if (dropDData != null)
            gameObject.GetComponent<TMP_Dropdown>().AddOptions(dropDData);
    }
}
