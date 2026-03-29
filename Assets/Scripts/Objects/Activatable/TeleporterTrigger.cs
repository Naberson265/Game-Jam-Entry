using UnityEngine;
using System.Collections;

public class TeleporterTrigger : Activatable
{
    public Vector3 destinationPos;
    public GameObject[] glows;
    public bool multiUse = true;
    public bool ditherAutoEnd = true;
    public AudioClip ambience;
    public AudioClip teleportSound;
    void Update()
    {
        if (activated)
        {
            foreach(GameObject glow in glows)
            {
                glow.SetActive(true);
            }
        }
        else
        {
            foreach(GameObject glow in glows)
            {
                glow.SetActive(false);
            }
        }
    }
    private IEnumerator PlayerTeleport()
    {
        DitherTransition ditherer = FindFirstObjectByType<DitherTransition>();
        PlayerController ps = PlayerController.playerController;
        ps.canMove = false;
        ps.playerAudio.PlayOneShot(teleportSound);
        ditherer.StartAnim("Start");
        yield return new WaitForSeconds(1);
        if (ditherAutoEnd) ditherer.StartAnim("End");
        ps.rb.position = destinationPos;
        ps.canMove = true;
        if (!multiUse) activated = false;
    }
    private IEnumerator ObjectTeleport(Rigidbody rbToTeleport)
    {
        GetComponent<AudioSource>().PlayOneShot(teleportSound);
        yield return new WaitForSeconds(1);
        rbToTeleport.position = destinationPos;
        if (!multiUse) activated = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() && activated)
        {
            if (other.gameObject.layer != 3)
            {
                Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
                StartCoroutine(ObjectTeleport(otherRb));
            }
        }
        if (other.gameObject.layer == 3 && activated)
        {
            StartCoroutine(PlayerTeleport());
        }
    }
}
