using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }
    public float timePassed;
    public TMP_Text L1time;
    public TMP_Text L2time;
    public AudioSource gameMusic;
    public AudioClip[] levelSongs;
    public int currentLevel;
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
        L1time.gameObject.SetActive(false);
        L2time.gameObject.SetActive(false);
    }
    void Update()
    {
        timePassed += Time.deltaTime;
    }
    public void StartNewLevel()
    {
        if (currentLevel == 0)
        {
            L1time.text = "L1: " + timePassed.ToString();
            L1time.gameObject.SetActive(true);
        }
        if (currentLevel == 1)
        {
            L2time.text = "L2: " + timePassed.ToString();
            L2time.gameObject.SetActive(true);
        }
        currentLevel++;
        gameMusic.Stop();
        gameMusic.clip = levelSongs[currentLevel];
        gameMusic.Play();
        timePassed = 0f;
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
