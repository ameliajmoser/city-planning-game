using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private UITransitionManager[] transitionList;

    private bool isPaused;
    private bool isTransitioning;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isTransitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            
            if(!isTransitioning) {
                
                if(isPaused) {
                    foreach (UITransitionManager transition in transitionList) {
                        transition.TriggerTransition("Unpause");
                    }
                    isTransitioning = true;
                } else {
                    foreach (UITransitionManager transition in transitionList) {
                        transition.TriggerTransition("Pause");
                    }
                    isPaused = true;
                    isTransitioning = true;
                }
            }
        }
    }

    public void finishPause() {
        isTransitioning = false;
    }

    public void finishUnpause() {
        isPaused = false;
        isTransitioning = false;
    }

    public void ClickQuitButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public bool IsPaused() {
        return isPaused;
    }
}
