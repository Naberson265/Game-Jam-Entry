using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool triggerActivated = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !triggerActivated)
        {
            Resettable.SaveDefaults();
        }
    }
}
