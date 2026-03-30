using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EscapeSequenceTrigger : MonoBehaviour
{
    public bool activated = false;
    public bool midCutscene = false;
    public float levelRiseSpeed = 15f;
    public GameObject escapeObjects;
    public Image clockImg;
    public TMP_Text timeText;
    public Sprite newClock;
    public Transform playerNewTransform;
    public Transform playerEndAnimTransform;
    public Transform camNewTransform;
    public Activatable lavaRise;
    public AudioClip introSong;
    public AudioClip loopSong;
    public AudioClip voiceline;
    void Start()
    {
        escapeObjects.SetActive(false);
        // Prevents issues when the game is paused.
        CameraScript cms = Camera.main.GetComponent<CameraScript>();
        cms.GetComponent<Animator>().enabled = false;
    }
    void Update()
    {
        if (midCutscene)
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, 1.5f, 0.01f);
            if (midCutscene)
            {
                if (Input.GetButtonDown("Ability"))
                {
                    // Press the Q key to skip the cutscene.
                    CutsceneEnd();
                }
                if (escapeObjects.transform.position.y < 0f)
                {
                    escapeObjects.transform.position += new Vector3(0f, levelRiseSpeed * Time.deltaTime, 0f);
                }
                else
                {
                    escapeObjects.transform.position = new Vector3(0, 0, 0);
                }
            }
        }
    }
    public IEnumerator StartSequence()
    {
        activated = true;
        GameController gc = GameController.gameController;
        PlayerController ps = PlayerController.playerController;
        CameraScript cms = ps.mainCam.GetComponent<CameraScript>();
        StartCoroutine(gc.SwitchSong(introSong));
        // Disables movement but allows player and camera to decelerate.
        ps.canMove = false;
        yield return new WaitForSeconds(1f);
        ps.useMood = false;
        ps.mouthState = 1;
        gc.mainGUI.SetActive(false);
        // Prevents the player and camera from moving due to inputs at all as the song starts.
        cms.canMove = false;
        ps.rb.isKinematic = true;
        ps.transform.position = playerNewTransform.position;
        ps.transform.rotation = playerNewTransform.rotation;
        cms.transform.position = camNewTransform.position;
        cms.transform.rotation = camNewTransform.rotation;
        // Starts all animations.
        ps.modelAnimator.SetTrigger("Escape");
        cms.GetComponent<Animator>().enabled = true;
        cms.GetComponent<Animator>().SetTrigger("Escape");
        midCutscene = true;
        gc.gameMusic.PlayOneShot(voiceline);
        yield return new WaitForSeconds(5.75f);
        escapeObjects.transform.position = new Vector3(0, -450, 0);
        escapeObjects.SetActive(true);
        yield return new WaitForSeconds(5f);
        // Expression changes during the cutscene.
        ps.mouthState = 2;
        ps.eyeState = 1;
        yield return new WaitForSeconds(0.5f);
        ps.eyeState = 0;
        yield return new WaitForSeconds(5f);
        ps.mouthState = 4;
        ps.eyeState = 1;
        yield return new WaitForSeconds(5.25f);
        CutsceneEnd();
    }
    public void CutsceneEnd()
    {
        StopAllCoroutines();
        GameController gc = GameController.gameController;
        PlayerController ps = PlayerController.playerController;
        CameraScript cms = ps.mainCam.GetComponent<CameraScript>();
        clockImg.sprite = newClock;
        timeText.color = Color.red;
        ps.mouthState = 7;
        ps.eyeState = 1;
        midCutscene = false;
        // Restores all input and resets most things as the escape starts.
        gc.mainGUI.SetActive(true);
        gc.timePassed = 0;
        ps.canMove = true;
        cms.canMove = true;
        ps.modelAnimator.Play("Idle");
        cms.GetComponent<Animator>().enabled = false;
        ps.rb.isKinematic = false;
        escapeObjects.transform.position = new Vector3(0, 0, 0);
        lavaRise.activated = true;
        ps.transform.position = playerEndAnimTransform.position;
        ps.transform.rotation = playerEndAnimTransform.rotation;
        StartCoroutine(gc.SwitchSong(loopSong, 0f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !activated)
        {
            StartCoroutine(StartSequence());
        }
    }
}
