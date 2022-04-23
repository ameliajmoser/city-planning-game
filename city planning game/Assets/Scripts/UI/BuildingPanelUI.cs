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

    List<Transform> buttons;
    Transform buildingButtonTemplate;

    [SerializeField]
    private int MAX_INVENTORY_SIZE = 7;

    private void Awake()
    {
        buttons = new List<Transform>();

        buildingButtonTemplate = transform.Find( "BuildingButtonTemplate" );
        buildingButtonTemplate.gameObject.SetActive( false );

        // int index = 0;
        // foreach ( GameObject buildingPrefab in buildingPrefabs )
        // {
        //     if ( index > MAX_INVENTORY_SIZE - 1 )
        //     {
        //         break;
        //     }

        //     addButton( buildingPrefab ); 
        //     index++;
        // }
    }

    private void addButton( GameObject buildingPrefab )
    {
        Transform buildingButtonTransform = Instantiate( buildingButtonTemplate, transform );
        buildingButtonTransform.gameObject.SetActive( true );
        buttons.Add( buildingButtonTransform );

        buildingButtonTransform.Find( "Text" ).GetComponent<Text>().text = buildingPrefab.GetComponent<Building>().ToString();
    
        buildingButtonTransform.GetComponent<Button>().onClick.AddListener( () => {
            placementController.SetActiveBuildingType( buildingPrefab, buildingButtonTransform );
        } );
    }

    public void removeButton( Transform transform )
    {
        Destroy( transform.gameObject );
    }

    public void addRandomBuilding()
    {
        int randInt = Random.Range( 0, buildingPrefabs.Count - 1 );
        GameObject prefab = buildingPrefabs[randInt];

        addButton( prefab );
    }

    public bool addBuildingSet( List<GameObject> prefabs )
    {
        bool allAdded = false;

        if ( buttons.Count + prefabs.Count <= MAX_INVENTORY_SIZE )
        {
            foreach ( GameObject prefab in prefabs )
            {
                addButton( prefab );
            }

            allAdded = true;
        }

        return ( allAdded );
    }

    public bool insertFromSet( List<GameObject> prefabs )
    {
        bool hasAdded = false;

        while ( numInInventory() <= MAX_INVENTORY_SIZE && prefabs.Count > 0 )
        {
            addButton( prefabs[0] );
            prefabs.RemoveAt( 0 );

            hasAdded = true;
        }

        return ( hasAdded );
    }

    public int numInInventory()
    {
        return ( buttons.Count );
    }

    public void clearInventory()
    {
        foreach( Transform transform in buttons )
        {
            if ( transform )
            {
                Destroy( transform.gameObject );
            }
        }

        buttons = new List<Transform>();
    }
}
