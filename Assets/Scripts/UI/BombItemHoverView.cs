using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class BombItemHoverView : MonoBehaviour
    {
        private FreeBomb referenceBomb;
        
        public TextMeshProUGUI bombNameText;
        public TextMeshProUGUI bombDescriptionText;

        public TextMeshProUGUI bombDamage;
        public TextMeshProUGUI bombTimeLeft;
        public TextMeshProUGUI bombRange;

        public void SetBombTarget(FreeBomb bomb)
        {
            referenceBomb = bomb;
            UpdateView();
        }

        void UpdateView()
        {
            string BombName = BombHelpers.BombTypeToString(referenceBomb.BombType);
            bombNameText.text = BombName;
            bombDescriptionText.text = referenceBomb.Description;
            
            bombDamage.text = referenceBomb.Damage.ToString();
            bombTimeLeft.text = referenceBomb.Health.ToString();
            bombRange.text = referenceBomb.Range.ToString();
        }
    }
}