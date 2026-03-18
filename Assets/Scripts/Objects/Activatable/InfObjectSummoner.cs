using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfObjectSpawn : Activatable
{
    public float spawnInterval = 2f;
    private float spawnTime = 2f;
    public float despawnTime = 30f;
    public GameObject objToSummon;
    private void Start()
    {
        activated = true;
    }
    private void Update()
    {
        spawnTime -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (activated)
        {
            if (spawnTime < 0f)
            {
                GameObject newObject = Instantiate(objToSummon, transform.position, transform.rotation);
                newObject.AddComponent<DespawnerScript>();
                newObject.GetComponent<DespawnerScript>().despawnTime = despawnTime;
                spawnTime = spawnInterval;
            }
        }
    }
}
