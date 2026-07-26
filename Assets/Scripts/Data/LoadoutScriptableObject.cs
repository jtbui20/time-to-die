using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "Loadout", menuName = "Data/Loadout", order = 0)]
    public class LoadoutScriptableObject : ScriptableObject
    {
        public string LoadoutName;
        public List<BombDefinition> bombsInLoadout;
        public string Description;
    }
}