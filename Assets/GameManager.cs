using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameRunning;
    private float timer;


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
}
