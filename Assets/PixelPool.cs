using System.Collections.Generic;
using UnityEngine;

public class PixelPool : MonoBehaviour
{
    public static PixelPool Instance { get; private set; }

    [SerializeField]
    private GameObject pixelPrefab; // Pixel prefab

    private Queue<GameObject> availablePixels = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 50; i++) // Adjust pool size as needed
        {
            var pixel = Instantiate(pixelPrefab);
            pixel.transform.SetParent(transform);
            AddToPool(pixel);
        }
    }

    public void AddToPool(GameObject pixel)
    {
        pixel.SetActive(false);
        availablePixels.Enqueue(pixel);
    }

    public GameObject GetFromPool()
    {
        if (availablePixels.Count == 0)
        {
            GrowPool();
        }
        var pixel = availablePixels.Dequeue();
        pixel.SetActive(true);
        return pixel;
    }
}
