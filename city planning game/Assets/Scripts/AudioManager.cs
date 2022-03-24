using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public FMODUnity.EventReference musicEventRef;
    public FMODUnity.EventReference birdsEventRef;

    public GameObject birdsObject;

    private FMOD.Studio.EventInstance birdsEvent;
    private FMOD.Studio.EventInstance musicEvent;


    // Start is called before the first frame update
    void Start()
    {
        musicEvent = FMODUnity.RuntimeManager.CreateInstance(musicEventRef);
        musicEvent.start();
        birdsEvent = FMODUnity.RuntimeManager.CreateInstance(birdsEventRef);
        birdsEvent.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(birdsEvent, birdsObject.GetComponent<Transform>());
    }

    void OnDisable()
    {
        musicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        birdsEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
