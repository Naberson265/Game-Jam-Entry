using UnityEngine;
using System.Collections;

public class EscapeSequenceTrigger : MonoBehaviour
{
    public bool activated = false;
    public Transform playerNewTransform;
    public Transform camNewTransform;
    public Activatable lavaRise;
    public AudioClip songSwitch;
    void Update()
    {
        if (activated)
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, 1.5f, 0.01f);
        }
    }
    public IEnumerator StartSequence()
    {
        activated = true;
        GameController gc = GameController.gameController;
        PlayerController ps = PlayerController.playerController;
        CameraScript cms = ps.mainCam.GetComponent<CameraScript>();
        StartCoroutine(GameController.gameController.SwitchSong(songSwitch));
        // Disables movement but allows player and camera to decelerate.
        ps.canMove = false;
        yield return new WaitForSeconds(1f);
        ps.mouthState = 4;
        gc.mainGUI.SetActive(false);
        // Prevents the player and camera from moving due to inputs at all as the song starts.
        cms.canMove = false;
        ps.rb.isKinematic = true;
        ps.transform.position = playerNewTransform.position;
        ps.transform.rotation = playerNewTransform.rotation;
        cms.transform.position = camNewTransform.position;
        cms.transform.rotation = camNewTransform.rotation;
        yield return new WaitForSeconds(21.5f);
        ps.mouthState = 7;
        ps.eyeState = 1;
        // Restores all input as the escape starts.
        gc.mainGUI.SetActive(true);
        ps.canMove = true;
        cms.canMove = true;
        ps.rb.isKinematic = false;
        lavaRise.activated = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !activated)
        {
            StartCoroutine(StartSequence());
        }
    }
}
