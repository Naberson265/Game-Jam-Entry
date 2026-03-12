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
}
