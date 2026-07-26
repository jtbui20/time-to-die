using System;
using DefaultNamespace.VFX;
using Unity.VisualScripting;
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
        public void InjectVFX(VFXDispatcher vfx);
    }
    
    [Serializable]
    public class AnimBombMove : ICustomAnimation
    {
        public AnimBombMoveConfig config;
        public Vector3 originalPosition;
        public Vector3 targetPosition;

        public double startTime;
        public double totalDuration;
        
        public FreeBomb obj;

        public event Action OnComplete;

        public AnimBombMove(FreeBomb obj, Vector3 targetPosition, double totalDuration,
            AnimBombMoveConfig config)
        {
            this.obj = obj;
            originalPosition = obj.Position;
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

        public bool debuged = true;

        public void ComputePosition(double currentTime)
        {
            if (obj == null) return;
            if (IsComplete) return;
            
            float normalizedTime = (float)((currentTime - startTime) / totalDuration);
            if (debuged)
            {
                debuged = false;
            }
            
            float x = Mathf.Lerp(originalPosition.x, targetPosition.x, config.xzMoveGraph.Evaluate(normalizedTime));
            float z = Mathf.Lerp(originalPosition.z, targetPosition.z, config.xzMoveGraph.Evaluate(normalizedTime));
            
            
            float yMax = Mathf.Max(originalPosition.y, targetPosition.y) + config.MaximumHeight;
            
            float y = Mathf.Lerp(originalPosition.y, yMax, config.yMoveGraph.Evaluate(normalizedTime));
            
            obj.Position = new Vector3(x, y, z);
            
            if (normalizedTime >= 1.0f)
            {
                obj.Position = targetPosition;
                OnComplete?.Invoke();
                IsComplete = true;
            }
        }

        public void InjectVFX(VFXDispatcher vfx)
        {
            return;
        }
    }

    [Serializable]
    public class AnimBombExplode : ICustomAnimation
    {
        public FreeBomb obj;

        public double startTime;
        public double totalDuration;

        public void SetTotalDuration(double totalDuration)
        {
            this.totalDuration = totalDuration;
        }

        public event Action OnComplete;

        public bool HasPlayed = false;

        private VFXDispatcher _vfxDispatcher;

        public AnimBombExplode(FreeBomb obj, double totalDuration)
        {
            this.obj = obj;
            this.totalDuration = totalDuration;
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
            if (obj == null) return;
            if (IsComplete) return;

            if (!HasPlayed)
            {
                // Play the vfx 
                _vfxDispatcher.RequestVFX(obj.Position, VFXMainTypes.BombExplode);
                HasPlayed = true;
            }
            
            float normalizedTime = (float)((currentTime - startTime) / totalDuration);
            
            if (normalizedTime >= 1.0f)
            {
                OnComplete?.Invoke();
                IsComplete = true;
            }
        }
        
        public void InjectVFX(VFXDispatcher vfx)
        {
            _vfxDispatcher = vfx;
        }
    }
}