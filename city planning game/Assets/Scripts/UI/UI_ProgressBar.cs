using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ProgressBar : MonoBehaviour
{
    // the bar is moved in two halves to keep alpha values consistent
    [SerializeField]
    private RectTransform LeftBar;

    [SerializeField]
    private RectTransform RightBar;

    [Range(0.0f, 1.0f)]
    public float tempValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LeftBar.sizeDelta = new Vector2((100.0f * tempValue), 100);
        RightBar.sizeDelta = new Vector2((100.0f - LeftBar.rect.width), 100);
    }
}
