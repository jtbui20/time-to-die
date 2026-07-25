using System;
using DefaultNamespace.VFX;
using UnityEngine;
using UnityEngine.Playables;

namespace DefaultNamespace.CustomAnimations
{

    public enum ICustomAnimationTimeType
    {
        fill,
        fix,
    }
    
    public interface ICustomAnimation
    {
        public ICustomAnimationTimeType timeType { get; }
        public void SetStartTime(double time);
        public bool IsComplete { get; }
        public void ComputePosition(double currentTime);
        public void SetTotalDuration(double totalDuration);
        public event Action OnComplete;
    }
    
    [Serializable]
    public class AnimBombMove : ICustomAnimation
    {
        public AnimBombMoveConfig config;
        public Vector3 originalPosition;
        public Vector3 targetPosition;

        public double startTime;
        public double totalDuration;
        
        public GameObject obj;

        public event Action OnComplete;

        public AnimBombMove(GameObject obj, Vector3 targetPosition, double totalDuration,
            AnimBombMoveConfig config)
        {
            this.obj = obj;
            originalPosition = obj.transform.position;
            this.targetPosition = targetPosition;
            this.totalDuration = totalDuration;
            this.config = config;
            IsComplete = false;
        }

        public ICustomAnimationTimeType timeType => ICustomAnimationTimeType.fill;

        public void SetStartTime(double time)
        {
            startTime = time;
        }

        public void SetTotalDuration(double totalDuration)
        {
            this.totalDuration = totalDuration;
        }

        public bool IsComplete { get; private set; }

        public void ComputePosition(double currentTime)
        {
            if (!obj) return;
            if (IsComplete) return;
            
            float normalizedTime = (float)((currentTime - startTime) / totalDuration);
            
            float x = Mathf.Lerp(originalPosition.x, targetPosition.x, config.xzMoveGraph.Evaluate(normalizedTime));
            float z = Mathf.Lerp(originalPosition.y, targetPosition.y, config.xzMoveGraph.Evaluate(normalizedTime));
            
            
            float yMax = Mathf.Max(originalPosition.y, targetPosition.y) + config.MaximumHeight;
            
            float y = Mathf.Lerp(originalPosition.y, yMax, config.yMoveGraph.Evaluate(normalizedTime));
            
            obj.transform.position = new Vector3(x, y, z);
            
            if (normalizedTime >= 1.0f)
            {
                obj.transform.position = targetPosition;
                OnComplete?.Invoke();
                IsComplete = true;
            }
        }
    }

    [Serializable]
    public class AnimBombExplode : ICustomAnimation
    {
        public GameObject obj;

        public double startTime;
        public double totalDuration;

        public void SetTotalDuration(double totalDuration)
        {
            this.totalDuration = totalDuration;
        }

        public event Action OnComplete;

        public bool HasPlayed = false;

        private VFXDispatcher _vfxDispatcher;

        public AnimBombExplode(GameObject obj, VFXDispatcher vfx, double totalDuration, Action onExplode)
        {
            this.obj = obj;
            this.totalDuration = totalDuration;
            this.OnComplete = onExplode;
            this._vfxDispatcher = vfx;
            IsComplete = false;
        }

        public ICustomAnimationTimeType timeType => ICustomAnimationTimeType.fix;

        public void SetStartTime(double time)
        {
            startTime = time;
        }
        
        public bool IsComplete { get; private set; }

        public void ComputePosition(double currentTime)
        {
            if (!obj) return;
            if (IsComplete) return;

            if (!HasPlayed)
            {
                // Play the vfx 
                _vfxDispatcher.RequestVFX(obj.transform, VFXMainTypes.BombExplode);
                HasPlayed = true;
            }
            
            float normalizedTime = (float)((currentTime - startTime) / totalDuration);
            
            if (normalizedTime >= 1.0f)
            {
                OnComplete?.Invoke();
                IsComplete = true;
            }
        }
    }
}