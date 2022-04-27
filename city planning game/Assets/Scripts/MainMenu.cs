using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private UITransitionManager[] transitionList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ClickPlayButton()
    {
        foreach(UITransitionManager transition in transitionList)
        {
            transition.TriggerTransition("Play");
        }
    }

    public void ClickOptionsButton()
    {
        foreach(UITransitionManager transition in transitionList)
        {
            transition.TriggerTransition("Options");
        }
    }

    public void ClickBackButton()
    {
        foreach(UITransitionManager transition in transitionList)
        {
            transition.TriggerTransition("Back");
        }
    }

    public void ClickQuitButton()
    {
        Application.Quit();
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
