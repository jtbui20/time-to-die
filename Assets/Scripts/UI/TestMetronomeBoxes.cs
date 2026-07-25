using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class TestMetronomeBoxes : MonoBehaviour
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private int numberOfBoxes = 4;
        [SerializeField] private float spacing = 1.0f;
        private List<GameObject> boxes = new List<GameObject>();

        private int count = 0;
        private int max = 4;

        private void Awake()
        {
            FindAnyObjectByType<PlayableSynchronizer>().OnBPMTick += OnTick;
        }
        
        private void Start()
        {
            for (int i = 0; i < numberOfBoxes; i++)
            {
                GameObject box = Instantiate(boxPrefab, transform);
                box.transform.localPosition = new Vector3(i * spacing, 0, 0);
                boxes.Add(box);
            }
        }

        private void OnTick()
        {
            
            
            count++;
            if (count >= max) count = 0;

            for (int i = 0; i < numberOfBoxes; i++)
            {
                boxes[i].SetActive(i == count);
            }
        }
    }
}