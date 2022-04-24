using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    
    [SerializeField]
    private TMP_Text bodyText;

    [SerializeField]
    private GameObject characterArt;

    [SerializeField]
    private UITransitionManager[] transitionList;

    private List<string> slides;
    private bool dialogueOpen;
    private int currentSlide;

    // Start is called before the first frame update
    void Start()
    {
        dialogueOpen = false;
        currentSlide = 0;
    }

    public void QueueDialoguePopup( Character character, string key ) {
        Character.Dialogue dialogue = character.GetDialogue(key);
        nameText.text = dialogue.name;

        characterArt.GetComponent<Image>().sprite = character.GetCharacterArt();

        slides = new List<string>(dialogue.slides);
        bodyText.text = slides[0];
        currentSlide = 0;
        foreach(UITransitionManager transition in transitionList) {
            transition.TriggerTransition("StartDialogue");
        }
    }

    public void DialogueOpened() {
        dialogueOpen = true;
    }

    void Update() { // I'd like to move this out of update into like a coroutine or something but it's fine for now
        if (dialogueOpen) {
            if(Input.anyKeyDown) {
                if (currentSlide < slides.Count - 1) {
                    currentSlide++;
                    bodyText.text = slides[currentSlide];
                } else {
                    foreach(UITransitionManager transition in transitionList) {
                        transition.TriggerTransition("EndDialogue");
                        dialogueOpen = false;
                    }
                }
            }
        }
    }

    public bool IsOpen()
    {
        return ( dialogueOpen );
    }
}
