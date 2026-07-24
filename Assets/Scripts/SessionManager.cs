using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SessionManager : MonoBehaviour
    {
        public PlayerData player;

        public GameObject GameplayManagerPrefab;

        public bool ImmediateStart = false;
        
        private GameplayScenarioManager gameplayManager;
        
        private Scene currentScene;

        void Start()
        {
            DontDestroyOnLoad(this);
            if (ImmediateStart)
            {
                SetupNextStage();
            }
        }
        

        public void NewRun(PlayerData player)
        {
            this.player = player;
            SetupNextStage();
        }

        async Awaitable SetupNextStage()
        {
            player.StageNumber++;
            player.StageHistory.Add(player.CurrentLevel.LevelName);
            int buildIndex = player.CurrentLevel.SceneToLoad.BuildIndex;
            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
            // Set that scene active
            
            currentScene = SceneManager.GetSceneByBuildIndex(buildIndex);
            // SceneManager.SetActiveScene(currentScene);
            
            gameplayManager = FindAnyObjectByType<GameplayScenarioManager>();
            if (gameplayManager == null)
            {
                gameplayManager = Instantiate(GameplayManagerPrefab).GetComponent<GameplayScenarioManager>();
            }
            gameplayManager.Inject(player);
            
            // The gameplay manager will start when it's ready
        }

        void DeconstructCurrentScene()
        {
            if (gameplayManager != null)
            {
                gameplayManager.Deconstruct();
                Destroy(gameplayManager.gameObject);
            }
            
            if (currentScene.IsValid())
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }
            
        }
    }
}