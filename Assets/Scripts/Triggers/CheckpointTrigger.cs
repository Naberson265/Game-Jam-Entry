using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public bool triggerActivated = false;
    public Transform spawnpointPosition;
    public Transform flagTop;
    public AudioSource Sound;

    private void FixedUpdate()
    {
        if (triggerActivated && flagTop)
        {
            flagTop.localPosition = Vector3.Lerp(flagTop.localPosition, new Vector3(flagTop.localPosition.x, 3.5f, flagTop.localPosition.z), 0.05f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !triggerActivated)
        {
            Sound.Play();
            PlayerController.playerController.spawnpoint = spawnpointPosition.position;
            Resettable.SaveDefaults();
            triggerActivated = true;
        }
    }
}
