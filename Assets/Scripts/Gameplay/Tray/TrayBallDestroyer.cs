using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class TrayBallDestroyer : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<TrayBomb>() != null)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}