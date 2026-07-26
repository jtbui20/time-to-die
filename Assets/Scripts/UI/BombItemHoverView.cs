using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.UI
{
    public class BombItemHoverView : MonoBehaviour
    {
        [SerializeField] private LayerMask bombLayers;
        private FreeBomb referenceBomb;
        
        public TextMeshProUGUI bombNameText;
        public TextMeshProUGUI bombDescriptionText;

        public TextMeshProUGUI bombDamage;
        public TextMeshProUGUI bombTimeLeft;
        public TextMeshProUGUI bombRange;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
            ClearView();
        }

        private void Update()
        {
            UpdateHoverTarget();
        }

        private void UpdateHoverTarget()
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, bombLayers))
            {
                IBombReference bombRef = hit.collider.GetComponentInParent<IBombReference>();

                if (bombRef != null && bombRef.Bomb != referenceBomb)
                {
                    SetBombTarget(bombRef.Bomb);
                }
            }
            else if (referenceBomb != null)
            {
                referenceBomb = null;
                ClearView();
            }
        }

        public void SetBombTarget(FreeBomb bomb)
        {
            if (referenceBomb != null) 
            { 
                referenceBomb.OnStatusChanged -= UpdateView;
                referenceBomb.OnCleanup -= DereferenceBomb;
            }

            referenceBomb = bomb;

            if (referenceBomb != null) 
            { 
                referenceBomb.OnStatusChanged += UpdateView;
                referenceBomb.OnCleanup += DereferenceBomb;
            }

            UpdateView();
        }

        void UpdateView()
        {
            if (referenceBomb == null) { return; }

            string BombName = BombHelpers.BombTypeToString(referenceBomb.BombType);
            bombNameText.text = BombName;
            bombDescriptionText.text = referenceBomb.Description;
            
            bombDamage.text = $"{referenceBomb.Damage.ToString()} (+{referenceBomb.CurrentDamageMult * referenceBomb.Damage})";
            bombTimeLeft.text = referenceBomb.Health.ToString();
            bombRange.text = referenceBomb.Range.ToString();
        }

        private void ClearView()
        {
            bombNameText.text = "Bomb";
            bombDescriptionText.text = "";

            bombDamage.text = "";
            bombTimeLeft.text = "";
            bombRange.text = "";
        }

        private void DereferenceBomb()
        {
            if (referenceBomb != null) { referenceBomb.OnStatusChanged -= UpdateView; }
        }
    }
}