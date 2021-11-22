using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    // Actions to be read by other scripts. A single use of Update() function
    public static event Action MousePressed = delegate { };
    public static event Action MouseReleased = delegate { };
    public static event Action<float> MouseScroll = delegate { };

    private static bool isPaused;
    public static bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            if (value)
                MouseReleased.Invoke();

            isPaused = value;
        }
    }

    public static int Scene = 0;


    private void Awake()
    {
        // Singleton Pattern
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!isPaused)
        {
            // Pressed but not Held, only once
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                MousePressed.Invoke();
            }

            // Released after pressed
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                MouseReleased.Invoke();
            }

            float mouseScrollY = Input.mouseScrollDelta.y;
            if (mouseScrollY != 0)
            {
                MouseScroll.Invoke(mouseScrollY);
            }
        }
    }


    // Fuctions for UI to utilize
    public void QuitApp()
    {
        Application.Quit();
    }

    public void GoToNextScene()
    {
        // NextScene is 1 unless Scene is 1, then NextScene is 2.
        // This loops between scenes 1 and 2.
        // 0 leads into 1 using the same logic.
        int nextScene = Scene == 1 ? 2 : 1;
        Scene = nextScene;

        // Scene definitions are in the Build menu.
        // 0 is Main Menu
        // 1 is Bedroom
        // 2 is Garden
        SceneManager.LoadScene(nextScene);
    }
}
