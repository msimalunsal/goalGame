using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{


    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
    public void Exit()
    {
        Debug.Log("Çıkış Yapıldı");
        Application.Quit();
    }

}
