using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMixerBar : MonoBehaviour
{
    [SerializeField] private string busPath;

    FMOD.Studio.Bus bus;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus(busPath);
        slider = transform.GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("FMOD Bus-" + busPath, 0.5f);
        bus.setVolume(slider.value);
    }

    public void ChangeMixerBusValue()
    {
        bus.setVolume(slider.value);
        PlayerPrefs.SetFloat("FMOD Bus-" + busPath, slider.value);
    }
}
