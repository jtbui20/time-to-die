using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IActionSynchronizer
    {
        public double ProvideNextDurationUntilTick();
        public event Action OnBeatTick;
    }
    
    public class BPMActionSynchronizer : MonoBehaviour, IActionSynchronizer
    {
        public double bpm = 95F;
        private double nextTick = 0.0F;
        private double samplesPerBeat = 0.0;
        private bool running;

        public double rate => 60.0f / bpm;

        public double ProvideNextDurationUntilTick()
        {
            return rate;
        }
        
        public double deadzone = 0.01F;

        public event Action OnBeatTick;
        private bool usingDSP = false;

        public void StartTimer()
        {
            if (usingDSP) StartDSPTimer();
            else StartFrameTimer();
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
            nextTick = Time.timeAsDouble + rate;
            running = true;
        }

        private void Update()
        {
            if (!running) return;
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
            OnBeatTick?.Invoke();
        }
    }
}