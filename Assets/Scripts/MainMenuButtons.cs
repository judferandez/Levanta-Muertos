using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{

    public void SinglePlayer(){
        SceneManager.LoadScene("Game");
    }

    public void Multiplayer(){
        SceneManager.LoadScene("Loading");
    }
}
