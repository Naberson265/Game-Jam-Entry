using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeEndTrigger : MonoBehaviour
{
    public bool activated = false;
    public Transform camNewTransform;
    public Activatable lavaRise;
    public AudioClip endSong;
    private DitherTransition ditherer;
    void Start()
    {
        ditherer = FindFirstObjectByType<DitherTransition>();
    }
    void Update()
    {
        PlayerController ps = PlayerController.playerController;
        if (activated)
        {
            // Make sure the player goes far enough into the light that they get out of view.
            ps.rb.position += transform.forward * 10 * Time.deltaTime;
            ps.transform.LookAt(ps.transform.position + transform.forward);
        }
    }
    public IEnumerator StartSequence()
    {
        activated = true;
        GameController gc = GameController.gameController;
        PlayerController ps = PlayerController.playerController;
        CameraScript cms = ps.mainCam.GetComponent<CameraScript>();
        ProgressionManager.SetRecord(GameController.gameController.timePassed);
        lavaRise.activated = false;
        gc.mainGUI.SetActive(false);
        gc.EndLevelSet(camNewTransform, camNewTransform);
        gc.gameMusic.PlayOneShot(endSong);
        ps.canMove = false;
        cms.canMove = false;
        cms.transform.position = camNewTransform.position;
        cms.transform.rotation = camNewTransform.rotation;
        yield return new WaitForSeconds(2f);
        ditherer.StartAnim("Start");
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Credits");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !activated)
        {
            StartCoroutine(StartSequence());
        }
    }
}
