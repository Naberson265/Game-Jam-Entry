using UnityEngine;
using UnityEngine.UI;

public class BackButtonSwitcher : MonoBehaviour
{
    private MenuController mc;
    void Start() => mc = FindFirstObjectByType<MenuController>();
    void Update() => mc.targetBackButton = GetComponent<Button>();
}
