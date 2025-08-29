using UnityEngine;
using System.Collections.Generic;

public class towerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private lavaSet lava;

    [Header("Tower Settings")]
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private float sectionHeight = 10f;
    [SerializeField] private int bufferSections = 3;

    private List<GameObject> spawnedSections = new List<GameObject>();
    private float highestSectionY = 0f;

    void Start()
    {
        for (int i = 0; i < bufferSections; i++)
        {
            SpawnNextSection();
        }

    }

    void Update()
    {
        if (player.position.y + (bufferSections * sectionHeight) > highestSectionY)
        {
            SpawnNextSection();
        }

        CleanupSections();
    }

    void SpawnNextSection()
    {
        GameObject prefab = towerPrefabs[Random.Range(0, towerPrefabs.Length)];
        Vector3 spawnPosition = new Vector3(0, highestSectionY, 0);
        GameObject newSection = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnedSections.Add(newSection);
        highestSectionY += sectionHeight;
    }

    void CleanupSections()
    {
        float lavaHeight = lava.GetLavaHeight();
        for (int i = spawnedSections.Count - 1; i >= 0; i--)
        {
            GameObject section = spawnedSections[i];
            if (section.transform.position.y + sectionHeight < lavaHeight)
            {
                Destroy(section);
                spawnedSections.RemoveAt(i);
            }
        }
    }
}
