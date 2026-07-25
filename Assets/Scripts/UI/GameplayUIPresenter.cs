using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class GameplayUIPresenter : MonoBehaviour
    {
        
        public TextMeshProUGUI stageText;
        public TextMeshProUGUI enemiesLeftText;
        public Button endTurnButton;
        public Button settingsButton;

        [Header("Menu Panel")] public GameObject menuPanel;
        public Button optionsButton;
        public Button endGameButton;
        public Button closeButton;
        public Button forceWin;

        [Header("Win Screen Panel")] public GameObject winScreen;
        public Button goNextButton;

        public LoseOverlayController loseScreenPanel;

        [Header("Turn State Texts")]
        public TextMeshProUGUI statePlayerTurnText;
        public TextMeshProUGUI stateEnemyTurnText;

        [Header("UI Prefabs")] public GameObject PopupPrefab;
        
        public event Action OnEndTurnButtonPressed;
        public event Action OnEndGameConfirmButtonPressed;
        public event Action<GameLeavingReason> OnLeaveGameRequested;
        public event Action OnForceWin;

        private Animator UIAnimator;

        private void Awake()
        {
            UIAnimator = GetComponent<Animator>();
        }

        void Start()
        {
            endTurnButton.onClick.AddListener(() => OnEndTurnButtonPressed?.Invoke());
            closeButton.onClick.AddListener(HideMenu);
            settingsButton.onClick.AddListener(ShowMenu);
            endGameButton.onClick.AddListener(() => ShowPopup("Are you sure you want to end the game?",
                () =>
                {
                    HideMenu();
                    OnEndGameConfirmButtonPressed?.Invoke();
                }));
            goNextButton.onClick.AddListener(() => OnLeaveGameRequested?.Invoke(GameLeavingReason.NextLevel));
            forceWin.onClick.AddListener(() => OnForceWin?.Invoke());
            loseScreenPanel.OnReturnToMenuPressed += () => OnLeaveGameRequested?.Invoke(GameLeavingReason.ReturnToMenu);
        }

        void ShowPopup(string message, Action callback)
        {
            var popup = Instantiate(PopupPrefab, transform);
            var popupComponent = popup.GetComponent<PopupWithConfirmCancel>();
            popupComponent.Setup(message, callback);
        }
        
        public void UpdateStageText(GameplayStates stage)
        {
            stageText.text = stage switch
            {
                GameplayStates.GameStart => "Game Start",
                GameplayStates.TurnStart => "Turn Start",
                GameplayStates.PlayerTurn => "Player Turn",
                GameplayStates.PlayerExit => "Player Exit",
                GameplayStates.Detonation => "Detonation",
                GameplayStates.EnemyTurn => "Enemy Turn",
                GameplayStates.GameEnd => "Game End",
                _ => "Unknown"
            };
        }

        public void UpdateEnemiesLeftText(int enemiesLeft)
        {
            enemiesLeftText.text = $"Enemies Left: {enemiesLeft}";
        }

        public void ShowMenu()
        {
            menuPanel.SetActive(true);
        }

        public void HideMenu()
        {
            menuPanel.SetActive(false);
        }
        
        public void HideUI()
        {
            stageText.gameObject.SetActive(false);
            enemiesLeftText.gameObject.SetActive(false);
            statePlayerTurnText.gameObject.SetActive(false);
            stateEnemyTurnText.gameObject.SetActive(false);
        }

        public void ShowUI()
        {
            stageText.gameObject.SetActive(true);
            enemiesLeftText.gameObject.SetActive(true);
        }
        
        public void ShowStateText(GameplayStates state)
        {
            if (state == GameplayStates.TurnStart || state == GameplayStates.PlayerTurn || state == GameplayStates.PlayerExit)
            {
                statePlayerTurnText.gameObject.SetActive(true);
                stateEnemyTurnText.gameObject.SetActive(false);
            }
            else if (state == GameplayStates.EnemyTurn)
            {
                statePlayerTurnText.gameObject.SetActive(false);
                stateEnemyTurnText.gameObject.SetActive(true);
            }
            else
            {
                statePlayerTurnText.gameObject.SetActive(false);
                stateEnemyTurnText.gameObject.SetActive(false);
            }
        }
    }
}