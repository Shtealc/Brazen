using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fun : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("mainMenu");
    }
}