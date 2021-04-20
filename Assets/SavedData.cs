using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class SavedData : MonoBehaviour
{
    int Score;
    public TextMeshProUGUI ScoreText;

    // Start is called before the first frame update
    private void Awake()
    {
        ScoreText.text = "Your Score: " + Score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GotoStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
