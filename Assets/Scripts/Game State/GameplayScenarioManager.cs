using DefaultNamespace;
using DefaultNamespace.Game_State;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.Playables;

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

public class GameplayScenarioManager : MonoBehaviour
{
    // TODO: SO for config
    public int Lives;
    public int CurrentTurn = 0;
    public GameplayStates CurrentState;

    public Object MapData;

    private GameplayPlayerInstance player;
    private BombManager _bombManager;
    private EnemyManager _enemyManager;

    private GameScenarioDirector _director;
    private GameplayUIPresenter _uiPresenter;

    public bool ImmediateStart = false;

    void Awake()
    {
        player = GetComponent<GameplayPlayerInstance>();
        _bombManager = GetComponent<BombManager>();
        _enemyManager = GetComponent<EnemyManager>();
        _director = GetComponentInChildren<GameScenarioDirector>();
        _uiPresenter = GetComponentInChildren<GameplayUIPresenter>();
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
        // Spawn in the UI prefab
        _uiPresenter.OnEndTurnButtonPressed += OnPlayerEndTurn;
        _director.OnTimelineCompleted += OnTimelineCompleted;
    }

    public void GeneralUpdateUI()
    {
        _uiPresenter.UpdateStageText(CurrentState);
        _uiPresenter.UpdateEnemiesLeftText(15);
    }

    public void Deconstruct()
    {
        Destroy(gameObject);
    }

    public void SwitchToState(GameplayStates state)
    {
        CurrentState = state;
        Debug.Log($"Switching to state: {state}");
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
                DetonationStep();
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

    public void OnTimelineCompleted(GameplayStates state)
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
    }

    void TurnStart()
    {
        // Show turn start
        CurrentTurn++;
        player.BombDeck.Draw(player.MaxHandSize);
        
        // _bombManager.Tick();
    }

    void PlayerTurn()
    {
        // Enable inputs and such
    }

    void OnPlayerEndTurn()
    {
        SwitchToState(GameplayStates.PlayerExit);
    }

    void PlayerEndTurn()
    {
        // Gather Information
    }

    async Awaitable DetonationStep()
    {
        // This should be a "detonate & tick" function
        // await
        
        // This will eventually tick multiple times by a separate function
        // _bombManager.Tick();

        // _enemyManager.ProcessDamage();
        // _enemyManager.ProcessDeathChains();
    }

    async Awaitable EnemyTurn()
    {
        // _enemyManager.ProcessStep();
    }

    async Awaitable GameEnd()
    {
    }
}
