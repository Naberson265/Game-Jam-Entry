using UnityEngine;
using TMPro;

public class RecordTimeText : MonoBehaviour
{
    public int level;
    public int zone;

    // TextMeshProUGUI
    private TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = "Best Time: " +  CalculateFormattedTime(ProgressionManager.GetRecord(level, zone));
    }

    public string CalculateFormattedTime(float timeToFormat)
    {
        int timeMinutes;
        int timeSeconds;
        if (timeToFormat > 0f) timeMinutes = Mathf.FloorToInt(timeToFormat / 60);
        else timeMinutes = 0;
        if (timeToFormat > 0f) timeSeconds = Mathf.FloorToInt(timeToFormat % 60);
        else timeSeconds = 0;
        return string.Format("{0:00}:{1:00}", timeMinutes, timeSeconds);
    }
}
