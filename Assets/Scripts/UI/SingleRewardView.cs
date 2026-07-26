using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class SingleRewardView : MonoBehaviour
    {
        // public Button selfSelect;
        public BombDefinition bombDefinition;
        
        [SerializeField]
        private BombItemInventoryView bombItemInventoryView;

        public Image Background;

        public Color selectedBackground;
        public Color unselectedBackground;
        
        private bool IsSelected = false;

        public Button selfButton;

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            
            Debug.Log(isSelected);

            Background.color = IsSelected ? selectedBackground : unselectedBackground;
        }
        public event Action<BombDefinition> OnRewardSelected;

        private void Start()
        {
            
        }
        
        public void SetReward(BombDefinition bombDefinition)
        {
            this.bombDefinition = bombDefinition;
            bombItemInventoryView.SetBombTarget(bombDefinition);
            selfButton.onClick.AddListener(() => OnRewardSelected?.Invoke(bombDefinition));
        }
    }
}