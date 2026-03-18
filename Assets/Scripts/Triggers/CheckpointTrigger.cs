using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public bool triggerActivated = false;
    public Transform spawnpointPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !triggerActivated)
        {
            PlayerController.playerController.spawnpoint = spawnpointPosition.position;
            Resettable.SaveDefaults();
            triggerActivated = true;
        }
    }
}
