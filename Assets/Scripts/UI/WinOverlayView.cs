using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class WinOverlayView : MonoBehaviour
    {

        public Button GoNextButton;
        
        public RewardsViewUI rewardsView;
        public RoundStatsView roundStatsView;

        public event Action OnGoNextPressed;
        public event Action<BombDefinition> OnRewardSelected;

        public void ShowStats(GameScenarioStats stats, List<BombDefinition> rewards)
        {
            roundStatsView.SetStats(stats);
            rewardsView.SetRewards(rewards);
        }

        private void Start()
        {
            GoNextButton.onClick.AddListener(() => OnGoNextPressed?.Invoke());
            rewardsView.OnRewardSelected += (reward) => OnRewardSelected?.Invoke(reward);
        }
        
    }
}