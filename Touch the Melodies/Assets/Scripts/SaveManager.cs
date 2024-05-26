using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public ActualSave saveData;
    private void Awake()
    {
        instance = this;
        saveData = new ActualSave();
        if (!PlayerPrefs.HasKey("LanguageID"))
        {
            PlayerPrefs.SetInt("LanguageID", 0);
        }
        if (!PlayerPrefs.HasKey("VisualAngleID"))
        {
            PlayerPrefs.SetInt("VisualAngleID", 0);
        }
        if (!PlayerPrefs.HasKey("CoinsCount"))
        {
            PlayerPrefs.SetInt("CoinsCount", 0);
        }
        if (!PlayerPrefs.HasKey("EnergyCount"))
        {
            PlayerPrefs.SetInt("EnergyCount", 0);
        }
        if (!PlayerPrefs.HasKey("level0_progress"))
        {
            PlayerPrefs.SetInt("level0_progress", 0);
        }
        if (!PlayerPrefs.HasKey("level0_gems"))
        {
            PlayerPrefs.SetInt("level0_gems", 0);
        }
        if (!PlayerPrefs.HasKey("level0_crowns"))
        {
            PlayerPrefs.SetInt("level0_crowns", 0);
        }
        Load();
    }
    public void Save()
    {
        PlayerPrefs.SetInt("LanguageID", saveData._languageID);
        PlayerPrefs.SetInt("VisualAngleID", saveData._visualAngleID);
        PlayerPrefs.SetInt("CoinsCount", saveData._coinsCount);
        PlayerPrefs.SetInt("EnergyCount", saveData._energyCount);
        PlayerPrefs.SetInt("level0_progress", saveData._level0_progress);
        PlayerPrefs.SetInt("level0_gems", saveData._level0_gems);
        PlayerPrefs.SetInt("level0_crowns", saveData._level0_crowns);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        saveData._languageID = PlayerPrefs.GetInt("LanguageID");
        saveData._visualAngleID = PlayerPrefs.GetInt("VisualAngleID");
        saveData._coinsCount = PlayerPrefs.GetInt("CoinsCount");
        saveData._energyCount = PlayerPrefs.GetInt("EnergyCount");
        saveData._level0_progress = PlayerPrefs.GetInt("level0_progress");
        saveData._level0_gems = PlayerPrefs.GetInt("level0_gems");
        saveData._level0_crowns = PlayerPrefs.GetInt("level0_crowns");
    }
    public class ActualSave
    {
        public int _languageID = 0;
        public int _visualAngleID = 0;
        public int _coinsCount = 0;
        public int _energyCount = 0;
        public int _level0_progress = 0;
        public int _level0_gems = 0;
        public int _level0_crowns = 0;
    }
}
