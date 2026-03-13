using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0f);
    }
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.75f);
    }
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Sadly Buttons can only have one argument so I did it this way
    public void OpenLevel(string levelId)
    {
        string[] levelValues = levelId.Split("-");
        ProgressionManager.LoadLevel(int.Parse(levelValues[1]) - 1, "Zone" + levelValues[0]);
    }
}
