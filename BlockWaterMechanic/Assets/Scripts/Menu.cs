/* */
/*
/* Script to manage the Menu Scene
/*
/*
/* */
using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject cScene1, cScene2, cQuit;
    public GameObject cSelectionSphereL;
    public GameObject cSelectionSphereR;

    public int nChoice;
    private bool bKeyInput = true;

    void Start()
    {
        nChoice = 0;
        cScene1.GetComponent<Renderer>().material.color = Color.yellow;
        cSelectionSphereL.transform.position = (new Vector2(-4f, 7f));
        cSelectionSphereR.transform.position = (new Vector2(4f, 7f));
    }

    public void OnScene1()
    {
        nChoice = 0;
        cScene2.GetComponent<Renderer>().material.color = Color.white;
        cQuit.GetComponent<Renderer>().material.color = Color.white;

        cScene1.GetComponent<Renderer>().material.color = Color.yellow;
        cSelectionSphereL.transform.position = (new Vector2(-4f, 7f));
        cSelectionSphereR.transform.position = (new Vector2(4f, 7f));

        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
    }

    public void OnScene2()
    {
        nChoice = 1;
        cScene1.GetComponent<Renderer>().material.color = Color.white;
        cQuit.GetComponent<Renderer>().material.color = Color.white;

        cScene2.GetComponent<Renderer>().material.color = Color.yellow;
        cSelectionSphereL.transform.position = (new Vector2(-5f, -1f));
        cSelectionSphereR.transform.position = (new Vector2(5f, -1f));

        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene2");
    }

    public void OnQuit()
    {
        nChoice = 2;
        cScene1.GetComponent<Renderer>().material.color = Color.white;
        cScene2.GetComponent<Renderer>().material.color = Color.white;

        cQuit.GetComponent<Renderer>().material.color = Color.yellow;
        cSelectionSphereL.transform.position = (new Vector2(-3f, -6f));
        cSelectionSphereR.transform.position = (new Vector2(3f, -6f));

        Application.Quit();
    }

    void Update()
    {
        float fAxis = Input.GetAxis("Vertical");
        if (Mathf.Abs(fAxis) > 0.1f)
        {
            if (!bKeyInput)
            {
                bKeyInput = true;
                if (fAxis > 0)
                {
                    --nChoice;
                    if (nChoice < 0)
                        nChoice = 2;
                }
                else
                {
                    ++nChoice;
                    if (nChoice > 2)
                        nChoice = 0;
                }
                cScene1.GetComponent<Renderer>().material.color = Color.white;
                cScene2.GetComponent<Renderer>().material.color = Color.white;
                cQuit.GetComponent<Renderer>().material.color = Color.white;
                switch (nChoice)
                {
                    case 0:
                        {
                            cScene1.GetComponent<Renderer>().material.color = Color.yellow;
                            cSelectionSphereL.transform.position = (new Vector2(-4f, 7f));
                            cSelectionSphereR.transform.position = (new Vector2(4f, 7f));
                            break;
                        }
                    case 1:
                        {
                            cScene2.GetComponent<Renderer>().material.color = Color.yellow;
                            cSelectionSphereL.transform.position = (new Vector2(-5f, -1f));
                            cSelectionSphereR.transform.position = (new Vector2(5f, -1f));
                            break;
                        }
                    case 2:
                        {
                            cQuit.GetComponent<Renderer>().material.color = Color.yellow;
                            cSelectionSphereL.transform.position = (new Vector2(-3f, -6f));
                            cSelectionSphereR.transform.position = (new Vector2(3f, -6f));
                            break;
                        }
                }
            }
        }
        else
            bKeyInput = false;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            switch (nChoice)
            {
                case 0:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
                        break;
                    }
                case 1:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene2");
                        break;
                    }
                case 2:
                    {
                        Application.Quit();
                        break;
                    }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}