using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AK.Wwise.Event MenuStop;
    public AK.Wwise.Event MusicGame;
    public AK.Wwise.Event StopAll;
    public GameObject wwiseObj;

    public void Start()
    {
        StopAll.Post(wwiseObj);
    }

    public void StartGame()
    {
        MenuStop.Post(wwiseObj);
        MusicGame.Post(wwiseObj);
        SceneManager.LoadScene("Scene 1");

    }

    public void Quit()
    {
        Application.Quit();
    }
}
