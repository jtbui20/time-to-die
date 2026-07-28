using System;
using UnityEngine;

public class TrayBomb : MonoBehaviour, IBombReference
{
    public FreeBomb actualBomb;
    public Rigidbody rigidBody;
    public Collider collider;

    public Vector3 BombOffset = new Vector3(0f, -0.312f, 0f);
    public Vector3 BombScale = new Vector3(0.246f, 0.246f, 0.246f);

    public GameObject dropShadow;
    public Vector3 dropShadowOffset = new Vector3(0f, +0.312f, 0f);

        public bool isLifted = false;
        
    
    private GameObject insideReference;
    
    public event Action<TrayBomb> OnMouseDownEvent;
    public event Action<TrayBomb> OnMouseUpEvent;
    public FreeBomb Bomb => actualBomb;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        SpawnInBombVisual();
    }

    public void Setup(FreeBomb bomb, GameObject inside)
    {
        actualBomb = bomb;
        insideReference = inside;
    }

    private void SpawnInBombVisual()
    {
        if (insideReference == null) return;
        var visual = Instantiate(insideReference, transform);
        // Fix scaling down to 0.246
        visual.transform.localScale = BombScale;
        // Shift -0.312 down
        visual.transform.localPosition = BombOffset;
    }


    public void ShowDropshadow()
    {
        dropShadow.SetActive(true);
    }
    
    public void HideDropshadow()
    {
        dropShadow.SetActive(false);
    }

    public void TryShowDropShadow(Vector3 position)
    {
        dropShadow.transform.position = position + dropShadowOffset;
    }
    

    private void OnMouseDown()
    {
        OnMouseDownEvent?.Invoke(this);
    }

    private void OnMouseUp()
    {
        OnMouseUpEvent?.Invoke(this);
    }
}
