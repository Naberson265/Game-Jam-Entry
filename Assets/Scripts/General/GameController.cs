using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static public float[,,] rankTimes = new float[4, 4, 4] {
//Level   0                    1                     2             3
//Rank    S  A  B  C           S  A  B  C            S  A  B  C    S  A  B  C
        {{50, 100, 200, 400}, {150, 200, 400, 900}, {210, 250, 500, 1000}, {0, 0, 0, 0}},    //Zone 1
        {{45, 120, 300, 600}, {120, 400, 600, 1300}, {120, 240, 500, 900}, {0, 0, 0, 0}},    //Zone 2
        {{120, 210, 400, 800}, {190, 270, 400, 1000}, {300, 360, 600, 1200}, {0, 0, 0, 0}},    //Zone 3
        {{120, 210, 240, 300}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},     //3-4
    };

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
    // Almost always true except for the zone 3s. When off the player keeps movement and rank screen isn't shown.
    public bool showRankScreen = true;
    // The below two are set manually every level.
    public int zone;
    public int levelCount;
    // Include the main (blue), and two level timers in the below array:
    public GameObject[] levelTimers;
    public GameObject[] levelSpawnpoints;
    // If on, reloads the scene instead of calling Resettables on death.
    public bool reloadOnDeath;

    [Header("Fog")]
    public Color currentFogColor = Color.white;
    public float currentFogDensity = 0f;
    private void Awake()
    {
        if (gameController != null && gameController != this)
        {
            Destroy(gameObject); // Destroy duplicate
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
    public void SwitchSongF(AudioClip songToSwitch, float switchDelay = 1)
    {
        StartCoroutine(SwitchSong(songToSwitch, switchDelay));
    }
    public IEnumerator SwitchSong(AudioClip songToSwitch, float switchDelay = 1)
    {
        gameMusic.Stop();
        yield return new WaitForSeconds(switchDelay);
        gameMusic.clip = songToSwitch;
        gameMusic.Play();
    }
    protected void SaveStuff()
    {
        Resettable.SaveDefaults();
    }

    public string getRank(float time, int level, int zone)
    {
        if (time < rankTimes[zone - 1, level, 0])
        {
            levelRanks.Add(0);
            return "S";
        }
        else if (time < rankTimes[zone - 1, level, 1])
        {
            levelRanks.Add(1);
            return "A";
        }
        else if (time < rankTimes[zone - 1, level, 2])
        {
            levelRanks.Add(2);
            return "B";
        }
        else if (time < rankTimes[zone - 1, level, 3])
        {
            levelRanks.Add(3);
            return "C";
        }
        else
        {
            levelRanks.Add(4);
            return "D";
        }
    }

    public void StartNewLevel()
    {
        Invoke("SaveStuff", 0.5f);
        if (timePassed > 5)   // Lazy fix but I don't feel like reworking everything. Hopefully someone doesn't lag for more than 5 seconds.
        {
            ProgressionManager.SetRecord(timePassed);
        }
        currentLevel++;
        ProgressionManager.SaveProgess(PlayerController.playerController.gameObject.transform.position);
        if (timePassed > 1)
        {
            SetLevelTimer(currentLevel-1, timePassed);
        }
        gameMusic.Stop();
        gameMusic.clip = levelSongs[currentLevel];
        gameMusic.Play();
        timePassed = 0f;
    }
    public void SetLevelTimer(int level, float time)
    {
        string rankString = getRank(time,level,zone);
        levelTimers[level+1].GetComponent<TMP_Text>().text = "(" +  rankString +  ")" + "L" + (level+1).ToString() + ": " + CalculateFormattedTime(time);
        levelTimers[level+1].SetActive(true);
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
        //string rankString = getRank(timePassed, currentLevel, zone);


        gameMusic.Stop();
        finalRank = 0;
        if (showRankScreen)
        {
            print(levelRanks.Count);
            foreach (int level in levelRanks)
            {
                print(level);
            }
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
        }
        ProgressionManager.SetRecord(timePassed);
        currentLevel = 0;
        zone++;     // Lazy Fix
        ProgressionManager.SaveProgess(PlayerController.playerController.gameObject.transform.position);
        zone--;     // Lazy Fix
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
