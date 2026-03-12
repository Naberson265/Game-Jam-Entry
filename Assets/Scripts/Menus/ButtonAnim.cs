using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonAnim : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector2 destinationPos;
    public float timeUntilAnim = 1f;

    void Start()
    {
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
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, destinationPos, Time.deltaTime);
        }
    }
}