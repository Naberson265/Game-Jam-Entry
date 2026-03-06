using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    void Update()
    {
        if (pressed)
        {
            GetComponent<Collider>().enabled = false;
        }
        else
        {
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
    public PlayerController pScript;
    public CameraScript cScript;
}
