using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    public bool activated;

    public void Toggle()
    {
        if (activated)
        {
            activated = false;
        } else
        {
            activated = true;
        }
    }
}

/*
 * Seems like a stupid script, but is actually incredibly convenient.
 * Now anytime you want a button or lever to activate an object
 * you can reference the object type of Activatable which will work
 * for doors, moving platforms, spikes, and whatever else you want to
 * add.
 */