using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "Loadout", menuName = "Data/Loadout", order = 0)]
    public class LoadoutScriptableObject : ScriptableObject
    {
        public string Name;
        public List<BombDefinition> bombsInLoadout;
        public string Description;
    }
}