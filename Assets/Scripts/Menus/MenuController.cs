using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button targetBackButton;
    void Start() => RenderSettings.skybox.SetFloat("_Rotation", 0f);
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.75f);
        if (Input.GetButtonDown("Cancel"))
        {
            LastPage();
        }
    }
    public void SetBackButton(Button setButton) => targetBackButton = setButton;
    public void LastPage()
    {
        if (targetBackButton != null)
        {
            EventSystem.current.SetSelectedGameObject(targetBackButton.gameObject);
            targetBackButton.onClick.Invoke();
        }
    }
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenWebsite(string linkToOpen)
    {
        Application.OpenURL(linkToOpen);
    }
    public void CloseGame()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
		    Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }

    // Sadly Buttons can only have one argument so I did it this way
    public void OpenLevel(string levelId)
    {
        string[] levelValues = levelId.Split("-");
        ProgressionManager.LoadLevel(int.Parse(levelValues[1]) - 1, int.Parse(levelValues[0]));
    }

    public void Continue()
    {
        ProgressionManager.LoadLevel(ProgressionManager._saveData.latestCheckpoint.levelNum, ProgressionManager._saveData.latestCheckpoint.zoneNum);
    }

    public void NewGame()
    {
        ProgressionManager.ResetSave();
        ProgressionManager.LoadLevel(0, 1);
    }
}
