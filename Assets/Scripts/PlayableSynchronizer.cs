using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace.CustomAnimations;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace DefaultNamespace
{
    /// <summary>
    /// The synchronizer provides time durations and optionally provides the time when things can execute
    /// 
    /// </summary>
    public class PlayableSynchronizer : MonoBehaviour
    {
        public double bpm = 95.0F;

        private double nextTick = 0.0F;
        private bool ticked = false;

        private bool running;

        private double rate => 60.0f / bpm;

        public GameObject bombReference;
        public Transform targetPosition;
        public AnimBombMoveConfig bombMoveConfig;
        
        public AnimBombMovePlayable thing;

        [Range(0, 1)] public float slider;

        private void Start()
        {
            thing = new AnimBombMovePlayable(
                bombReference, targetPosition.position, 1.0, bombMoveConfig);
        }

        private void Update()
        {
            thing.ComputePosition(slider);
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (!running) return;
            
        }

        void DoTick()
        {
            // Check if there is an action available to execute 
        }

        private UniTask RequestWithTime(Action action, float duration)
        {
            return new UniTask();
        }

        private UniTask RequestGetTime(Action<float> action)
        {
            // Gets the current time, see's how long it can go for 
            return new UniTask();
        }
    }
}