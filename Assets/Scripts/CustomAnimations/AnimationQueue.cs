using System;
using System.Collections.Generic;
using DefaultNamespace.VFX;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace.CustomAnimations
{
    public class AnimationQueue : MonoBehaviour
    {
        public List<ICustomAnimation> animationQueue = new();
        private ICustomAnimation currentAnimation;

        public VFXDispatcher _vfx;
        public ICustomAnimation getCurrentAnimation => currentAnimation;
        private Func<double> AskForDuration;

        public BPMActionSynchronizer synchronizer;

        public AnimBombMoveConfig bombMoveConfig;

        public UniTask GetCompletionToken()
        {
            return UniTask.WaitUntil(() =>
                animationQueue.Count == 0 && currentAnimation == null
                );
        }

        private void Awake()
        {
            _vfx = FindAnyObjectByType<VFXDispatcher>();
            synchronizer = FindAnyObjectByType<BPMActionSynchronizer>();
        }

        private void Start()
        {
            synchronizer.OnBeatTick += LoadIfEmpty;
            Setup(() => synchronizer.ProvideNextDurationUntilTick());
        }

        public void Setup(Func<double> askForDuration)
        {
            this.AskForDuration = askForDuration;

            Camera mainCamera = Camera.main;

            if (mainCamera)
            {
                mainCamera.gameObject.AddComponent<CameraReference>();
            }
        }

        private void Update()
        {
            ProcessCurrentAnimation();
        }

        public void Enqueue(ICustomAnimation customAnimation)
        {
            customAnimation.InjectVFX(_vfx);
            animationQueue.Add(customAnimation);
        }

        public void EnqueueHead(ICustomAnimation customAnimation)
        {
            customAnimation.InjectVFX(_vfx);
            animationQueue.Insert(0, customAnimation);
        }
        
        public void Dequeue(ICustomAnimation customAnimation)
        {
            if (currentAnimation == customAnimation)
            {
                CleanupAnimation();
            }
            else
            {
                animationQueue.Remove(customAnimation);
            }
        }
        
        // Very VERY jank

        public bool RemoveDuplicateExplode(FreeBomb gameObject)
        {
            foreach (var anim in animationQueue)
            {
                if (anim is AnimBombExplode explodeAnim && explodeAnim.obj == gameObject)
                {
                    animationQueue.Remove(anim);
                    return true;
                }
            }

            return false;
        }

        public void LoadIfEmpty()
        {
            if (currentAnimation == null && animationQueue.Count > 0)
            {
                currentAnimation = animationQueue[0];
                animationQueue.RemoveAt(0);
                currentAnimation.SetStartTime(Time.timeAsDouble);
                currentAnimation.OnComplete += CleanupAnimation;
                if (currentAnimation.timeType == ICustomAnimationTimeType.fill)
                {
                    currentAnimation.SetTotalDuration(AskForDuration());
                }
            }
        }

        private void ProcessCurrentAnimation()
        {
            if (currentAnimation == null) return;
            currentAnimation.ComputePosition(Time.timeAsDouble);
        }

        private void CleanupAnimation()
        {
            currentAnimation.OnComplete -= CleanupAnimation;
            currentAnimation = null;
        }
    }
}