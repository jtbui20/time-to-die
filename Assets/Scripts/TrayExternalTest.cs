using System.Collections;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class TrayExternalTest : MonoBehaviour
    {
        public RackController rack;
        public BombDefinition bombDefinition;
        void Start()
        {
            StartCoroutine(IntervalSpawn());
        }


        async void OnValidPositionFound(FreeBomb bomb, Vector3 position)
        {
            // Tell the bomb spawner
        }

        public IEnumerator IntervalSpawn()
        {
            var bomb = new FreeBomb(bombDefinition);
            yield return rack.LoadInNewBombs(new System.Collections.Generic.List<FreeBomb> { bomb });
            yield return new WaitForSeconds(5f);
            StartCoroutine(IntervalSpawn());
        }
    }
}