using Eflatun.SceneReference;
using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game Data/Level Data", order = 0)]
    public class LevelScriptableObject : ScriptableObject
    {
        public string LevelName;
        public SceneReference SceneToLoad;
        public int StartingLives;
        
        // Need to add types
        public SpawnerSchedule SpawnerSchedule;
        private Object LevelVariantArrangement;
    }
}