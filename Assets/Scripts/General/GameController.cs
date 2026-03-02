using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    void Update()
    {
        timePassed += Time.deltaTime;
        int timeMinutes;
        int timeSeconds;
		string formattedTime;
        if (timePassed > 0f) timeMinutes = Mathf.FloorToInt(timePassed / 60);
		else timeMinutes = 0;
        if (timePassed > 0f) timeSeconds = Mathf.FloorToInt(timePassed % 60);
		else timeSeconds = 0;
		formattedTime = string.Format("{0:00}:{1:00}", timeMinutes, timeSeconds);
		timeText.text = formattedTime.ToString();
    }
    public void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public float timePassed;
    public TMP_Text timeText;
}
