using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    public Transform landscape;
    public GameObject skipText;
    public RectTransform scrollText;
    public float timeUntilScroll;
    // When the text stops scrolling.
    public float scrollLimit;
    public float scrollSpeed;
    private DitherTransition ditherer;
    void Start()
    {
        ditherer = FindFirstObjectByType<DitherTransition>();
        ditherer.StartAnim("End");
    }

    void Update()
    {
        if (timeUntilScroll <= timeUntilScroll - 1f)
        {
            // Starts music the moment the dither animation is done
            GetComponent<AudioSource>().enabled = true;
        }
        if (timeUntilScroll <= 0)
        {
            skipText.SetActive(true);
            timeUntilScroll = 0;
            if (scrollText.anchoredPosition.y < scrollLimit)
            {
                float scrollMultiplier = 1;
                if (Input.GetButton("Jump"))
                {
                    scrollMultiplier = 10;
                }
                scrollText.anchoredPosition += new Vector2(0f, scrollSpeed * scrollMultiplier * Time.deltaTime);
            }
            else
            {
                scrollText.anchoredPosition = new Vector2(0f, scrollLimit);
            }
            if (Input.GetButtonDown("Ability"))
            {
                // Press the Q key when it starts scrolling to exit.
                StartCoroutine(CreditsEnd());
            }
        }
        else
        {
            timeUntilScroll -= Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        // Static objects like the water don't move.
        landscape.localPosition = new Vector3(0f, Mathf.Sin((float)Time.frameCount * 0.025f) / 2f, 0f);
    }
    public IEnumerator CreditsEnd()
    {
        timeUntilScroll = 99f;
        ditherer.StartAnim("Start");
        skipText.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TitleScreen");
    }
}
