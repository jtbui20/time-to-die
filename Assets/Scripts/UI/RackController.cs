using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.UI
{
    public class RackController : MonoBehaviour
    {
        // Stuff it needs to do:
        // Make itself a child of the camera
        // Also track physics
        // These "bombs" are fake
        [Header("Tray Bomb Behaviours")]
        public Vector3 fakeGravity = new Vector3(-10f, 0, 0);
        public Vector3 verticalOffset = new Vector3(0,1,0);
        
        /// <summary>
        /// This is where we can drop and spawn
        /// </summary>
        public LayerMask PlaceableLayers;
        /// <summary>
        /// This is where we can also move above
        /// </summary>
        public LayerMask OverlayLayers;
        
        public List<Transform> bombPositionsOnTray;
        public GameObject ballKillerZone;
        
        [Header("Animation Configuration")]
        public float TimeDelayBetweenBombs = 0.2f;

        [Header("Prefabs")] public GameObject trayBombPrefab;
        
        
        private bool FollowingMouse;
        private TrayBomb currentBomb;
        private List<TrayBomb> bombsOnRack;
        

        public event Action<FreeBomb, Vector3> OnValidBombSpawnPosition;


        public async Awaitable LoadInNewBombs(List<FreeBomb> bombs)
        {
            foreach (FreeBomb bomb_src in bombs)
            {
                RegisterBomb(bomb_src);
                await Awaitable.WaitForSecondsAsync(TimeDelayBetweenBombs);
            }
        }
        
        public async Awaitable RemoveBomb(List<FreeBomb> bombs)
        {
            // Sort the bombs somhow
            List<TrayBomb> bombsToRemove = new List<TrayBomb>();
            foreach (TrayBomb bomb in bombsOnRack)
            {
                if (bombs.Contains(bomb.actualBomb))
                {
                    bombsToRemove.Add(bomb);
                }
            }
            foreach (TrayBomb bomb in bombsToRemove)
            {
                UnregisterBomb(bomb);
                await Awaitable.WaitForSecondsAsync(TimeDelayBetweenBombs);
            }
        }

        /// <summary>
        /// This is used to "logically" remove the bomb
        /// </summary>
        /// <param name="bomb"></param>
        private void UnregisterBomb(TrayBomb bomb)
        {
            bombsOnRack.Remove(bomb);
            bomb.OnMouseDownEvent -= OnBombMouseDown;
            bomb.OnMouseUpEvent -= OnBombMouseUp;
        }

        private void Awake()
        {
            bombsOnRack = new List<TrayBomb>();
            GetAnyExistingBombsOnChild();
        }

        private void GetAnyExistingBombsOnChild()
        {
            var children = GetComponentsInChildren<TrayBomb>();
            bombsOnRack.AddRange(children);
            foreach (TrayBomb bomb in bombsOnRack)
            {
                BindEvents(bomb);
            }
        }

        private void FixedUpdate()
        {
            FakeGravity();
        }

        private void Update()
        {
            if (currentBomb)
            {
                MoveCurrentBomb();
            }
        }

        private void FakeGravity()
        {
            // keep applying a constant force to the left
            foreach (var bomb in bombsOnRack)
            {
                bomb.rigidBody.AddForce(fakeGravity, ForceMode.Acceleration);
            }
        }

        private void MoveCurrentBomb()
        {
            RaycastHit hit = GetViableLocationForBomb();
            if (hit.collider != null)
            {
                currentBomb.transform.DOMove(hit.point + verticalOffset, 0.3f).SetEase(Ease.OutSine);
            }
        }

        private void OnBombMouseDown(TrayBomb bomb)
        {
            Debug.Log("clicked");
            bomb.rigidBody.isKinematic = true;
            bomb.collider.excludeLayers = OverlayLayers;
            currentBomb = bomb;
            Debug.Log($"Bomb {bomb.actualBomb} selected");
        }

        private void OnBombMouseUp(TrayBomb bomb)
        {
            Debug.Log("released");
            
            // Check if it's a valid spot
            RaycastHit hit = GetViableLocationForBomb();
            if (hit.collider != null)
            {
                Debug.Log($"Bomb {currentBomb.actualBomb} placed at {hit.point}");
                // Unmanage this bomb from the tray
                UnregisterBomb(currentBomb);
                OnValidBombSpawnPosition?.Invoke(currentBomb.actualBomb, hit.point);
                Destroy(currentBomb.gameObject);
            }
            else
            {
                bomb.rigidBody.isKinematic = false;
                bomb.collider.excludeLayers = 0;
                // Return this bomb to the end of the tray position
                if (bombPositionsOnTray.Count > 0)
                {
                    currentBomb.transform.DOMove(GetLastPositionOnTray(), 0.5f);
                }
            }
            
            currentBomb = null;
        }

        private RaycastHit GetViableLocationForBomb()
        {
            Vector2 mousePosition = InputSystem.actions["MousePosition"].ReadValue<Vector2>();
            RaycastHit hit = Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity, PlaceableLayers) ? hit : default;
            return hit;
        }

        private Vector3 GetLastPositionOnTray()
        {
            return bombPositionsOnTray[bombPositionsOnTray.Count - 1].position;
        }

        private void RegisterBomb(FreeBomb bomb)
        {
            var TrayBomb = Instantiate(trayBombPrefab, GetLastPositionOnTray(),  Quaternion.identity, this.transform)
                .GetComponent<TrayBomb>();
            TrayBomb.actualBomb = bomb;
            BindEvents(TrayBomb);
            bombsOnRack.Add(TrayBomb);
        }

        private void BindEvents(TrayBomb bomb)
        {
            bomb.OnMouseDownEvent += OnBombMouseDown;
            bomb.OnMouseUpEvent += OnBombMouseUp;
        }
    }
}