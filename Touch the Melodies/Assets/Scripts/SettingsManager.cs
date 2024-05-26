using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Button[] _languageParameter;
    [SerializeField]
    private TMPro.TMP_Text _languageparameter;

    [SerializeField]
    private Button[] _visualAngleParameter;
    [SerializeField]
    private TMPro.TMP_Text _angleparameter;

    public int _languageID;
    public int _visualAngleID;
    private void Start()
    {
        _languageID = SaveManager.instance.saveData._languageID;
        _visualAngleID = SaveManager.instance.saveData._visualAngleID;
        Debug.Log(_languageID);
        Debug.Log(PlayerPrefs.GetInt("LanguageID"));
        OnLanguageSelected(_languageID);
        OnVisualAngleChanged(_visualAngleID);
    }
    public void OnLanguageSelected(int languageID)
    {
        _languageID = languageID;
        SaveManager.instance.saveData._languageID = languageID;
        SaveManager.instance.Save();
        for (int i = 0; i < _languageParameter.Length; i++)
        {
            if (_languageID == i)
            {
                _languageParameter[i].transform.GetChild(0).gameObject.SetActive(true);
                _languageParameter[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _languageParameter[i].transform.GetChild(0).gameObject.SetActive(false);
                _languageParameter[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        _languageparameter.text = _languageParameter[languageID].transform.parent.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text;
    }
    public void OnVisualAngleChanged(int angleID)
    {
        _visualAngleID = angleID;
        SaveManager.instance.saveData._visualAngleID = angleID;
        SaveManager.instance.Save();
        for (int i = 0; i < _visualAngleParameter.Length; i++)
        {
            if (_visualAngleID == i)
            {
                _visualAngleParameter[i].transform.GetChild(0).gameObject.SetActive(true);
                _visualAngleParameter[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _visualAngleParameter[i].transform.GetChild(0).gameObject.SetActive(false);
                _visualAngleParameter[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        _angleparameter.text = _visualAngleParameter[angleID].transform.parent.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text;
    }
}
