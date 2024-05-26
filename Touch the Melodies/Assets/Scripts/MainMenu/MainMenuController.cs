using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField]
    private GameObject menuCanvas;
    public GameObject levelCanvas;
    [SerializeField]
    private GameObject shopCanvas;
    [SerializeField]
    private GameObject specialCanvas;
    [SerializeField]
    private GameObject settingsCanvas;
    [SerializeField]
    private GameObject achievementsCanvas;
    [SerializeField]
    private GameObject newsPaperCanvas;
    [SerializeField]
    private GameObject dailyRewardCanvas;
    [SerializeField]
    private GameObject playerAccountCanvas;
    [SerializeField]
    private GameObject navigationButtons;
    public GameObject levelPlayableCanvas;

    [Header("Buttons")]
    [SerializeField]
    private GameObject playButton, quitButton;
    [SerializeField]
    private GameObject xQuitFromDailyReward, xQuitFromGameNews;

    [Header("LevelMenu")]
    [SerializeField]
    private LevelButton_Initialization _levelButton;
    [SerializeField]
    private Transform _levelMenuPanel;

    public static MainMenuController instance;

    private void Start()
    {
        instance = this;
        for(int orderNum = 0; orderNum < LevelInfo_Data._levelOrdering.Count; orderNum++)
        {
            for(int levelID = 0; levelID < LevelInfo_Data._levelOrdering.Count; levelID++)
            {
                if(levelID == orderNum)
                {
                    LevelButton_Initialization newLevelButton = Instantiate(_levelButton, _levelMenuPanel);
                    newLevelButton._levelName.text = Localization.originalLevelsTranslation[levelID][SaveManager.instance.saveData._languageID];
                    newLevelButton._levelProgress.text = $"{SaveManager.instance.saveData._level0_progress}%";
                    newLevelButton._gemsCount.text = $"{SaveManager.instance.saveData._level0_gems}/10";
                    newLevelButton._levelOrderNum.text = "1/1";
                    newLevelButton._levelCover.sprite = Resources.Load<Sprite>($"Worldshows/Worldshow{levelID}");
                    newLevelButton.levelID = levelID;
                    break;
                }
            }
        }
    }
    public void OnPlayButton()
    {
        menuCanvas.SetActive(false);
        levelCanvas.SetActive(true);

        playButton.SetActive(false);
        quitButton.SetActive(true);
    }
    public void OnQuitButton()
    {
        menuCanvas.SetActive(true);
        levelCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        specialCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        achievementsCanvas.SetActive(false);
        levelPlayableCanvas.SetActive(false);
        navigationButtons.SetActive(true);

        playButton.SetActive(true);
        quitButton.SetActive(false);
    }

    public void OnShopButton()
    {
        menuCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        levelCanvas.SetActive(false);
        specialCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        achievementsCanvas.SetActive(false);

        playButton.SetActive(false);
        quitButton.SetActive(true);
    }

    public void OnSpecialbutton()
    {
        menuCanvas.SetActive(false);
        specialCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        levelCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        achievementsCanvas.SetActive(false);

        playButton.SetActive(false);
        quitButton.SetActive(true);
    }

    public void OnSettingsButton()
    {
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        specialCanvas.SetActive(false);
        levelCanvas.SetActive(false);
        achievementsCanvas.SetActive(false);

        playButton.SetActive(false);
        quitButton.SetActive(true);
    }

    public void OnAchievementsButton()
    {
        menuCanvas.SetActive(false);
        achievementsCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        specialCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        levelCanvas.SetActive(false);

        playButton.SetActive(false);
        quitButton.SetActive(true);
    }

    public void OnDailyRewardButton()
    {
        dailyRewardCanvas.SetActive(true);
        menuCanvas.GetComponent<CanvasGroup>().interactable = false;
        navigationButtons.GetComponent<CanvasGroup>().interactable = false;
    }
    public void OnNewspaperButton()
    {
        newsPaperCanvas.SetActive(true);
        menuCanvas.GetComponent<CanvasGroup>().interactable = false;
        navigationButtons.GetComponent<CanvasGroup>().interactable = false;
    }
    public void OnXQuitButton()
    {
        dailyRewardCanvas.SetActive(false);
        newsPaperCanvas.SetActive(false);
        menuCanvas.GetComponent<CanvasGroup>().interactable = true;
        navigationButtons.GetComponent<CanvasGroup>().interactable = true;
    }

    public void OnPlayableLevel()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void HideNavigation()
    {
        navigationButtons.SetActive(false);
        CrossingToLevel.visualAngleID = SaveManager.instance.saveData._visualAngleID;
    }
}
