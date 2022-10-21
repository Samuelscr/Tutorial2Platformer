using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController control;
    public int score;


    void Awake()
    {
        if(control ==null)
        {
            control = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (control !=this)
        {
            Destroy(gameObject);
        }
    }
    public void ResetTheGame()
    {
        GameController.control.score = 0;
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Button Pressed");
    }
}
