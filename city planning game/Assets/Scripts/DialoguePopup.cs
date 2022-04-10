using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    
    [SerializeField]
    private TMP_Text bodyText;

    private UITransitionManager firstTransition;

    [SerializeField]
    private UITransitionManager secondTransition;

    private bool dialogueOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        firstTransition = GetComponent<UITransitionManager>();
        QueueDialoguePopup();
    }

    public void QueueDialoguePopup( /*Character character*/ ) {
        // nameText.text = character.characterName;
        // bodyText.text = dialogueString;
        firstTransition.TriggerTransition("StartDialogue");
        dialogueOpen = true;

    }

    void Update() { // I'd like to move this out of update into like a coroutine or something but it's fine for now
        if (dialogueOpen) {
            if(Input.anyKey) {
                // if more dialogue
                    // change text
                // else
                secondTransition.TriggerTransition("EndDialogue");
                dialogueOpen = false;
            }
        }
    }
}
