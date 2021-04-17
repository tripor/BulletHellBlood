using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameRunning;
    private float timer;

    public List<GameObject> enemyShips;

    public List<GameObject> friendlyShips;

    public TextMeshProUGUI scoreText;
    public GameObject nonXrCanvas;
    public Slider slider;

    private int score = 0;
    private bool usingVr = false;
    public int maxLife = 200;
    private int life = 200;


    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance);
        }
        GameManager.Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameRunning = false;
        timer = 0;
        scoreText.text = "Score: " + score;
        slider.maxValue = maxLife;
        slider.value = maxLife;
        life = maxLife;
        if (XRSettings.isDeviceActive)
        {
            usingVr = true;
            nonXrCanvas.SetActive(false);
        }
        else
        {
            usingVr = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            timer += Time.deltaTime;
            if (timer > 10)
            {
                gameRunning = false;
                timer = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                continueGame();
            }
        }
    }

    public void continueGame()
    {
        if (!gameRunning)
        {
            gameRunning = true;
        }
    }

    public void addScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void removeLife()
    {
        life--;
        slider.value = life;
        if (life == 0)
        {
            endGame();
        }
    }

    public void endGame()
    {

    }
}
