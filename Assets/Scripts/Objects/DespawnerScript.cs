using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnerScript : MonoBehaviour
{
    public float despawnTime;
    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0f) Destroy(gameObject);
    }
}
