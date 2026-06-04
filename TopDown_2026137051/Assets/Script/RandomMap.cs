using UnityEngine;
using System.Collections.Generic;

public class RandomMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    [SerializeField] private List<GameObject> Map = new List<GameObject>();

    void Start()
    {
        //1/n È®·ü ¼±Á¤
        int chosen = Random.Range(0, items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(i == chosen);
        }
        chosen = Random.Range(0, Map.Count);
        for (int i = 0; i < Map.Count; i++)
        {
            Map[i].SetActive(i == chosen);
        }
    }
}
