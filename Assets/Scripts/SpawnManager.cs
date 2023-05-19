using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    public Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform position in spawnPoints)
        {
            position.gameObject.SetActive(false);
        }
    }


    public Transform GetSpawnPoint(){
        return spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)];
    }
}
