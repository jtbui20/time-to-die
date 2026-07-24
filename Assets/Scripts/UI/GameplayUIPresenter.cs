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

        [Header("Turn State Texts")]
        public TextMeshProUGUI statePlayerTurnText;
        public TextMeshProUGUI stateEnemyTurnText;

        public event Action OnEndTurnButtonPressed;

        private Animator UIAnimator;

        private void Awake()
        {
            UIAnimator = GetComponent<Animator>();
        }

        void Start()
        {
            Debug.Log("bound");
            endTurnButton.onClick.AddListener(() => OnEndTurnButtonPressed?.Invoke());
        }
        
        public void UpdateStageText(GameplayStates stage)
        {
            stageText.text = stage.ToString();
        }

        public void UpdateEnemiesLeftText(int enemiesLeft)
        {
            enemiesLeftText.text = $"Enemies Left: {enemiesLeft}";
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