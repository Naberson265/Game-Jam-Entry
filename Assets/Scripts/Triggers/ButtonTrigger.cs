using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Activatable[] activatables;
    private AudioSource buttonAud;
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
    private void OnTriggerEnter()
    {
        buttonAud.PlayOneShot(pressSound);
    }
    private void OnTriggerExit()
    {
        buttonAud.PlayOneShot(releaseSound);
    }
}
