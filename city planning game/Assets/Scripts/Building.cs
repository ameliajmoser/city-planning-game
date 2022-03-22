using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Building : MonoBehaviour
{
    public string buildingID;

    //dimensions of building
    [SerializeField]
    public float affinityRadius = 0;
    
    [SerializeField]
    public List<BuildingAffinity> affinities = new List<BuildingAffinity>();

    // Building object data
    public GameObject radius;
    public TMP_Text points;

    private int currPoints;

    [SerializeField]
    public BuildingType building = BuildingType.Default;
    public enum BuildingType
    {
        Default,
        HouseSmall,
        HouseSuburb,
        Factory,
        ApartmentLuxury,
        ApartmentLowIncome,
        Office,
        School,
        CityHall,
    }

    private enum PlacementState
    {
        Hover,
        Placed,
    }

    private PlacementState currState;

    void Awake()
    {
        buildingID = System.Guid.NewGuid().ToString();
        currState = PlacementState.Hover;
        currPoints = 0;

        radius.SetActive( true );

        // TODO: fix text
        points.enabled = true;

        // Set radius of sensing radius
        radius.transform.localScale = new Vector3( affinityRadius, affinityRadius, 0 );
    }

    // TODO: create update method
    // TODO: if building is in hover state, constantly check for surrounding buildings and update points value

    public void PlaceBuilding()
    {
        radius.SetActive( false );
        points.enabled = false;

        // TODO: calculate points from buildings within radius
    }

    public int getPoints()
    {
        return ( currPoints );
    }
}
