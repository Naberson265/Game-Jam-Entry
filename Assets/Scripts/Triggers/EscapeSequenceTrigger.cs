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
    public GameObject skipText;
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
    public CameraScript cms;
    void Start()
    {
        escapeObjects.SetActive(false);
        // Prevents issues when the game is paused.
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
        // Labels for people looking through code since this cutscene is a mess cause of how I did it.
        // Also to those who are snooping around, no this isn't a 4th zone, it's an extension to zone 3.
        Debug.Log("CutsceneSegment1");
        activated = true;
        GameController.gameController.SwitchSongF(introSong);
        // Disables movement but allows player and camera to decelerate.
        PlayerController.playerController.canMove = false;
        yield return new WaitForSeconds(1f);
        Debug.Log("CutsceneSegment2");
        PlayerController.playerController.useMood = false;
        PlayerController.playerController.mouthState = 1;
        // Disale UI, show skip scene text.
        GameController.gameController.mainGUI.SetActive(false);
        skipText.SetActive(true);
        // Prevents the player and camera from moving due to inputs at all as the song starts.
        cms.canMove = false;
        PlayerController.playerController.rb.isKinematic = true;
        PlayerController.playerController.transform.position = playerNewTransform.position;
        PlayerController.playerController.transform.rotation = playerNewTransform.rotation;
        cms.transform.position = camNewTransform.position;
        cms.transform.rotation = camNewTransform.rotation;
        // Starts all animations.
        PlayerController.playerController.modelAnimator.SetTrigger("Escape");
        cms.GetComponent<Animator>().enabled = true;
        cms.GetComponent<Animator>().SetTrigger("Escape");
        midCutscene = true;
        GameController.gameController.gameMusic.PlayOneShot(voiceline);
        yield return new WaitForSeconds(5.75f);
        Debug.Log("CutsceneSegment3");
        escapeObjects.transform.position = new Vector3(0, -450, 0);
        escapeObjects.SetActive(true);
        yield return new WaitForSeconds(5f);
        Debug.Log("CutsceneSegment4");
        // Expression changes during the cutscene.
        PlayerController.playerController.mouthState = 2;
        PlayerController.playerController.eyeState = 1;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("CutsceneSegment5");
        PlayerController.playerController.eyeState = 0;
        yield return new WaitForSeconds(5f);
        Debug.Log("CutsceneSegment6");
        PlayerController.playerController.mouthState = 4;
        PlayerController.playerController.eyeState = 1;
        yield return new WaitForSeconds(5.25f);
        Debug.Log("CutsceneSegment7");
        CutsceneEnd();
    }
    public void CutsceneEnd()
    {
        StopAllCoroutines();
        clockImg.sprite = newClock;
        timeText.color = Color.red;
        PlayerController.playerController.mouthState = 7;
        PlayerController.playerController.eyeState = 1;
        midCutscene = false;
        // Restores all input and resets most things as the escape starts.
        GameController.gameController.mainGUI.SetActive(true);
        skipText.SetActive(false);
        GameController.gameController.timePassed = 0;
        PlayerController.playerController.canMove = true;
        cms.canMove = true;
        PlayerController.playerController.modelAnimator.Play("Idle");
        cms.GetComponent<Animator>().enabled = false;
        PlayerController.playerController.rb.isKinematic = false;
        escapeObjects.SetActive(true);
        escapeObjects.transform.position = new Vector3(0, 0, 0);
        lavaRise.activated = true;
        RenderSettings.ambientIntensity = 1.5f;
        PlayerController.playerController.transform.position = playerEndAnimTransform.position;
        PlayerController.playerController.transform.rotation = playerEndAnimTransform.rotation;
        StartCoroutine(GameController.gameController.SwitchSong(loopSong, 0f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !activated)
        {
            StartCoroutine(StartSequence());
        }
    }
}
