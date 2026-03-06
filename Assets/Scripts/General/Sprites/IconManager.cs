using UnityEngine;

public class IconManager : MonoBehaviour
{
    public static IconManager iconManager { get; private set; }
    public Sprite[] powerupIcons;
    private void Awake()
    {
        if (iconManager != null && iconManager != this)
        {
            Destroy(this.gameObject); // Destroy duplicate
            return;
        }
        iconManager = this;
    }
}
