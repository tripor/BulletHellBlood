using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startGame : MonoBehaviour
{
    public GameObject loading;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void startGameplay()
    {
        this.GetComponent<Button>().interactable = false;
        loading.SetActive(true);
        loading.GetComponent<TextMeshProUGUI>().text = "Loading...";
        SceneManager.LoadScene(1);
    }
    public void exitGame()
    {
        this.GetComponent<Button>().interactable = false;
        Application.Quit(0);
    }
}
