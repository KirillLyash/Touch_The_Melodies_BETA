using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Item_IDManager
{
    public static List<string> ItemNames = new List<string>()
    {
        "00_Air", //0
        "01_Tile", //1
        "02_Jumppad", //2
        "UniversalTiles/T", //3
        "UniversalTiles/T", //4
        "UniversalTiles/T", //5
        "UniversalTiles/T", //6
        "UniversalTiles/L", //7
        "UniversalTiles/L", //8
        "UniversalTiles/L", //9
        "UniversalTiles/L", //10
        "UniversalTiles/D", //11
        "UniversalTiles/D", //12
        "UniversalTiles/D", //13
        "UniversalTiles/D", //14
        "UniversalTiles/I", //15
        "UniversalTiles/I", //16
        "03_MoverArrow_Forward", //17
        "03_MoverArrow_Right", //18
        "03_MoverArrow_Backward", //19
        "03_MoverArrow_Left", //20
        "03_MoverArrowAuto_Forward", //21
        "03_MoverArrowAuto_Right", //22
        "03_MoverArrowAuto_Backward", //23
        "03_MoverArrowAuto_Left", //24
        "04_Slider", //25
        "04_Slider", //26
        "05_Fragile", //27
        "05_Fragile", //28
        "05_Fragile", //29
        "05_Fragile", //30
        "enemyRiser", //31
        "BinaryMini", //32
        "BinaryMini", //33
        "enemyFloater", //34
        "EnemyCrasher(UP)", //35
        "EnemyCrasher(X)", //36
        "Spotlight", //37
        "Laser", //38
        "enemyRiser", //39
        "enemyRiser", //40
        "08_Pickup", //41
        "Crown", //42
        "08_Pickup", //43
        "Crown", //44
        "Flyer", //45
    };
    public static Dictionary<int, string> TileType = new Dictionary<int, string>()
    {
        { 31 , "01_Tile" },
        { 32 , "01_Tile" },
        { 33 , "01_Tile" },
        { 34 , "01_Tile" },
        { 35 , "01_Tile" },
        { 36 , "01_Tile" },
        { 37 , "01_Tile" },
        { 38 , "01_Tile" },
        { 39, "04_Slider" },
        { 40, "04_Slider" },
        { 41, "01_Tile" },
        { 42, "01_Tile" },
        { 43, "02_Jumppad" },
        { 44, "02_Jumppad" },
        { 45, "01_Tile" },

    };
    public static Dictionary<int, string> ObstacleOrientation = new Dictionary<int, string>()
    {
        { 25 , "Right" },
        { 26 , "Left" },
        { 32 , "Right" },
        { 33 , "Left" },
        { 39, "Left" },
        { 40, "Right" },
    };
    public static Dictionary<int, int> fragileIdentificationIDs = new Dictionary<int, int>()
    {
        { 27, 0 },
        { 28, 1 },
        { 29, 2 },
        { 30, 3 },
    };
    public static Dictionary<int, int> UniversalTilesOrientation = new Dictionary<int, int>()
    {
        { 3 , 0 },
        { 4, 90 },
        { 5 , 180 },
        { 6 , 270 },
        { 7 , 0 },
        { 8 , 90 },
        { 9 , 180 },
        { 10 , 270 },
        { 11 , 180 },
        { 12 , 270 },
        { 13 , 0 },
        { 14 , 90 },
        { 15 , 90 },
        { 16 , 0 },

    };
}
