using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Building : MonoBehaviour
{
    public string buildingID;

    [SerializeField]
    private bool startPlaced = false;
    //dimensions of building
    [SerializeField]
    public float affinityRadius = 0;
    
    [SerializeField]
    public List<BuildingAffinity> affinities = new List<BuildingAffinity>();

    // Building object data
    public Rigidbody rigidbody;
    public GameObject radius;
    public TMP_Text points;

    private int currPoints;
    private int numCollisions;

    [SerializeField]
    public BuildingType buildingType = BuildingType.Default;
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
        numCollisions = 0;

        // Set radius of sensing radius
        radius.transform.localScale = new Vector3( affinityRadius, affinityRadius, 0 );
        if (startPlaced){
            this.PlaceBuilding();
        }
    }

    // TODO: if building is in hover state, constantly check for surrounding buildings and update points value
    
    void OnGUI() {
        points.transform.LookAt(Camera.main.transform);
    }
    void OnCollisionEnter()
    {
        numCollisions++;
        // Debug.Log(numCollisions);
    }

    void OnCollisionExit()
    {
        numCollisions--;
        // Debug.Log(numCollisions);
    }

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

    public bool isColliding() {
        if(numCollisions > 0) {
            return true;
        }
        return false;
    }

    public String ToString() {
        String enumString = buildingType.ToString();
        List<int> spaceIndeces = new List<int>();
        for (int i = 1; i < enumString.Length; i++) {
            if(enumString[i] >= 'A' && enumString[i] <= 'Z')
            spaceIndeces.Add(i);
        }
        spaceIndeces.Add(enumString.Length);
        String resultString = enumString;
        if(spaceIndeces.Count > 0) {
            resultString = enumString.Substring(0, spaceIndeces[0]);
            for (int i = 1; i < spaceIndeces.Count; i++) {
                resultString = resultString + " " + enumString.Substring(spaceIndeces[i-1], spaceIndeces[i] - spaceIndeces[i-1]);
            }
        }
        return resultString;
    }
}
