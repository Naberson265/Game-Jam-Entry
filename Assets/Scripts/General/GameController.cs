using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Feel free to remove these comments once you've read and understood them.

    /* 
     * static is SUPER USEFUL. It allows you to access stuff between scripts
     * 
     *    Basically, instead of importing gameController into everything that needs
     * to use it I can just to GameController.gameController.<whatever> and
     * access all the data I would need.
     */
    public static GameController gameController { get; private set; }
    public float timePassed;

    /*
     * I removed the Timer text change from here. Keep visual stuff separate from
     * your main Game Manager and let other scripts handle that. I moved the Timer
     * to a Timer.cs script, we will likely never need to bother with it again now.
     */

    private void Awake()
    {
        // This basically makes sure there is only one gameController at a time
        // gameController is referred to as a Singleton (because only one can exist at a time)
        if (gameController != null && gameController != this)
        {
            Destroy(this.gameObject); // Destroy duplicate
            return;
        }
        gameController = this;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
    }

    /*
     * Here's an example of a static function. Now we can call GameController.ReloadLevel()
     * from anywhere without needing to reference a game object.
     */
    public static void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
