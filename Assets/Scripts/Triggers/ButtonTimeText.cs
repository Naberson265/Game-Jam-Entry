using UnityEngine;
using TMPro;

public class ButtonTimeText : MonoBehaviour
{
    private TMP_Text buttonText;
    private AudioSource buttonAud;
    private int lastButtonTime;
    public ButtonTrigger targetButton;
    public AudioClip clockTick;
    void Start()
    {
        buttonText = GetComponent<TMP_Text>();
        buttonAud = targetButton.GetComponent<AudioSource>();
    }
    void Update()
    {
        int buttonTime = Mathf.RoundToInt(targetButton.timeUntilClose);
        if (lastButtonTime != buttonTime) buttonAud.PlayOneShot(clockTick);
        lastButtonTime = buttonTime;
        if (buttonTime > 0f)
        {
            buttonText.text = buttonTime.ToString();
            buttonText.color = new Color(0f, 0f, 0f, 1f);
        }
        else
        {
            buttonText.text = "0";
            buttonText.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }
}
