using System;
using UnityEngine;
using UnityEngine.Playables;

namespace DefaultNamespace.CustomAnimations
{
    public class AnimBombMovePlayable
    {
        public AnimBombMoveConfig config;
        public Vector3 originalPosition;
        public Vector3 targetPosition;

        public double totalDuration;
        
        public GameObject obj;

        public event Action OnComplete;
        

        public AnimBombMovePlayable(GameObject obj, Vector3 targetPosition, double totalDuration,
            AnimBombMoveConfig config)
        {
            this.obj = obj;
            this.originalPosition = obj.transform.position;
            this.targetPosition = targetPosition;
            this.totalDuration = totalDuration;
            this.config = config;
            isComplete = false;
        }

        public bool isComplete { get; private set; }

        public void ComputePosition(double time)
        {
            if (!obj) return;
            if (isComplete) return;
            
            float normalizedTime = (float)(time / totalDuration);
            
            float x = Mathf.Lerp(originalPosition.x, targetPosition.x, config.xzMoveGraph.Evaluate(normalizedTime));
            float z = Mathf.Lerp(originalPosition.y, targetPosition.y, config.xzMoveGraph.Evaluate(normalizedTime));
            
            
            float yMax = Mathf.Max(originalPosition.y, targetPosition.y) + config.MaximumHeight;
            
            float y = Mathf.Lerp(originalPosition.y, yMax, config.yMoveGraph.Evaluate(normalizedTime));
            
            obj.transform.position = new Vector3(x, y, z);
            
            if (normalizedTime >= 1.0f)
            {
                obj.transform.position = targetPosition;
                OnComplete?.Invoke();
                isComplete = true;
            }
        }
    }
}