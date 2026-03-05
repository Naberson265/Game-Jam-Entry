using UnityEngine;
using System.Collections;

public class ButtonVisual : MonoBehaviour
{
    void Start()
    {
        buttonInitialPos = buttonVis.transform.position;
    }
    void Update()
    {
        if (pressed)
        {
            buttonVis.transform.position = new Vector3(transform.position.x, buttonInitialPos.y - distanceDown, transform.position.z);
        }
        else
        {
            buttonVis.transform.position = new Vector3(transform.position.x, buttonInitialPos.y, transform.position.z);
        }
        if (timeUntilRelease > 0f) timeUntilRelease -= Time.deltaTime;
        else pressed = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (!pressed)
        {
            pressed = true;
            timeUntilRelease = 0.1f;
        }
    }
    private float timeUntilRelease;
    public float distanceDown = 0.75f;
    public bool pressed = false;
    private Vector3 buttonInitialPos;
    public GameObject buttonVis;
    public PlayerScript pScript;
}
