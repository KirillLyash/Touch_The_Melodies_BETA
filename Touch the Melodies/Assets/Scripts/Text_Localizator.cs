using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Localizator : MonoBehaviour
{
    [SerializeField]
    private int _uiElementID;
    private TMPro.TMP_Text _textField;
    private int _languageID;

    private void Start()
    {
        _languageID = SaveManager.instance.saveData._languageID;
        _textField = GetComponent<TMPro.TMP_Text>();
        _textField.text = Localization.uiTranslation[_uiElementID][_languageID].ToString();
    }
    private void UpdateLanguage()
    {
        _textField.text = Localization.uiTranslation[_uiElementID][_languageID].ToString();
    }

    private void Update()
    {
        if (SaveManager.instance.saveData != null && _languageID != SaveManager.instance.saveData._languageID)
        {
            _languageID = SaveManager.instance.saveData._languageID;
            UpdateLanguage();
        }
    }
}
