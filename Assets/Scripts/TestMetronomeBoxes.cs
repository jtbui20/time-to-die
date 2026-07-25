using System;
using System.Collections.Generic;
using DefaultNamespace.CustomAnimations;
using DefaultNamespace.VFX;
using Synchronizers;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class TestMetronomeBoxes : MonoBehaviour
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private int numberOfBoxes = 4;
        [SerializeField] private float spacing = 1.0f;
        private List<GameObject> boxes = new List<GameObject>();

        [Header("anim test")] public GameObject bombReference;
        public GameObject bombReference2;
        public Transform targetPosition;

        public AnimBombMoveConfig bombMoveConfig;
        public AnimationQueue animationQueue = new();
        
        public TimelineActionSynchronizer synchronizer;
        private VFXDispatcher _vfx;

        private int count = 0;
        private int max = 4;

        private void Awake()
        {
            
            _vfx = FindAnyObjectByType<VFXDispatcher>();
        }
        
        private void Start()
        {
            for (int i = 0; i < numberOfBoxes; i++)
            {
                GameObject box = Instantiate(boxPrefab, transform);
                box.transform.localPosition = new Vector3(i * spacing, 0, 0);
                boxes.Add(box);
            }
            
            SetupAnims();
            animationQueue.Setup(() => synchronizer.ProvideNextDurationUntilTick());
            
            synchronizer.OnBeatTick += OnTick;
            synchronizer.PlayAsset();
        }

        private void SetupAnims()
        {
            var thing = new AnimBombMove(
                bombReference, targetPosition.position, 1.0, bombMoveConfig);
            var bombExplode = new AnimBombExplode(bombReference, _vfx, 0.2f);
            var thing2 = new AnimBombMove(
                bombReference2, targetPosition.position, 1.0, bombMoveConfig);
            var bombExplode2 = new AnimBombExplode(bombReference2, _vfx, 0.2f);

            thing.OnComplete += () =>
            {
                animationQueue.Enqueue(bombExplode);
            };
            
            bombExplode.OnComplete += () =>
            {
                animationQueue.Enqueue(thing2);
            };

            thing2.OnComplete += () =>
            {
                animationQueue.Enqueue(bombExplode2);
            };
            
            bombExplode2.OnComplete += () =>
            {
                Debug.Log("Done");
            };
            
            animationQueue.Enqueue(thing);
        }

        private void Update()
        {
            animationQueue.Update();
        }

        private void OnTick()
        {
            count++;
            if (count >= max) count = 0;

            for (int i = 0; i < numberOfBoxes; i++)
            {
                boxes[i].SetActive(i == count);
            }

            animationQueue.LoadIfEmpty();
        }
    }
}