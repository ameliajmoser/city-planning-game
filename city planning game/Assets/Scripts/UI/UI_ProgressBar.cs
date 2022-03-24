using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ProgressBar : MonoBehaviour
{
    // the bar is moved in two halves to keep alpha values consistent
    [SerializeField]
    private RectTransform LeftBar;

    [SerializeField]
    private RectTransform RightBar;

    [Range(0.0f, 1.0f)]
    public float tempValue;

    [SerializeField]
    private GameObject pointsText;

    // Start is called before the first frame update
    void Start()
    {
        pointsText.GetComponent<TMP_Text>().text = "0/100";
    }

    // Update is called once per frame
    void Update()
    {
        LeftBar.sizeDelta = new Vector2((100.0f * tempValue), 100);
        RightBar.sizeDelta = new Vector2((100.0f - LeftBar.rect.width), 100);
    }

    public void SetUIScore( int score )
    {
        tempValue = score / 100.0f;

        // Transform pointsText = transform.Find( "Points Text" );
        // pointsText.gameObject.GetComponent<Text>().text = score + "/100";
        pointsText.GetComponent<TMP_Text>().text = score + "/100";
    }
}
