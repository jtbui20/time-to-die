using DefaultNamespace;
using UnityEngine;

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
    public int Lives;
    public int CurrentTurn = 0;

    private GameplayPlayerInstance player;

    private BombManager _bombManager;

    private EnemyManager _enemyManager;
    
    
    void Start()
    {
        ResetScenario();
    }

    public void ResetScenario()
    {
        Lives = 5;
        CurrentTurn = 0;
    }

    public void SetupGameplay(GameplayPlayerInstance player)
    {
        this.player = player;
    }

    // This offloads work done from the Session manager to the Gameplay scenario manager's playable director
    public void InitializeGameplaySequences()
    {
        StartScenario();
    }

    public void Deconstruct()
    {
        Destroy(_bombManager);
        Destroy(_enemyManager);
    }

    public void StartScenario()
    {
        // Start with enemy turn spawning
        // _enemyManager.SpawnEnemies()
        
        // Then we transition to turn start
        Debug.Log("Starting Scenario");
        TurnStart();
    }

    void TurnStart()
    {
        // Show turn start
        CurrentTurn++;
        player.BombDeck.Draw(player.MaxHandSize);
        _bombManager.Tick();
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
        _bombManager.Tick();
        await Awaitable.WaitForSecondsAsync(3f);

        // _enemyManager.ProcessDamage();
        // _enemyManager.ProcessDeathChains();
    }

    async Awaitable EnemyTurn()
    {
        // _enemyManager.ProcessStep();
        await Awaitable.WaitForSecondsAsync(3f);
    }
}
