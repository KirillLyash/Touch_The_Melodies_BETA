using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject _setUp;
    private bool activated = false;

    public void OnParameter()
    {
        activated = !activated;
        _setUp.SetActive(activated);
    }
}
