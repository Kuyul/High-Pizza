using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    //Declare public variables
    public GameObject pizza;
    public PlayerControl Player;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GameOver()
    {
        StartCoroutine(WaitBeforeDeath());
    }

    IEnumerator WaitBeforeDeath()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }

    public void AddRight()
    {
        Player.AddRight();
    }

    public void AddLeft()
    {
        Player.AddLeft();
    }

    public int GetPlayerUnbalance()
    {
        return Player.GetUnbalance();
    }
}
