using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    public static Dictionary<int, List<string>> originalLevelsTranslation = new Dictionary<int, List<string>>()
    {
        { 0, new List<string>() { "Refreshing Island", "���������� ������" } }
    };
    public static Dictionary<int, List<string>> uiTranslation = new Dictionary<int, List<string>>()
    {
        { 0, new List<string>() { "Play", "������" } },
        { 1, new List<string>() { "Home", "�����" } },
        { 2, new List<string>() { "Special", "�����������" } },
        { 3, new List<string>() { "Shop", "�������" } },
        { 4, new List<string>() { "Achievements", "����������" } },
        { 5, new List<string>() { "Settings", "���������" } },
        { 6, new List<string>() { "Account", "�������" } },
        { 7, new List<string>() { "Pause", "�����" } },
        { 8, new List<string>() { "Restart", "������ ������" } },
    };
}
