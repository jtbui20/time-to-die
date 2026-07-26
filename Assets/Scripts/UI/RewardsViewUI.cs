using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.UI
{
    
    public class RewardsViewUI : MonoBehaviour
    {
        public event Action<BombDefinition> OnRewardSelected;
        public List<SingleRewardView> rewardViews = new();
        
        private List<BombDefinition> rewardList = new();
        
        // Tell the actual controller that we've selected

        public void SetRewards(List<BombDefinition> bombDefinitions)
        {
            ConfigureViews(bombDefinitions);
            Debug.Log($"RewardsViewUI: SetRewards called with {bombDefinitions.Count} rewards.");
        }
        
        public void SetSelectedReward(BombDefinition bombDefinition)
        {
            foreach (var rewardView in rewardViews)
            {
                if (rewardView.bombDefinition == bombDefinition)
                {
                    rewardView.SetSelected(true);
                }
                else
                {
                    rewardView.SetSelected(false);
                }
            }

            OnRewardSelected?.Invoke(bombDefinition);
        }

        void ConfigureViews(List<BombDefinition> bombDefinitions)
        {
            rewardList = bombDefinitions;
            for (int i = 0; i < rewardViews.Count; i++)
            {
                var rewardView = rewardViews[i];
                rewardView.SetReward(bombDefinitions[i]);
                rewardView.OnRewardSelected += SetSelectedReward;
            }
        }
    }
}