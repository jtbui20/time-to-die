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
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public List<BombDefinition> BombBagReference;
        public int StageNumber;
        public List<string> StageHistory;
        public LevelScriptableObject CurrentLevel;

        // public Map MapReference;
    }
}