using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton(string scenename)
    {
        Debug.Log("StartButton called with scene: " + scenename);
        SceneManager.LoadScene(scenename);
    }


    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit button pressed...");
    }

    public void LoadScene(string scenename)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Current active scene: " + currentScene.name);

        SceneManager.LoadScene(scenename);
    }
}
