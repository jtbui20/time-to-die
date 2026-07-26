using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.Game_State;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

public enum GameplayStates
{
    GameStart,
    TurnStart,
    PlayerTurn,
    PlayerExit,
    Detonation,
    EnemyTurn,
    GameEnd,
}

public enum GameEndingBecause
{
    None,
    Win,
    Lose,
}

public enum GameLeavingReason
{
    ReturnToMenu,
    NextLevel,
    HardQuit
}

public class GameplayScenarioManager : MonoBehaviour
{
    // TODO: SO for config
    public int Lives;
    public int CurrentTurn = 0;
    public GameplayStates CurrentState;

    public Object MapData;
    public NavMeshDataInstance NavMeshData;

    private GameplayPlayerInstance player;
    private BombManager _bombManager;
    private EnemyManager _enemyManager;

    private GameScenarioDirector _director;
    private GameplayUIPresenter _uiPresenter;
    private RackController _rackController;
    private BPMActionSynchronizer _bpmSynchronizer;

    public bool ImmediateStart = false;
    public GameEndingBecause EndGameReason;

    public event Action<GameLeavingReason> OnGameLeave;

    void Awake()
    {
        player = GetComponent<GameplayPlayerInstance>();
        _bombManager = GetComponent<BombManager>();
        _enemyManager = GetComponent<EnemyManager>();
        _director = GetComponentInChildren<GameScenarioDirector>();
        _uiPresenter = GetComponentInChildren<GameplayUIPresenter>();
        _rackController = GetComponentInChildren<RackController>();
        _bpmSynchronizer = GetComponentInChildren<BPMActionSynchronizer>();
    }
    
    void Start()
    {
        if (ImmediateStart)
        {
            Setup();
            SwitchToState(GameplayStates.GameStart);
        }
    }

    public void Inject(PlayerData player)
    {
        GetComponent<GameplayPlayerInstance>().Inject(player);
    }

    void Setup()
    {
        player.InitializePlayer();
        Lives = player.PlayerData.CurrentLevel.StartingLives;
        // Spawn in the UI prefab
        LinkUI();
        _director.OnTimelineCompleted += OnTimelineCompleted;
        _rackController._bombManager = _bombManager;
        _rackController.SetInteraction(false);

        this.NavMeshData = NavMesh.AddNavMeshData(player.PlayerData.CurrentLevel.LevelNavigation.NavMeshData);
        _enemyManager.SetupSpawner(player.PlayerData.CurrentLevel.SpawnerSchedule, player.PlayerData.CurrentLevel.LevelNavigation.Waypoints);

        _enemyManager.OnEnemyCountChanged += GeneralUpdateUI;
        _enemyManager.OnEnemyEscape += LoseLife;
        _enemyManager.OnEnemiesDefeated += EnemyDefeat;
        EnemyTurn();
    }

    void LinkUI()
    {
        _uiPresenter.OnEndTurnButtonPressed += PlayerTurnButtonPressed;
        _uiPresenter.OnEndGameConfirmButtonPressed += () => EndGameWithReason(GameEndingBecause.Lose);
        _uiPresenter.OnLeaveGameRequested += OnGameLeave;
        _uiPresenter.OnForceWin += () => EndGameWithReason(GameEndingBecause.Win);
    }
    
    void PlayerTurnButtonPressed()
    {
        // Only do this if 
        SwitchToState(GameplayStates.PlayerExit);
    }
    
    void EndGameWithReason(GameEndingBecause reason)
    {
        EndGameReason = reason;
        SwitchToState(GameplayStates.GameEnd);
    }

    public void GeneralUpdateUI()
    {
        _uiPresenter.UpdateStageText(CurrentState);
        _uiPresenter.UpdateEnemiesLeftText(_enemyManager.EnemyCount);
    }

    public void GoNextStage()
    {
        OnGameLeave?.Invoke(GameLeavingReason.NextLevel);
    }

    public void GoMenu()
    {
        OnGameLeave?.Invoke(GameLeavingReason.ReturnToMenu);
    }

    public void Deconstruct()
    {
        _enemyManager.OnEnemyCountChanged -= GeneralUpdateUI;
        _enemyManager.OnEnemyEscape -= LoseLife;
        _enemyManager.OnEnemiesDefeated -= EnemyDefeat;
        NavMesh.RemoveNavMeshData(this.NavMeshData);
        Destroy(gameObject);
    }

    private void SwitchToState(GameplayStates state)
    {
        CurrentState = state;
        Debug.Log($"Switching to state: {state}");
        _uiPresenter.UpdateStageText(CurrentState);
        switch (CurrentState)
        {
            case GameplayStates.GameStart:
                StartScenario();
                break;
            case GameplayStates.TurnStart:
                TurnStart();
                break;
            case GameplayStates.PlayerTurn:
                PlayerTurn();
                break;
            case GameplayStates.PlayerExit:
                PlayerEndTurn();
                break;
            case GameplayStates.Detonation:
                DetonationStep().Forget();
                break;
            case GameplayStates.EnemyTurn:
                EnemyTurn();
                break;
            case GameplayStates.GameEnd:
                GameEnd();
                break;
            default:
                throw new System.NotImplementedException($"State {state} is not implemented");
        }
        
        _director.PlayState(CurrentState);
    }

    private void OnTimelineCompleted(GameplayStates state)
    {
        switch (state)
        {
            case GameplayStates.GameStart:
                SwitchToState(GameplayStates.TurnStart);
                break;
            case GameplayStates.TurnStart:
                SwitchToState(GameplayStates.PlayerTurn);
                break;
            case GameplayStates.PlayerExit:
                SwitchToState(GameplayStates.Detonation);
                break;
            case GameplayStates.Detonation:
                SwitchToState(GameplayStates.EnemyTurn);
                break;
            case GameplayStates.EnemyTurn:
                SwitchToState(GameplayStates.TurnStart);
                break;
            default:
                Debug.LogWarning($"No next state defined for {CurrentState}");
                break;
        }
    }

    void StartScenario()
    {
        // Start with enemy turn spawning
        // _enemyManager.SpawnEnemies()
        
        // Then we transition to turn start
        _bpmSynchronizer.StartTimer();
    }

    void TurnStart()
    {
        // Show turn start
        CurrentTurn++;
        int drawCount = player.MaxHandSize - player.BombDeck.PileHand.Count;
        player.BombDeck.Draw(player.MaxHandSize);
        
        _rackController.LoadInNewBombs(player.BombDeck.PileHand.ViewPile().ToList());
        
        _bombManager.CountdownBombs();
    }

    void PlayerTurn()
    {
        // Enable inputs and such
        _rackController.SetInteraction(true);
        _uiPresenter.SetInteraction(true);
    }

    void PlayerEndTurn()
    {
        // Gather Information
        // _bombManager.Tick();
        player.BombDeck.DiscardAllHand();
        _rackController.HandleDiscard();
    }

    async UniTask DetonationStep()
    {
        _rackController.SetInteraction(false);
        _uiPresenter.SetInteraction(false);
        _bombManager.GenerateBombActionQueue();
        
        await _bombManager.WaitForBombsToComplete();
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        // _enemyManager.ProcessDamage();
        // _enemyManager.ProcessDeathChains();
        
        SwitchToState(GameplayStates.EnemyTurn);
    }

    void EnemyTurn()
    {
        _enemyManager.ProcessStep();
        
        _enemyManager.SpawnWave(CurrentTurn);
    }

    void GameEnd()
    {

        // Configure which end screen to show
        _director.SetGameEndReason(EndGameReason);
    }

    public void LoseLife()
    {
        Lives -= 1;
        if (Lives <= 0)
        {
            EndGameReason = GameEndingBecause.Lose;
            SwitchToState(GameplayStates.GameEnd);
        }
    }

    public void EnemyDefeat()
    {
        EndGameReason = GameEndingBecause.Win;
        SwitchToState(GameplayStates.GameEnd);
    }
}
