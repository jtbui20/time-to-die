using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class BombBagView : MonoBehaviour
    {
        public GameObject bombItemPrefab;

        public GameObject content;

        public void LoadBombs(List<BombDefinition> bombs)
        {
            foreach (BombDefinition bomb in bombs)
            {
                GameObject bombItem = Instantiate(bombItemPrefab, content.transform);
                BombItemInventoryView bombItemView = bombItem.GetComponent<BombItemInventoryView>();
                bombItemView.SetBombTarget(bomb);
            }
        }
    }
}