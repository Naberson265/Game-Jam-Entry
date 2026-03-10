using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }
    // Include the main (blue), and two level timers in the below array:
    public GameObject[] levelTimers;
    public float timePassed;
    public AudioSource gameMusic;
    public AudioClip[] levelSongs;
    public int currentLevel;

    //Set manually every level
    public int zone;
    private void Awake()
    {
        if (gameController != null && gameController != this)
        {
            Destroy(this.gameObject); // Destroy duplicate
            return;
        }
        gameController = this;
    }
    void Start()
    {
        gameMusic = GetComponent<AudioSource>();
        // Make the timers only appear when needed besides the main one which is almost always on.
        foreach (GameObject timer in levelTimers)
        {
            timer.SetActive(false);
        }
        levelTimers[0].SetActive(true);
    }
    void Update()
    {
        timePassed += Time.deltaTime;
    }
    public void StartNewLevel()
    {
        ProgressionManager.SetRecord(timePassed);
        currentLevel++;
        ProgressionManager.SaveProgess(PlayerController.playerController.gameObject.transform.position);
        levelTimers[currentLevel].GetComponent<TMP_Text>().text = 
        "L" + currentLevel.ToString() + ": " + CalculateFormattedTime(timePassed);
        levelTimers[currentLevel].SetActive(true);
        gameMusic.Stop();
        gameMusic.clip = levelSongs[currentLevel];
        gameMusic.Play();
        timePassed = 0f;
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
    public void EndLevelSet()
    {
        gameMusic.Stop();
    }
    public static void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
