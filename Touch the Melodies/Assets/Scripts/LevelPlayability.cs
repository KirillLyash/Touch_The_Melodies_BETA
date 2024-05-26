using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayability : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _levelName, _levelProgress, _gemsCount;
    [SerializeField]
    private GameObject _crownHolder;
    [SerializeField]
    private UnityEngine.UI.Image _levelWorldshow;


    private void OnEnable()
    {
        _levelName.text = Localization.originalLevelsTranslation[CrossingToLevel.playingLevelID][SaveManager.instance.saveData._languageID];
        _levelProgress.text = $"{SaveManager.instance.saveData._level0_progress}%";
        _gemsCount.text = $"{SaveManager.instance.saveData._level0_gems}/10";
        for(int c = 0; c < SaveManager.instance.saveData._level0_crowns; c++)
        {
            _crownHolder.transform.GetChild(c).GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,0);
            _crownHolder.transform.GetChild(c).GetChild(0).gameObject.SetActive(true);
        }
        _levelWorldshow.sprite = Resources.Load<Sprite>($"Worldshows/Worldshow{CrossingToLevel.playingLevelID}");
    }
}
