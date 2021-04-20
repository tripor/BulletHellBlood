using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
public class StartingScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //PlayerPrefs.SetInt(score);
        SceneManager.LoadScene("GameOverScene");
    }
}
