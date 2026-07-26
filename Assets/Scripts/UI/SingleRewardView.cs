using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class SingleRewardView : MonoBehaviour, IPointerClickHandler
    {
        // public Button selfSelect;
        public BombDefinition bombDefinition;
        
        [SerializeField]
        private BombItemInventoryView bombItemInventoryView;

        public Image Background;

        public Color selectedBackground;
        public Color unselectedBackground;
        
        private bool IsSelected = false;

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            
            if (IsSelected)
            {
                Background.color = selectedBackground;
            }
            else
            {
                Background.color = unselectedBackground;
            }
        }
        private event Action OnRewardSelected;

        private void Start()
        {
            // selfSelect.onClick.AddListener(() => OnRewardSelected?.Invoke());
        }
        
        public void SetReward(BombDefinition bombDefinition, Action OnRewardSelected)
        {
            this.bombDefinition = bombDefinition;
            this.OnRewardSelected = OnRewardSelected;
            bombItemInventoryView.SetBombTarget(bombDefinition);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnRewardSelected?.Invoke();
        }
    }
}