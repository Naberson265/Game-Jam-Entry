using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenScript : MonoBehaviour
{
    /*  Custom splash screen tool for all Squared Production games. Disable the native splash screen,
    add this as the first scene in the scene list, and add in any splash screen images you want. */
    [SerializeField] private Sprite[] splashImages;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image splashDisplay;
    [SerializeField] private Color bgTargetColor = Color.grey;
    [SerializeField] private float splashVisTime = 2f;
    [SerializeField] private float splashFadeTime = 0.5f;
    [SerializeField] private float bgFadeTime = 1f;
    [SerializeField] private string nextScene;
    private AudioSource aud;
    private int currentSplash = 0;
    private IEnumerator Start()
    {
        aud = GetComponent<AudioSource>();
        currentSplash = 0;
        // The color will transition to the target color over the first second of the scene.
        float colorTime = 0f;
        while (colorTime < bgFadeTime)
        {
            colorTime += Time.deltaTime;
            bgImage.color = Color.Lerp(Color.black, bgTargetColor, colorTime / bgFadeTime);
            yield return null;
        }
        aud.enabled = true;
        IEnumeratorBridge1();
    }
    private void IEnumeratorBridge1() => StartCoroutine(LoadSplash());
    private IEnumerator LoadSplash()
    {
        splashDisplay.sprite = splashImages[currentSplash];
        float fadeTime = 0f;
        while (fadeTime < splashFadeTime)
        {
            fadeTime += Time.deltaTime;
            splashDisplay.color = Color.Lerp(Color.clear, Color.white, fadeTime / splashFadeTime);
            yield return null;
        }
        yield return new WaitForSeconds(splashVisTime);
        while (fadeTime > 0f)
        {
            fadeTime -= Time.deltaTime;
            splashDisplay.color = Color.Lerp(Color.clear, Color.white, fadeTime / splashFadeTime);
            yield return null;
        }
        currentSplash++;
        if (currentSplash < splashImages.Length) IEnumeratorBridge1();
        else IEnumeratorBridge2();
    }
    private void IEnumeratorBridge2() => StartCoroutine(SplashEnd());
    private IEnumerator SplashEnd()
    {
        float colorTime = 0f;
        while (colorTime < bgFadeTime)
        {
            colorTime += Time.deltaTime;
            bgImage.color = Color.Lerp(bgTargetColor, Color.black, colorTime / bgFadeTime);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }
}
