using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private LevelGeneration _levelGenerator;
    public GameObject ballPlayerInstance;
    public bool setInstance;
    public bool hasStarted;
    public static int LevelID = 0;
    private int lastThemeChanged = 0;
    [SerializeField]
    private AudioSource levelMusic;
    public bool paused;
    [SerializeField]
    private TMP_Text _levelProgress;

    public int currentGemsCount, currentCrownsCount, currentProgress;

    [SerializeField]
    private GameObject _pauseCanvas;
    private int touchCount;
    [SerializeField]
    private UnityEngine.UI.Button _pauseButton;
    [SerializeField]
    private TMP_Text timer;
    [SerializeField]
    private GameObject _continueButton;
    float timeEnumerator = 3f;
    private bool continuing;

    [SerializeField]
    private GameObject _failedCanvas;
    private void Awake()
    {
        ballPlayerInstance = GameObject.FindGameObjectWithTag("Player");
        instance = this;
        LevelID = CrossingToLevel.playingLevelID;
        setInstance = true;
        _levelGenerator.GetMapFromFile(LevelID);
        _levelGenerator.GenerateLevel();
        if (TextureManager.instance != null && Theme_EventManager.instance != null)
        {
            TextureManager.instance.EnemyChangeTextures(Theme_EventManager.instance.themeIDs[0]);
            lastThemeChanged++;
        }
        ballPlayerInstance.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color_Sets.ballColorSet[Theme_EventManager.instance.themeIDs[0]] / 255);
        ballPlayerInstance.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        ballPlayerInstance.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color_Sets.ballEmissionSet[Theme_EventManager.instance.themeIDs[0]] / 255);
        levelMusic.clip = Resources.Load<AudioClip>($"Music/Music{LevelID}");
    }
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            hasStarted = true;
        }
        if(setInstance && lastThemeChanged < Theme_EventManager.instance.themePositions.Length && ballPlayerInstance.transform.position.z >= Theme_EventManager.instance.themePositions[lastThemeChanged] && Theme_EventManager.instance != null)
        {
            TextureManager.instance.EnemyChangeTextures(Theme_EventManager.instance.themeIDs[lastThemeChanged]);
            ballPlayerInstance.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color_Sets.ballColorSet[Theme_EventManager.instance.themeIDs[lastThemeChanged]] / 255);
            ballPlayerInstance.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            ballPlayerInstance.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color_Sets.ballEmissionSet[Theme_EventManager.instance.themeIDs[lastThemeChanged]] / 255);
            lastThemeChanged++;
        }
        if(hasStarted)
        {
            if(!levelMusic.isPlaying)
            {
                levelMusic.Play();
            }
            if(paused || (BallPlayer_CollisionSystem.instance != null && BallPlayer_CollisionSystem.instance.Failed))
            {
                levelMusic.Pause();
            }
            else
            {
                levelMusic.UnPause();
            }
            if(BallPlayer_CollisionSystem.instance != null && BallPlayer_CollisionSystem.instance.Failed)
            {
                _failedCanvas.SetActive(true);
            }
        }
        currentProgress = Mathf.RoundToInt((ballPlayerInstance.transform.position.z * 100) / LevelInfo_Data._originalLevelsInformation[LevelID]._levelLength);
        _levelProgress.text = $"{currentProgress}%";
        if(continuing)
        {
            _continueButton.SetActive(false);
            timer.transform.gameObject.SetActive(true);
            timeEnumerator -= Time.deltaTime;
            timer.text = timeEnumerator.ToString();
        }
    }
    public void SaveAfterDie()
    {
        if(currentProgress > SaveManager.instance.saveData._level0_progress)
        {
            SaveManager.instance.saveData._level0_progress = currentProgress;
        }
        if (currentGemsCount > SaveManager.instance.saveData._level0_gems)
        {
            SaveManager.instance.saveData._level0_gems = currentGemsCount;
        }
        if (currentCrownsCount > SaveManager.instance.saveData._level0_crowns)
        {
            SaveManager.instance.saveData._level0_crowns = currentCrownsCount;
        }
        SaveManager.instance.Save();
    }
    public void OnPauseButton()
    {
        paused = true;
        _pauseCanvas.SetActive(true);
        touchCount++;
        if(touchCount == 2)
        {
            SceneManager.LoadScene("GameMainMenuScene");
        }
    }
    public void OnContinueButton()
    {
        continuing = true;
        StartCoroutine(ContinueGame());
    }
    public IEnumerator ContinueGame()
    {
        yield return new WaitForSeconds(3f);
        paused = false;
        touchCount = 0;
        _pauseCanvas.SetActive(false);
        _continueButton.SetActive(true);
        timer.transform.gameObject.SetActive(false);
        continuing = false;
        timeEnumerator = 3f;
    }
    public void Restart()
    {
        SceneManager.LoadScene("PlayScene");
        _pauseButton.interactable = false;
    }
    public void LeaveToMenu()
    {
        SceneManager.LoadScene("GameMainMenuScene");
        _pauseButton.interactable = false;
    }
}
