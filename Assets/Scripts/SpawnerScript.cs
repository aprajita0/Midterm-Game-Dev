using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    //This number tracks how much time there is until the next item spawns
    //It changes in real time, always counting down to 0
    public float Countdown = 1f;

    //This number tracks how much time there is between items spawning
    //It never changes, but Countdown uses it to know what to reset to after triggering
    public float SpawnTime = 2f;

    //This is the object that should spawn when the timer triggers. It can be any prefab
    public GameObject SpawnedObject;
    public GameObject RareCoinPrefab;
    [Range(0f, 1f)]
    public float rareChance = 0.2f;

    //This sets how far from the spawner the spawned object can spawn
    //The object spawns in a random location within this x and y distance of the spawner
    //If set to 0,0, the object always spawns right on top of the spawner
    public Vector2 Range = new Vector2(2,8);
    public int MaxCoins = 1;
    public float MinX = -8f;
    public float MaxX = 8f;
    public float MinY = -4f;
    public float MaxY = 4f;
    
    void Update()
    {
        int coinCount = FindObjectsOfType<CoinScript>().Length;
        int rareCount = FindObjectsOfType<RareCoin>().Length;
        if (coinCount >= MaxCoins || rareCount >= MaxCoins) return;

        //Every frame, make the Countdown variable go down in real time.
        Countdown -= Time.deltaTime;
        
        //If Countdown hits 0, that means it's time to spawn the new object
        if (Countdown <= 0)
        {
            //First we decide where the object should spawn.
            //We take the spawner's position (transform.position) and offset it by the random spawn range
            Vector3 where = transform.position + new Vector3(Random.Range(MinX, MaxX), 
                Random.Range(MinY, MaxY), 0);
            
            //This line of code actually spawns the object. Ignore 'Quaternion.identity' for now
            GameObject coinToSpawn;

        if (Random.value < rareChance)
            {
                coinToSpawn = RareCoinPrefab;
            }
        else
            {
                coinToSpawn = SpawnedObject;
            }
            
            Instantiate(coinToSpawn, where, Quaternion.identity);
                
                //Finally, reset the Countdown timer so that the countdown starts all over again
            Countdown = SpawnTime;
        }
    }
}
