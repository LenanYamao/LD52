using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradesMenu : MonoBehaviour
{
    public string Scene = "";
    public void AttackSpeed()
    {
        Upgrades.attackSpeed++;
        SceneManager.LoadScene(Scene);
    }
    public void Damage()
    {
        Upgrades.damage++;
        SceneManager.LoadScene(Scene);
    }
    public void Speed()
    {
        Upgrades.speed++;
        SceneManager.LoadScene(Scene);
    }
}
