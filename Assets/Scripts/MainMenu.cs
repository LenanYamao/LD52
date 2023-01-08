using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string Scene = "";
    public void Play()
    {
        Upgrades.attackSpeed = 0;
        Upgrades.damage = 0;
        Upgrades.speed = 0;
        SceneManager.LoadScene(Scene);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
