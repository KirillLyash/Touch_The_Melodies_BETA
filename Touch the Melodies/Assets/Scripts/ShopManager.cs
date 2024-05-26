using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static int energyCount;
    public static int coinCount;

    [SerializeField]
    private TMPro.TMP_Text _coinCount, _energyCount;

    private void Start()
    {
        energyCount = SaveManager.instance.saveData._energyCount;
        coinCount = SaveManager.instance.saveData._coinsCount;
        _coinCount.text = coinCount.ToString();
        _energyCount.text = energyCount.ToString();
    }
}
