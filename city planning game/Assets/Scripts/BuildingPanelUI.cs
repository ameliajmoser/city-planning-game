using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Thanks to https://www.youtube.com/watch?v=EpMFOeOMInM&t=1104s
public class BuildingPanelUI : MonoBehaviour
{
    [SerializeField]
    private PlacementController placementController;

    [SerializeField]
    private List<GameObject> buildingPrefabs;

    private void Awake()
    {
        Transform buildingButtonTemplate = transform.Find( "BuildingButtonTemplate" );
        buildingButtonTemplate.gameObject.SetActive( false );

        int index = 0;
        foreach ( GameObject buildingPrefab in buildingPrefabs )
        {
            Transform buildingButtonTransform = Instantiate( buildingButtonTemplate, transform );
            buildingButtonTransform.gameObject.SetActive( true );

            buildingButtonTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2( index * 140, 0 );
            buildingButtonTransform.Find( "Text" ).GetComponent<Text>().text = buildingPrefab.GetComponent<Building>().buildingType.ToString();
        
            buildingButtonTransform.GetComponent<Button>().onClick.AddListener( () => {
                placementController.SetActiveBuildingType( buildingPrefab );
            } );

            index++;
        }
    }
}
