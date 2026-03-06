using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;
    private float timeUntilClose;

    void Update()
    {
        if (timeUntilClose > 0f) timeUntilClose -= Time.deltaTime;
        else
        {    
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        timeUntilClose = 0.1f;
        foreach (Activatable activatable in activatables)
        {
            activatable.activated = true;
        }
    }
}
