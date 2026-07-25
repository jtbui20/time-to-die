using System;
using UnityEngine;

public class TrayBomb : MonoBehaviour
{
    public FreeBomb actualBomb;
    public Rigidbody rigidBody;
    public Collider collider;
    
    public event Action<TrayBomb> OnMouseDownEvent;
    public event Action<TrayBomb> OnMouseUpEvent;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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
