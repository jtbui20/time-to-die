using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrayBomb : MonoBehaviour
{
    public GameObject actualBomb;

    public LayerMask PlaceableLayers;
    public Vector3 verticalOffset;

    private bool FollowingMouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowingMouse)
        {
            // Lerp to the mouse position in world space that is cast onto "surface" layer mask
            // From camera screen to world position
            
            Vector2 mousePosition = InputSystem.actions["MousePosition"].ReadValue<Vector2>();
            
            // Raycast down to hit the surface layer
            RaycastHit hit = Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity, PlaceableLayers) ? hit : default;
            if (hit.collider != null)
            {
                transform.position = hit.point + verticalOffset;
            }
        }
    }


    private void OnMouseDown()
    {
        Debug.Log("clicked");
        // Start to follow the mouse
        FollowingMouse = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("clicked");
        FollowingMouse = false;
    }
}
