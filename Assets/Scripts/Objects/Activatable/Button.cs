using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Activatable activatable;

    private void OnTriggerEnter(Collider other)
    {
        activatable.activated = true;
    }
    private void OnTriggerExit(Collider other)
    {
        activatable.activated = false;
    }
}
