using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MyEnemySpawner : MonoBehaviour
{
    [Header("Spawning Prefabs")]
    public List<GameObject> robotPrefabs;
    public List<GameObject> vehiclePrefabs;
    public List<GameObject> animalPrefabs;
    [Header("Spawning Prefabs")]
    public List<Transform> spawnPoint;
    private int spawnPointIndex = 0;
    private int spawnIndex = 0;
    public void Spawn(int totalRobots, int totalVehicles, int totalAnimals)
    {
        SpawNow(robotPrefabs, spawnPoint, totalRobots);
        SpawNow(vehiclePrefabs, null, totalVehicles);
        SpawNow(animalPrefabs, spawnPoint, totalAnimals);
    }

    private void SpawNow(List<GameObject> spawnerList, List<Transform> points, int spawnCount)
    {
        for(int i = 0; i < spawnCount; i++)
        {
            if(spawnerList.Count > spawnIndex)
            {
                if (spawnerList[spawnIndex])
                {
                    GameObject enemy = Instantiate(spawnerList[spawnIndex]);
                    spawnIndex++;
                    if (spawnIndex == spawnerList.Count)
                    {
                        spawnIndex = 0;
                    }
                    if (points != null && points.Count > spawnPointIndex)
                    {
                        if (points[spawnPointIndex])
                        {
                            enemy.transform.position = points[spawnPointIndex].position;
                            enemy.transform.rotation = points[spawnPointIndex].rotation;
                            spawnPointIndex++;
                            if (spawnPointIndex == points.Count)
                            {
                                spawnPointIndex = 0;
                            }
                        }                     
                    }
                }
            }    
        }
        spawnIndex = 0;
    }
}
