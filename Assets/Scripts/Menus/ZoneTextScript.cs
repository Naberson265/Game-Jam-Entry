using TMPro;
using UnityEngine;

public class ZoneTextScript : MonoBehaviour
{
    private int currentZone = 1;
    private int curentLevel = 1;
    
    private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        ChangeText(GameController.gameController.zone, GameController.gameController.currentLevel+1);
        currentZone = GameController.gameController.zone;
        curentLevel = GameController.gameController.currentLevel +1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameController.zone != currentZone || GameController.gameController.currentLevel + 1 != curentLevel)
        {
            ChangeText(GameController.gameController.zone, GameController.gameController.currentLevel +1);
            currentZone = GameController.gameController.zone;
            curentLevel = GameController.gameController.currentLevel;
        }
    }

    private void ChangeText(int zone, int level)
    {
        text.text = $"Zone {zone}-{level}";
    }
}
