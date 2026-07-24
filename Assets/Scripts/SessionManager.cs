using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SessionManager : MonoBehaviour
    {
        public PlayerData player;

        public GameObject GameplayManagerPrefab;

        public bool ImmediateStart = false;
        
        private GameplayPlayerInstance playerInstance;
        private GameplayScenarioManager gameplayManager;
        
        private Scene currentScene;

        void Start()
        {
            DontDestroyOnLoad(this);
            if (!ImmediateStart) return;
            SetupNextStage();
        }

        public void InjectData(PlayerData player)
        {
            this.player = player;
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

            playerInstance = FindAnyObjectByType<GameplayPlayerInstance>();
            
            if (playerInstance == null)
            {
                playerInstance = Instantiate(GameplayManagerPrefab).GetComponent<GameplayPlayerInstance>();
            }
            
            playerInstance.Inject(player);
            playerInstance.InitializePlayer();
            
            gameplayManager = playerInstance.GetComponent<GameplayScenarioManager>();
            gameplayManager.SetupGameplay(playerInstance);
            
            
            // When everything is done we start
        }

        void DeconstructCurrentScene()
        {
            if (gameplayManager != null)
            {
                gameplayManager.Deconstruct();
                Destroy(gameplayManager.gameObject);
            }
            
            if (playerInstance != null)
            {
                Destroy(playerInstance.gameObject);
            }
            
            if (currentScene.IsValid())
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }
            
        }
    }
}