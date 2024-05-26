using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton_Initialization : MonoBehaviour
{
    public TMPro.TMP_Text _levelName;
    public TMPro.TMP_Text _levelProgress;
    public TMPro.TMP_Text _gemsCount;
    public GameObject _difficultyStars;
    public GameObject _crowns;
    public TMPro.TMP_Text _levelOrderNum;
    public Image _levelCover;
    public int levelID;
    public int languageID;

    private void Start()
    {
        languageID = SaveManager.instance.saveData._languageID;
    }
    public void OnLevelPlayButton()
    {
        MainMenuController.instance.levelPlayableCanvas.SetActive(true);
        MainMenuController.instance.levelCanvas.SetActive(false);
        CrossingToLevel.playingLevelID = levelID;
        CrossingToLevel.levelType = "original";
        CrossingToLevel.playSpeed = LevelInfo_Data._originalLevelsInformation[levelID]._levelSpeed;
        for(int c = 0; c < SaveManager.instance.saveData._level0_crowns; c++)
        {
            _crowns.transform.GetChild(c).GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);
            _crowns.transform.GetChild(c).GetChild(0).gameObject.SetActive(true);
        }
        MainMenuController.instance.HideNavigation();
    }
    private void Update()
    {
        if(SaveManager.instance.saveData._languageID != languageID)
        {
            languageID = SaveManager.instance.saveData._languageID;
            _levelName.text = Localization.originalLevelsTranslation[levelID][languageID].ToString();
        }
    }
}
