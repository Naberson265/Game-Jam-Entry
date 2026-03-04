using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidTextureScript : MonoBehaviour
{
    public Vector2 vector;
    public float liquidSpeed = 0.5f;
    public MeshRenderer meshRenderer;
    private void Update()
    {
        if (Time.timeScale != 0f)
        {
            vector += new Vector2(liquidSpeed * Time.deltaTime, liquidSpeed * Time.deltaTime);
            meshRenderer.material.SetTextureOffset("_MainTex", vector);
        }
    }
}
