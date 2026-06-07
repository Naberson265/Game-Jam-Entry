using UnityEngine;

public class CamScreenManager : MonoBehaviour
{
    public GameObject mainScreen, mainGUI;
    private CameraScript cs;
    void Start()
    {
        cs = Object.FindFirstObjectByType<CameraScript>();
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
    void Update()
    {
        if (cs.paused && cs.freeCamMode)
        {
            mainScreen.SetActive(false);
            mainGUI.SetActive(false);
        }
        else
        {
            mainGUI.SetActive(true);
            mainScreen.SetActive(true);
            gameObject.SetActive(false);
        }
        if (Input.GetButtonDown("Ability"))
        {
            cs.UnlockMouse();
            cs.freeCamMode = false;
        }
    }
    public void FreeCamStart() => cs.freeCamMode = true;
}
