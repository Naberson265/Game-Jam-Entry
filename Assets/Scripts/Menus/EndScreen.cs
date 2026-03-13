using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ZoneEndScreen : MonoBehaviour
{
    // The first is the actual colored text, second is the "shadow".
    public TMP_Text[] finalRankTexts;
    public TMP_Text[] levelRankTexts;
    void Start()
    {
        GameController gc = GameController.gameController;
        foreach (TMP_Text frText in finalRankTexts)
        {
            if (gc.finalRank == 0)
            {
                frText.text = "S RANK!!!";
                if (frText.transform.name != "ZoneRankShadow") frText.color = Color.yellow;
            }
            else if (gc.finalRank < 4)
            {
                if (gc.finalRank == 1)
                {
                    frText.text = "A RANK!";
                    if (frText.transform.name != "ZoneRankShadow") frText.color = Color.green;
                }
                else if (gc.finalRank == 2) 
                {
                    frText.text = "B RANK!";
                    if (frText.transform.name != "ZoneRankShadow") frText.color = Color.magenta;
                }
                else if (gc.finalRank == 3)
                {
                    frText.text = "C RANK!";
                    if (frText.transform.name != "ZoneRankShadow") frText.color = Color.cyan;
                }
            }
            else
            {
                frText.text = "D RANK...";
                if (frText.transform.name != "ZoneRankShadow") frText.color = Color.brown;
            }
        }
        int currentLRCount = 0;
        foreach (TMP_Text levelText in levelRankTexts)
        {
            if (gc.levelRanks.Count < currentLRCount)
            {
                string rankLetter;
                if (gc.levelRanks[currentLRCount] == 0) rankLetter = "S";
                else if (gc.levelRanks[currentLRCount] == 1) rankLetter = "A";
                else if (gc.levelRanks[currentLRCount] == 2) rankLetter = "B";
                else if (gc.levelRanks[currentLRCount] == 3) rankLetter = "C";
                else rankLetter = "D";
                levelText.text = "Level " + gc.zone.ToString() + "-" + (currentLRCount + 1).ToString()
                + ": " + rankLetter.ToString();
                currentLRCount++;
            }
            else
            {
                levelText.text = "Level " + gc.zone.ToString() + "-" +
                (currentLRCount + 1).ToString() + ": D";
                currentLRCount++;
            }
        }
    }
}
