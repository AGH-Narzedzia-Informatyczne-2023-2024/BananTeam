using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWrogow : MonoBehaviour
{
    void Update()
    {
        if(Time.time % 5 == 0) 
        {
            GameObject prefab = Resources.Load("Prefaby/DoomerBaba") as GameObject;
            Spawnuj(prefab);
        }
    }

    public void Spawnuj(GameObject prefab)
    {
        Instantiate(prefab, transform.localPosition, Quaternion.identity);
    }
}
