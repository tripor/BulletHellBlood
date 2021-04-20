using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameOverScore : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("lastscore", 0);
        int bestScore = PlayerPrefs.GetInt("bestscore", 0);
        if (lastScore > bestScore)
        {
            bestScore = lastScore;
            PlayerPrefs.SetInt("bestscore", bestScore);
            text.text = "New best score " + bestScore + "!!";
        }
        else
        {
            text.text = "Best Score: " + bestScore + "\nYour Score: " + lastScore;
        }
    }
}
