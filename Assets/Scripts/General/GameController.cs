using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Objects")]
    public static GameController gameController { get; private set; }
    public GameObject mainGUI;
    public GameObject endScreen;
    public float timePassed;
    public AudioSource gameMusic;

    [Header("Level Related")]
    public int currentLevel;
    public AudioClip[] levelSongs;
    public AudioClip[] rankThemes;
    // A rank (from 0(S)-4(D)) is added to this list each level, then averaged at the zone ending.
    public List<int> levelRanks;
    public int finalRank;
    // The below two are set manually every level.
    public int zone;
    public int levelCount;
    // Include the main (blue), and two level timers in the below array:
    public GameObject[] levelTimers;
    public GameObject[] levelSpawnpoints;

    [Header("Fog")]
    public Color currentFogColor = Color.white;
    public float currentFogDensity = 0f;
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
        endScreen.SetActive(false);
        mainGUI.SetActive(true);
        RenderSettings.fog = true;
        gameMusic = GetComponent<AudioSource>();
        // Make the timers only appear when needed besides the main one which is almost always on.
        foreach (GameObject timer in levelTimers)
        {
            timer.SetActive(false);
        }
        if (levelTimers.Length > 0)
        {
            levelTimers[0].SetActive(true);
        }
    }
    void Update()
    {
        timePassed += Time.deltaTime;
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, currentFogColor, 0.075f);
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, currentFogDensity, 0.075f);
    }
    public void SetFog(float newDensity, Color newFogColor)
    {
        RenderSettings.fog = true;
        currentFogDensity = newDensity;
        currentFogColor = newFogColor;
    }
    public void StartNewLevel()
    {
        string lastLRank;
        if (levelRanks[levelRanks.Count - 1] == 0) lastLRank = "S";
        else if (levelRanks[levelRanks.Count - 1] == 1) lastLRank = "A";
        else if (levelRanks[levelRanks.Count - 1] == 2) lastLRank = "B";
        else if (levelRanks[levelRanks.Count - 1] == 3) lastLRank = "C";
        else if (levelRanks[levelRanks.Count - 1] == 4) lastLRank = "D";
        else lastLRank = "N/A";
        Resettable.SaveDefaults();
        if (timePassed > 5)   // Lazy fix but I don't feel like reworking everything. Hopefully someone doesn't lag for more than 5 seconds.
        {
            ProgressionManager.SetRecord(timePassed);
        }
        currentLevel++;
        ProgressionManager.SaveProgess(PlayerController.playerController.gameObject.transform.position);
        if (timePassed > 1)
        {
            SetLevelTimer(currentLevel, lastLRank, timePassed);
        }
        gameMusic.Stop();
        gameMusic.clip = levelSongs[currentLevel];
        gameMusic.Play();
        timePassed = 0f;
    }
    public void SetLevelTimer(int level, string rank, float time)
    {
        string rankString;
        if (rank.Length > 0)
        {
            rankString = "(" + rank + ") ";
        } else
        {
            rankString = "";
        }
        levelTimers[level].GetComponent<TMP_Text>().text = rankString + "L" + level.ToString() + ": " + CalculateFormattedTime(time);
        levelTimers[level].SetActive(true);
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
    public void EndLevelSet(Transform cameraPos, Transform playerPos)
    {
        gameMusic.Stop();
        finalRank = 0;
        if (levelRanks.Count != 0 && levelRanks.Count == levelCount)
        {
            foreach (int lRank in levelRanks)
            {
                finalRank += lRank;
            }
            int newFR = finalRank /= levelRanks.Count;
            finalRank = Mathf.RoundToInt(newFR);
            gameMusic.PlayOneShot(rankThemes[finalRank]);
        }
        else
        {
            finalRank = 5;
            gameMusic.PlayOneShot(rankThemes[4]);
        }
        PlayerController ps = PlayerController.playerController;
        ps.canMove = false;
        ps.transform.position = playerPos.position;
        ps.transform.rotation = playerPos.rotation;
        ps.mainCam.GetComponent<CameraScript>().canMove = false;
        ps.mainCam.GetComponent<CameraScript>().UnlockMouse();
        ps.mainCam.transform.position = cameraPos.position;
        ps.mainCam.transform.rotation = cameraPos.rotation;
        endScreen.SetActive(true);
        mainGUI.SetActive(false);
        ProgressionManager.SetRecord(timePassed);
        currentLevel = 0;
        zone++;
        ProgressionManager.SaveProgess(PlayerController.playerController.gameObject.transform.position);
    }
    public static void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public static void MovePlayerToLevel(int index)
    {
        PlayerController.playerController.gameObject.transform.position = gameController.levelSpawnpoints[index].transform.position;
    }
}
