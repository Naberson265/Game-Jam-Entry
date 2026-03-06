using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCrushingScript : MonoBehaviour
{
    // Add three of these as children of a moving platform to prevent clipping and have the player
    // be crushed instead of clipping through a surface.
    // Make sure that these stick out 0.5 units in their specific axis and -0.1 on others.
    public MovingPlatform targetPlatform;
    public PlayerController pScript;
    public float playerCollidingTime;
    public float surfaceCollidingTime;

    void FixedUpdate()
    {
        if (playerCollidingTime > 0f) playerCollidingTime -= Time.deltaTime;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.layer == 3) playerCollidingTime = 0.1f;
        else if (other.transform.gameObject.layer == 6) surfaceCollidingTime = 0.1f;
    }
}
