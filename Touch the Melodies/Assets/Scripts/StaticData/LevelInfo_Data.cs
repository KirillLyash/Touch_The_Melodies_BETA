using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelInfo_Data
{
    public static Dictionary<int, LevelInformation> _originalLevelsInformation = new Dictionary<int, LevelInformation>
    {
        { 
            0, new LevelInformation() { _levelSpeed = 8.1f, _levelLength = 709, _checkpoints = new List<int>() {123, 245}, _gemsCount = 10, _hasCrowns = true }
        },
        {
            1, new LevelInformation() { _levelSpeed = 9.45f, _levelLength = 1265, _checkpoints = new List<int>() {23, 145}, _gemsCount = 10, _hasCrowns = false }
        },
    };
    public static Dictionary<int, LevelOrder> _levelOrdering = new Dictionary<int, LevelOrder>
    {
        {
            0, new LevelOrder() { _difficultyOrderNum = 0, _releaseOrderNum = 0, _alphabetOrderNum = 0 }
        }
    };
    public struct LevelInformation
    {
        public float _levelSpeed;
        public int _levelLength;
        public List<int> _checkpoints;
        public int _gemsCount;
        public bool _hasCrowns;
    }
    public struct LevelOrder
    {
        public int _difficultyOrderNum;
        public int _releaseOrderNum;
        public int _alphabetOrderNum;
    }
}
