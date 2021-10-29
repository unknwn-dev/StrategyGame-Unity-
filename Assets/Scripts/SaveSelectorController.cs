using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveSelectorController : MonoBehaviour
{
    private void OnEnable() {

        gameObject.GetComponent<TMP_Dropdown>().options = new List<TMP_Dropdown.OptionData>();

        if (!Directory.Exists(Settings.Instance.SaveFolder)) {
            Directory.CreateDirectory(Settings.Instance.SaveFolder);
        }

        Settings.Instance.Saves = Directory.GetFiles(Settings.Instance.SaveFolder, "*.gmsv");

        List<TMP_Dropdown.OptionData> dropDData = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < Settings.Instance.Saves.Length; i++) {
            dropDData.Add(new TMP_Dropdown.OptionData(Settings.Instance.Saves[i].Replace(Application.dataPath + "/Saves\\", "").Replace(".gmsv", "")));
        }

        if (dropDData != null)
            gameObject.GetComponent<TMP_Dropdown>().AddOptions(dropDData);
    }
}
