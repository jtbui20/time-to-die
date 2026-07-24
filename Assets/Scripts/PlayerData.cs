using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using GameFramework.Cards;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// This is more like save data imo
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        public List<BombDefinition> BombBagReference;
        public int StageNumber;
        public List<string> StageHistory;
        public LevelScriptableObject CurrentLevel;

        // public Map MapReference;
    }
}