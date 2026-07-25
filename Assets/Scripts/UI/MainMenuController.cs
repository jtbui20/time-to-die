using System.Collections.Generic;
using DefaultNamespace.Data;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("References")]
        public GameObject LoadoutSelection;

        [Header("Loadouts")]
        public LoadoutScriptableObject BasicBoyLoadout;

        [Header("First Maps")] public List<LevelScriptableObject> FirstMaps;
        
        private LoadoutScriptableObject selectedLoadout;

        public void OnNewRunPressed()
        {
            LoadoutSelection.SetActive(true);
        }

        public void OnBackButtonPressed()
        {
            LoadoutSelection.SetActive(false);
        }


        public void OnBasicBoyPressed()
        {
            selectedLoadout = BasicBoyLoadout;
            Debug.Log(selectedLoadout.Name);
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