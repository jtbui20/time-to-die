using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class SingleRewardView : MonoBehaviour
    {
        public Button selfSelect;
        public Reward reward;
        
        [SerializeField]
        private TextMeshProUGUI rewardNameText;
        [SerializeField]
        private TextMeshProUGUI rewardDescriptionText;

        private event Action OnRewardSelected;

        private void Start()
        {
            selfSelect.onClick.AddListener(() => OnRewardSelected?.Invoke());
        }
        
        public void SetReward(Reward reward, Action OnRewardSelected)
        {
            this.reward = reward;
            this.OnRewardSelected = OnRewardSelected;
            UpdateView();
        }

        void UpdateView()
        {
            rewardNameText.text = reward.rewardType.ToString();
            rewardDescriptionText.text = $"Level {((int)reward.rewardLevel + 1)} {reward.rewardType}";
        }
    }
}