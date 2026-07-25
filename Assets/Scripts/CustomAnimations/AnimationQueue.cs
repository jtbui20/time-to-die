using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.CustomAnimations
{
    public class AnimationQueue
    {
        public List<ICustomAnimation> animationQueue = new();
        private ICustomAnimation currentAnimation;
        
        public ICustomAnimation getCurrentAnimation => currentAnimation;
        private Func<double> AskForDuration;

        public void Setup(Func<double> askForDuration)
        {
            this.AskForDuration = askForDuration;
        }

        public void Update()
        {
            ProcessCurrentAnimation();
        }

        public void Enqueue(ICustomAnimation customAnimation)
        {
            animationQueue.Add(customAnimation);
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