using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("References")] 
        public Button NewRunButton;
        public Button OptionsButton;
        public LoadoutViewController LoadoutSelection;

        [Header("Loadouts")]
        public LoadoutScriptableObject BasicBoyLoadout;

        [Header("First Maps")] public List<LevelScriptableObject> FirstMaps;
        
        private LoadoutScriptableObject selectedLoadout;

        private void Start()
        {
            LoadoutSelection.OnSetLoadout += (loadout) => selectedLoadout = loadout;
        }

        public void OnNewRunPressed()
        {
            LoadoutSelection.gameObject.SetActive(true);
        }

        public void OnBackButtonPressed()
        {
            LoadoutSelection.gameObject.SetActive(false);
        }


        public void ActuallyStartRun()
        {
            PlayerData data = ScriptableObject.CreateInstance<PlayerData>();
            data.BombBagReference = selectedLoadout.bombsInLoadout;
            // Pick a random first map
            data.CurrentLevel = FirstMaps[Random.Range(0, FirstMaps.Count)];
            data.StageNumber = 0;
            data.StageHistory = new List<string>();
            SessionManager session = FindAnyObjectByType<SessionManager>();
            
            session.NewRun(data);
        }
    }
}