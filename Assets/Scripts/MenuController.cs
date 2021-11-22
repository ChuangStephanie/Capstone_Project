using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel = null;
    // [SerializeField] private Animator characterAnim = null;

    public void ShowPanel()
    {
        // TODO: Slide From Above

        // Workaround: Appear!
        rootPanel.SetActive(true);

        // Pause Animation
        GameManager.IsPaused = true;
        // characterAnim.speed = 0;
    }

    public void HidePanel()
    {
        // TODO: Slide into Above

        // Workaround: Disappear!
        rootPanel.SetActive(false);

        // Resume Animation
        GameManager.IsPaused = false;
        // characterAnim.speed = 1;

    }
}
