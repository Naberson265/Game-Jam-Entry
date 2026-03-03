using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text timeText;

    private void Start()
    {
        // Gets this objects Text component for faster reference later.
        timeText = gameObject.GetComponent<TMP_Text>();
    }
    void Update()
    {
        int timeMinutes;
        int timeSeconds;
        string formattedTime;

        // Using static gameController to get time.
        float timePassed = GameController.gameController.timePassed;
        if (timePassed > 0f) timeMinutes = Mathf.FloorToInt(timePassed / 60);
        else timeMinutes = 0;
        if (timePassed > 0f) timeSeconds = Mathf.FloorToInt(timePassed % 60);
        else timeSeconds = 0;
        formattedTime = string.Format("{0:00}:{1:00}", timeMinutes, timeSeconds);

        timeText.text = formattedTime.ToString();
    }
}
