using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }
    public float timePassed;
    public AudioSource gameMusic;
    public AudioClip[] levelSongs;
    public int currentLevel;

    // Set zone manually for each scene
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
    }
    void Update()
    {
        timePassed += Time.deltaTime;
    }
    public void StartNewLevel()
    {
        ProgressionManager.SetRecord(timePassed);
        currentLevel++;
        ProgressionManager.SaveProgess(PlayerController.playerController.transform.position);
        Resettable.SaveDefaults();
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
