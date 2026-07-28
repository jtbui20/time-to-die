using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class BombItemInventoryView : MonoBehaviour
    {
        private BombDefinition referenceBomb;

        public TextMeshProUGUI bombNameText;
        public Image bombImage;
        
        public BombSpriteBindings bombSpriteBindings;

        public TextMeshProUGUI bombDamage;
        public TextMeshProUGUI bombTimeLeft;
        public TextMeshProUGUI bombRange;

        public void SetBombTarget(BombDefinition bomb)
        {
            referenceBomb = bomb;
            UpdateView();
        }

        void UpdateView()
        {
            string BombName = BombHelpers.BombTypeToString(referenceBomb.BombType);
            bombNameText.text = BombName;
            
            bombImage.sprite = bombSpriteBindings.GetPrefab(referenceBomb.BombType);
            
            bombDamage.text = referenceBomb.Damage.ToString();
            bombTimeLeft.text = referenceBomb.Health.ToString();
            bombRange.text = referenceBomb.Range.ToString();
        }
    }
}