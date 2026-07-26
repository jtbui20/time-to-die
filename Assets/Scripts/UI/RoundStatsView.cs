using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{

    public class GameScenarioStats
    {
        public int StageCount;
        public int starsEarnt;
        public int livesRemaining;
        public int MostDamage;
        public int TurnsTaken;
        public int LongestChain;
        
        public GameScenarioStats(int stage, int lives, int damage, int turns, int chain)
        {
            StageCount = stage;
            starsEarnt = lives switch
            {
                5 => 3,
                3 => 2,
                1 => 1,
                _ => 0
            };
            livesRemaining = lives;
            MostDamage = damage;
            TurnsTaken = turns;
            LongestChain = chain;
        }
    }
    
    public class RoundStatsView : MonoBehaviour
    {
        public List<GameObject> starObjects;
        public TextMeshProUGUI MostDamageText;
        public TextMeshProUGUI LongestChainText;
        public TextMeshProUGUI TurnsTakenText;
        public TextMeshProUGUI StageCountText;

        private GameScenarioStats _statReference;

        private void Start()
        {
            if (_statReference != null)
            {
                UpdateView();
            }
        }

        public void SetStats(GameScenarioStats stats)
        {
            _statReference = stats;
            UpdateView();
        }

        void UpdateView()
        {
            for (int i = 0; i < starObjects.Count; i++)
            {
                starObjects[i].SetActive(i < _statReference.starsEarnt);
            }

            MostDamageText.text = _statReference.MostDamage.ToString();
            LongestChainText.text = _statReference.LongestChain.ToString();
            TurnsTakenText.text = _statReference.TurnsTaken.ToString();
            StageCountText.text = _statReference.StageCount.ToString();
        }
    }
}