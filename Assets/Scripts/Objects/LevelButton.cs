using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
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
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            buttonVis.transform.position = new Vector3(transform.position.x, buttonInitialPos.y, transform.position.z);
            GetComponent<Collider>().enabled = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !pressed)
        {
            pressed = true;
            PlayerPrefs.SetInt(buttonName, 1);
        }
    }
    public bool pressed = false;
    public float distanceDown = 0.75f;
    public string buttonName = "Temp";
    private Vector3 buttonInitialPos;
    public GameObject buttonVis;
    public PlayerController pScript;
    public CameraScript cScript;
}
