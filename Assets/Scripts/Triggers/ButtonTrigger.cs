using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;
    public float onPressTime = 0.1f;
    public float timeUntilClose;

    void Update()
    {
        if (timeUntilClose > 0f) timeUntilClose -= Time.deltaTime;
        else
        {
            timeUntilClose = 0f;
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        timeUntilClose = onPressTime;
        foreach (Activatable activatable in activatables)
        {
            activatable.activated = true;
        }
    }
}
