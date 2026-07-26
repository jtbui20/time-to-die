using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DefaultNamespace.UI
{
    public enum RewardType
    {
        Bomb,
        Upgrade
    }

    public enum RewardLevel
    {
        Common, // Level 1
        Rare, // Level 2
        Epic // Level 3
    }
    
    public class Reward
    {
        public RewardType rewardType;   
        public RewardLevel rewardLevel;
    }
    
    public class RewardsViewUI : MonoBehaviour
    {
        public event Action<Reward> OnRewardSelected;
        public List<SingleRewardView> rewardViews = new List<SingleRewardView>();
        
        // Tell the actual controller that we've selected

        public void SetRewards(List<Reward> reward)
        {
            ConfigureViews();
        }

        void ConfigureViews()
        {
            for (int i = 0; i < rewardViews.Count; i++)
            {
                var rewardView = rewardViews[i];
                rewardView.SetReward(rewardView.reward, () => OnRewardSelected?.Invoke(rewardView.reward));
            }
        }
    }
}