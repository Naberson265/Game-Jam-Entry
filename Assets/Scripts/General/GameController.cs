using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }
    // Include the main (blue), and two level timers in the below array:
    public GameObject[] levelTimers;
    public GameObject[] levelSpawnpoints;
    public GameObject mainGUI;
    public GameObject endScreen;
    public float timePassed;
    public int finalRank;
    public AudioSource gameMusic;
    public AudioClip[] levelSongs;
    public AudioClip[] rankThemes;
    // A rank (from 0(S)-4(D)) is added to this list each level, then averaged at the zone ending.
    public List<int> levelRanks;
    public int currentLevel;
    //Set manually every level
    public int zone;
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
        int lastLRank = levelRanks.Count - 1;
        Resettable.SaveDefaults();
        ProgressionManager.SetRecord(timePassed);
        currentLevel++;
        ProgressionManager.SaveProgess(PlayerController.playerController.gameObject.transform.position);
        levelTimers[currentLevel].GetComponent<TMP_Text>().text = "(" + lastLRank.ToString() +
        ") L" + currentLevel.ToString() + ": " + CalculateFormattedTime(timePassed);
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
    public void EndLevelSet(Transform cameraPos, Transform playerPos)
    {
        gameMusic.Stop();
        finalRank = 0;
        if (levelRanks.Count != 0)
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
            finalRank = 4;
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
