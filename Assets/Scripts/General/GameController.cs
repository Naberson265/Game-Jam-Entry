using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }
    public float timePassed;
    public static AudioSource gameMusic;
    public static AudioClip[] levelSongs;
    public static int currentLevel;
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
    }
    void Update()
    {
        timePassed += Time.deltaTime;
    }
    public static void StartNewLevel()
    {
        currentLevel++;
        gameMusic.Stop();
        gameMusic.clip = levelSongs[currentLevel];
        gameMusic.Play();
    }
    public static void EndLevelSet()
    {
        gameMusic.Stop();
    }
    public static void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
