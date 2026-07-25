using System;
using System.Collections.Generic;
using DefaultNamespace.CustomAnimations;
using DefaultNamespace.VFX;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// The synchronizer provides time durations and optionally provides the time when things can execute
    /// 
    /// </summary>
    public class PlayableSynchronizer : MonoBehaviour
    {
        public double bpm = 95F;
        private double nextTick = 0.0F;
        private double samplesPerBeat = 0.0;
        private bool tickedThisBeat = false;

        private bool running;

        private double rate => 60.0f / bpm;

        public double deadzone = 0.01F;

        public GameObject bombReference;
        public GameObject bombReference2;
        public Transform targetPosition;
        
        public AnimBombMoveConfig bombMoveConfig;
        
        public List<ICustomAnimation> animationQueue = new List<ICustomAnimation>();
        public ICustomAnimation currentAnimation;

        private VFXDispatcher _vfx;
        private bool usingDSP = false;

        public event Action OnBPMTick;
        

        private void Start()
        {
            _vfx = GetComponent<VFXDispatcher>();
            var thing = new AnimBombMove(
                bombReference, targetPosition.position, 1.0, bombMoveConfig);
            var bombExplode = new AnimBombExplode(bombReference, _vfx, 0.2f, () =>
            {
                Debug.Log("Bomb exploded!");
            });
            var thing2 = new AnimBombMove(
                bombReference2, targetPosition.position, 1.0, bombMoveConfig);
            
            var bombExplode2 = new AnimBombExplode(bombReference2, _vfx, 0.2f, () =>
            {
                Debug.Log("Bomb exploded 2!");
            });
            animationQueue.Add(thing);
            animationQueue.Add(bombExplode);
            animationQueue.Add(thing2);
            animationQueue.Add(bombExplode2);
            
            usingDSP = GetComponent<AudioSource>() != null;
            
            StartTimer();
        }

        public void StartTimer()
        {
            if (usingDSP)
            {
                StartDSPTimer();
            }
            else
            {
                StartFrameTimer();
            }
        }

        private void StartDSPTimer()
        {
            double sampleRate = AudioSettings.outputSampleRate;
            samplesPerBeat = rate * sampleRate;
            nextTick = AudioSettings.dspTime * sampleRate + samplesPerBeat;
            running = true;
        }

        private void StartFrameTimer()
        {
            // Get the current time,
            nextTick = Time.timeAsDouble + rate;
            running = true;
        }

        private void Update()
        {
            if (!running) return;
            ProcessCurrentAnimation();
            if (usingDSP) return;

            double currentTime = Time.timeAsDouble;
            if (currentTime >= nextTick)
            {
                OnTick();
                nextTick += rate;
            }
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (!running) return;
            if (!usingDSP) return;

            double currentSample = AudioSettings.dspTime * AudioSettings.outputSampleRate;
            int dataLen = data.Length / channels;

            for (int i = 0; i < dataLen; i++)
            {
                if (currentSample + i >= nextTick)
                {
                    OnTick();
                    nextTick += samplesPerBeat;
                }
            }
        }

        private void OnTick()
        {
            LoadIfEmpty();
            OnBPMTick?.Invoke();
        }

        private void LoadIfEmpty()
        {
            if (currentAnimation == null && animationQueue.Count > 0)
            {
                currentAnimation = animationQueue[0];
                animationQueue.RemoveAt(0);
                currentAnimation.SetStartTime(Time.timeAsDouble);
                currentAnimation.OnComplete += OnAnimationComplete;
                if (currentAnimation.timeType == ICustomAnimationTimeType.fill)
                {
                    currentAnimation.SetTotalDuration(rate);
                }
            }
        }

        private void ProcessCurrentAnimation()
        {
            if (currentAnimation == null) return;
            currentAnimation.ComputePosition(Time.timeAsDouble);
        }

        private void OnAnimationComplete()
        {
            currentAnimation.OnComplete -= OnAnimationComplete;
            currentAnimation = null;
        }
    }
}