using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    public static Dictionary<int, List<string>> originalLevelsTranslation = new Dictionary<int, List<string>>()
    {
        { 0, new List<string>() { "Refreshing Island", "Освежающий остров" } }
    };
    public static Dictionary<int, List<string>> uiTranslation = new Dictionary<int, List<string>>()
    {
        { 0, new List<string>() { "Play", "Играть" } },
        { 1, new List<string>() { "Home", "Домой" } },
        { 2, new List<string>() { "Special", "Специальное" } },
        { 3, new List<string>() { "Shop", "Магазин" } },
        { 4, new List<string>() { "Achievements", "Достижения" } },
        { 5, new List<string>() { "Settings", "Настройки" } },
        { 6, new List<string>() { "Account", "Аккаунт" } },
        { 7, new List<string>() { "Pause", "Пауза" } },
        { 8, new List<string>() { "Restart", "Начать заново" } },
    };
}
