using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DelaySceneLoadTrigger : MonoBehaviour
{
    public float loadDelay = 5f;
    public string sceneToLoad = "Zone1";
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DelayScene());
        }
    }
    private IEnumerator DelayScene()
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
