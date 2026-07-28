using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DefaultNamespace.Game_State
{
    public class GameScenarioDirector : MonoBehaviour
    {
        private PlayableDirector _director;
        private SignalReceiver _receiver;
        
        private GameplayStates _currentState;
        
        public PlayableAsset scenarioStartTimeline;
        public PlayableAsset turnStartTimeline;
        public PlayableAsset playerTurnStartTimeline;
        public PlayableAsset playerTurnExitTimeline;
        public PlayableAsset detonationTimeline;
        public PlayableAsset enemyTurnTimeline;
        
        [Header("Game End Timelines")]
        public PlayableAsset gameWinTimeline;
        public PlayableAsset gameLoseTimeline;

        public event Action<GameplayStates> OnTimelineCompleted ;
        
        private GameEndingBecause _endGameReason = GameEndingBecause.None;

        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
            _receiver = GetComponent<SignalReceiver>();
        }

        public void SetGameEndReason(GameEndingBecause endGameReason)
        {
            _endGameReason = endGameReason;
        }

        public void PlayState(GameplayStates state)
        {
            _currentState = state;
            Debug.Log($"Playing timeline for state {state}");
            switch (state)
            {
                case GameplayStates.GameStart:
                    _director.Play(scenarioStartTimeline);
                    break;
                case GameplayStates.TurnStart:
                    _director.Play(turnStartTimeline);
                    break;
                case GameplayStates.PlayerTurn:
                    _director.Play(playerTurnStartTimeline);
                    break;
                case GameplayStates.PlayerExit:
                    _director.Play(playerTurnExitTimeline);
                    break;
                // case GameplayStates.Detonation:
                //     _director.Play(detonationTimeline);
                //     break;
                case GameplayStates.EnemyTurn:
                    _director.Play(enemyTurnTimeline);
                    break;
                case GameplayStates.GameEnd:
                    if (_endGameReason != GameEndingBecause.None)
                    {
                        // Play the appropriate end game timeline based on the reason
                        switch (_endGameReason)
                        {
                            case GameEndingBecause.Win:
                                _director.Play(gameWinTimeline);
                                break;
                            case GameEndingBecause.Lose:
                                _director.Play(gameLoseTimeline);
                                break;
                        }
                    }

                    break;
                default:
                    Debug.LogWarning($"No timeline defined for state {state}");
                    return;
            }
        }

        public void OnCompleted()
        {
            OnTimelineCompleted?.Invoke(_currentState);
        }
    }
}