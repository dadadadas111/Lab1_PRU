using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailEnemyPool : MonoBehaviour
{
    public static SnailEnemyPool Instance { get; private set; }

    [SerializeField]
    private GameObject snailPrefab;

    private Queue<GameObject> availableSnails = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(snailPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableSnails.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if (availableSnails.Count == 0)
        {
            GrowPool();
        }
        var instance = availableSnails.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
