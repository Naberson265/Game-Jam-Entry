using UnityEngine;
using TMPro;

public class ButtonTimeText : MonoBehaviour
{
    private TMP_Text buttonText;
    public ButtonTrigger targetButton;

    void Start()
    {
        buttonText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        int buttonTime = Mathf.RoundToInt(targetButton.timeUntilClose);
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
