using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameOverButtons : MonoBehaviour
{
    public GameObject loading;
    public void restartGame()
    {
        loading.SetActive(true);
        this.GetComponent<Button>().interactable = false;
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        this.GetComponent<Button>().interactable = false;
        Application.Quit(0);
    }
}
