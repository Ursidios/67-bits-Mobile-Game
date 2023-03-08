using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject SpawnerEnemy;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private float TimerSpawn;
    [SerializeField] private float TimerSpawnC;
    [SerializeField] private int EnemysCounter;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TimerSpawnC -= Time.deltaTime;
        
        if (TimerSpawnC <= 0 && EnemysCounter <= 10)
        {
            TimerSpawnC = TimerSpawn;
            Instantiate(Enemy, SpawnerEnemy.transform.position, SpawnerEnemy.transform.rotation);
            EnemysCounter++;
        }


    }

    public void SubtractEnemy()
    {
        EnemysCounter--;
    }
}
