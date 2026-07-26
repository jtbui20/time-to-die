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

        public CameraReference cam;

        public Vector3 startingPosition;

        private bool HasPlayed = false;
        public AnimBombMove(FreeBomb obj, Vector3 targetPosition, double totalDuration,
            AnimBombMoveConfig config)
        {
            this.obj = obj;
            originalPosition = obj.Position;
            this.targetPosition = targetPosition;
            this.totalDuration = totalDuration;
            this.config = config;
            IsComplete = false;
            cam = Camera.main.gameObject.GetComponent<CameraReference>();
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

            if (!HasPlayed)
            {
                startingPosition = cam.MoveToSafeStartingPosition(obj.Position);
                HasPlayed = true;
            }
            
            float x = Mathf.Lerp(originalPosition.x, targetPosition.x, config.xzMoveGraph.Evaluate(normalizedTime));
            float z = Mathf.Lerp(originalPosition.z, targetPosition.z, config.xzMoveGraph.Evaluate(normalizedTime));
            
            
            float yMax = Mathf.Max(originalPosition.y, targetPosition.y) + config.MaximumHeight;
            
            float y = Mathf.Lerp(originalPosition.y, yMax, config.yMoveGraph.Evaluate(normalizedTime));
            
            obj.Position = new Vector3(x, y, z);
            
            // cam.ZoomToTarget(obj.Position, config.CameraZoomGraph.Evaluate(normalizedTime), false);
            cam.FixedFollowDistance(obj.Position, 8f);
            
            if (normalizedTime >= 1.0f)
            {
                // cam.Reset();
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
        private CameraReference cam;

        public AnimBombExplode(FreeBomb obj, double totalDuration)
        {
            this.obj = obj;
            this.totalDuration = totalDuration;
            IsComplete = false;
            cam = Camera.main.gameObject.GetComponent<CameraReference>();
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
                // cam.ZoomToTarget(obj.Position, 0.6f);
                cam.FixedFollowDistance(obj.Position, 12f);
            }
            
            float normalizedTime = (float)((currentTime - startTime) / totalDuration);
            
            if (normalizedTime >= 1.0f)
            {
                OnComplete?.Invoke();
                // cam.Reset();
                IsComplete = true;
            }
        }
        
        public void InjectVFX(VFXDispatcher vfx)
        {
            _vfxDispatcher = vfx;
        }
    }
}