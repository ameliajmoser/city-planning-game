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
        Debug.Log("play");
        foreach(UITransitionManager transition in transitionList)
        {
            transition.TriggerTransition("Play");
        }
    }

    public void ClickOptionsButton()
    {
        Debug.Log("options");
        foreach(UITransitionManager transition in transitionList)
        {
            transition.TriggerTransition("Options");
        }
    }

    public void ClickBackButton()
    {
        Debug.Log("back");
        foreach(UITransitionManager transition in transitionList)
        {
            transition.TriggerTransition("Back");
        }
    }

    public void ClickQuitButton()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void LoadPlayScene()
    {
        Debug.Log("load");
        SceneManager.LoadScene("SampleScene");
    }
}
