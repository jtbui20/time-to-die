using UnityEngine;

namespace DefaultNamespace.CustomAnimations
{
    [CreateAssetMenu(fileName = "AnimBombMoveConfig", menuName = "Custom Animations/AnimBombMoveConfig", order = 0)]
    public class AnimBombMoveConfig : ScriptableObject
    {
        public AnimationCurve xzMoveGraph;
        public AnimationCurve yMoveGraph;
        public float MaximumHeight;
    }
}