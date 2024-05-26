using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme_EventManager : MonoBehaviour
{
    public int[] themeIDs;
    public int[] themePositions;
    public static Theme_EventManager instance;

    // Update is called once per frame
    void Awake()
    {
        GetThemeMap(GameManager.LevelID);
        instance = this;
    }
    private void GetThemeMap(int levelID)
    {
        var themeFile = Resources.Load<TextAsset>($"Themes/Theme{levelID}").ToString();
        string[] themeStrings = themeFile.Split("\n");
        themeIDs = new int[themeStrings.Length];
        themePositions = new int[themeStrings.Length];
        for(int i = 0; i < themeStrings.Length; i++)
        {
            themeIDs[i] = int.Parse(themeStrings[i].Split(",")[0]);
            themePositions[i] = int.Parse(themeStrings[i].Split(",")[1]);
        }
    }
}
