using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class LoseOverlayController : MonoBehaviour
    {
        public Button returnToMenuButton;
        
        public RoundStatsView roundStatsView;

        public event Action OnReturnToMenuPressed;
        
        public void SetStats(GameScenarioStats stats)
        {
            roundStatsView.SetStats(stats);
        }

        private void Start()
        {
            returnToMenuButton.onClick.AddListener(() => OnReturnToMenuPressed?.Invoke());
        }
    }
}