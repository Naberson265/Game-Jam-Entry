using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    void Update()
    {
        if (pressed)
        {
            buttonVis.transform.position = new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z);
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            buttonVis.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
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
    public string buttonName = "Temp";
    public GameObject buttonVis;
    public PlayerScript pScript;
    public CameraScript cScript;
}
