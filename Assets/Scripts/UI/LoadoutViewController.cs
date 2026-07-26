using System;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class LoadoutViewController : MonoBehaviour
    {
        public Button basicBoyButton;
        public Button chainCrazyButton;
        public Button PureRandomButton;
        
        [Header("Description Area")]
        public TextMeshProUGUI loadoutNameText;
        public TextMeshProUGUI loadoutDescriptionText;
        
        public TextMeshProUGUI loadoutBombsText;
        
        [Header("Loadouts")]
        public LoadoutScriptableObject BasicBoyLoadout;
        public LoadoutScriptableObject ChainCrazyLoadout;
        public LoadoutScriptableObject PureRandomLoadout;

        public event Action<LoadoutScriptableObject> OnSetLoadout;
        
        [SerializeField]
        private LoadoutScriptableObject currentlySelectedLoadout;

        private void Start()
        {
            basicBoyButton.onClick.AddListener(() => SetLoadout(BasicBoyLoadout));
            chainCrazyButton.onClick.AddListener(() => SetLoadout(ChainCrazyLoadout));
            PureRandomButton.onClick.AddListener(() => SetLoadout(PureRandomLoadout));
            if (currentlySelectedLoadout)
            {
                UpdateView();
            }
        }

        void SetLoadout(LoadoutScriptableObject loadout)
        {
            OnSetLoadout?.Invoke(loadout);
            if (currentlySelectedLoadout == loadout) return;
            currentlySelectedLoadout = loadout;
            UpdateView();
        }

        void UpdateView()
        {
            // Update view then animate ig
            loadoutNameText.text = currentlySelectedLoadout.LoadoutName;
            loadoutDescriptionText.text = currentlySelectedLoadout.Description;
        }
    }
}