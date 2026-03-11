using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;
    private AudioSource buttonAud;
    // private bool lastState;
    public float onPressTime = 0.1f;
    public float timeUntilClose;
    public AudioClip pressSound;
    public AudioClip releaseSound;

    void Start()
    {
        buttonAud = GetComponent<AudioSource>();
    }
    void Update()
    {
        // lastState = activatables[0].activated;
        if (timeUntilClose > 0f) timeUntilClose -= Time.deltaTime;
        else
        {
            timeUntilClose = 0f;
            // if (lastState == true) buttonAud.PlayOneShot(releaseSound);
            foreach (Activatable activatable in activatables)
            {
                activatable.activated = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // if (timeUntilClose <= 0f) buttonAud.PlayOneShot(pressSound);
        timeUntilClose = onPressTime;
        foreach (Activatable activatable in activatables)
        {
            activatable.activated = true;
        }
    }
}
