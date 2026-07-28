using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Synchronizers
{
    [RequireComponent(typeof(PlayableDirector))]
    [RequireComponent(typeof(SignalReceiver))]
    public class TimelineActionSynchronizer : MonoBehaviour, IActionSynchronizer
    {
        private PlayableDirector playableDirector;
        
        [SerializeField]
        private SignalAsset intervalSignalReference;
        
        List<double> SignalDurations = new();
        public double deadZone = 0.01F;
        public event Action OnBeatTick;

        private int signalIndex = 0;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        public void GetAllSignalDurations()
        {
            List<SignalEmitter> signalEmitters = new List<SignalEmitter>();
            TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;

            foreach (TrackAsset trackAsset in timelineAsset.GetRootTracks())
            {
                FindSignalsInTrackRecursive(trackAsset, intervalSignalReference, signalEmitters);
            }
            
            // Sort the signals
           signalEmitters.Sort((a, b) => a.time.CompareTo(b.time));

            SignalDurations = new();
            for (int i = 0; i < signalEmitters.Count - 1; i++)
            {
                double futureTime = signalEmitters[i + 1].time;
                double currentTime = signalEmitters[i].time;
                float duration = (float)(futureTime - currentTime);
                SignalDurations.Add(duration - deadZone);
            }
        }
        
        private static void FindSignalsInTrackRecursive(TrackAsset track, SignalAsset targetSignal, List<SignalEmitter> foundSignals)
        {
            if (track == null) return;

            // 1. Scan markers attached directly to this track
            foreach (IMarker marker in track.GetMarkers())
            {
                if (marker is SignalEmitter emitter)
                {
                    foundSignals.Add(emitter);
                }
            }

            // 2. Recursively scan sub-tracks if this is a Group Track
            foreach (TrackAsset subTrack in track.GetChildTracks())
            {
                FindSignalsInTrackRecursive(subTrack, targetSignal, foundSignals);
            }
        }

        public void PlayAsset(TimelineAsset timelineAsset = null)
        {
            if (timelineAsset != null)
            {
                playableDirector.playableAsset = timelineAsset;
            }

            if (playableDirector.playableAsset == null)
            {
                Debug.LogError("PlayableDirector has no TimelineAsset assigned.");
                return;
            }
            
            signalIndex = 0;
            GetAllSignalDurations();
            playableDirector.Play();
        }

        // Unity invoked event
        public void OnSignalReceived()
        {
            OnBeatTick?.Invoke();
            signalIndex++;
        }

        public double ProvideNextDurationUntilTick()
        {
            if (SignalDurations.Count <= 1) return 0.0;
            return SignalDurations[signalIndex];
        }
    }
}