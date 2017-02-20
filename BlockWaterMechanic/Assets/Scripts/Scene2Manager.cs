using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Manager : MonoBehaviour
{
    private World scWorld;
    private UIController scUI;

    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    // Use this for initialization
    void Start()
    {
        scUI = gameObject.GetComponent<UIController>();
        scWorld = GameObject.Find("World").GetComponent<World>();

        scUI.Scene2();
        scWorld.GenerateWorldScene2();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            Quit();
        }
    }
}