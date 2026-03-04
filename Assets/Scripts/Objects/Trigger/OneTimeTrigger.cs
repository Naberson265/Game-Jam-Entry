using UnityEngine;

public class OneTimeTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;
    [SerializeField] private bool setActivationTo = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = setActivationTo;
            }
        }
    }
}
