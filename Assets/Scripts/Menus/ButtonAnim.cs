using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonAnim : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector2 destinationPos;
    public float timeUntilAnim = 1f;
    public float speed = 0.1f;

    void Start()
    {
        Time.timeScale = 1f;
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (timeUntilAnim > 0f)
        {
            timeUntilAnim -= Time.deltaTime;
        }
        else
        {
            timeUntilAnim -= Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, destinationPos, speed);
        }
    }
}