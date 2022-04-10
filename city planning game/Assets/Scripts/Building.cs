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

    [SerializeField]
    public int cost = 2;

    // Building object data
    public Rigidbody rigidbody;
    public GameObject radius;
    public TMP_Text points;
    public GameObject indicator;

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

    private List<GameObject> activeBuildings;

    void Awake()
    {
        buildingID = System.Guid.NewGuid().ToString();
        currState = PlacementState.Hover;
        currPoints = 0;
        numCollisions = 0;
        
        indicator.SetActive( false );
        activeBuildings = new List<GameObject>();

        // Set radius of sensing radius
        radius.transform.localScale = new Vector3( affinityRadius * 2, affinityRadius * 2, 0 );
        if (startPlaced){
            this.PlaceBuilding();
        }
    }

    // TODO: if building is in hover state, constantly check for surrounding buildings and update points value
    void Update()
    {
        if ( currState == PlacementState.Hover )
        {
            currPoints = ComputeScore();
            points.text = currPoints.ToString();
        }
    }

    private int ComputeScore()
    {
        int score = 0;
        int layerMask = LayerMask.GetMask("Buildings");

        // New buildings list
        List<GameObject> newActiveBuildings = new List<GameObject>();
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, affinityRadius, layerMask);
        
        foreach (var hitCollider in hitColliders)
        {
            if ( hitCollider.gameObject != transform.gameObject )
            {
                GameObject hitBuilding = hitCollider.gameObject;
                newActiveBuildings.Add( hitBuilding );
                
                BuildingType type = hitBuilding.GetComponent<Building>().buildingType;
                score += GetBuildingAffinity( type );
            }
        }

        // We can probably do this real inefficiently cuz the number of buildings is so small I hope
        foreach (var hitBuilding in activeBuildings)
        {
            hitBuilding.GetComponent<Building>().indicator.SetActive( false );
        }

        foreach (var hitBuilding in newActiveBuildings)
        {
            hitBuilding.GetComponent<Building>().indicator.SetActive( true );
        }

        activeBuildings = newActiveBuildings;

        score -= cost;

        return ( score );
    }

    private int GetBuildingAffinity( BuildingType type )
    {
        foreach( var buildingAffinity in affinities )
        {
            if ( buildingAffinity.buildingType == type )
            {
                return ( buildingAffinity.points );
            }
        }

        return ( 0 );
    }

    void OnGUI() {
        points.transform.LookAt(Camera.main.transform);
        indicator.transform.LookAt(Camera.main.transform);
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

    public int PlaceBuilding()
    {
        radius.SetActive( false );
        points.enabled = false;
        currState = PlacementState.Placed;
        currPoints = ComputeScore();

        foreach (var hitBuilding in activeBuildings)
        {
            hitBuilding.GetComponent<Building>().indicator.SetActive( false );
        }

        return ( currPoints );
    }

    public List<GameObject> getActiveBuildings()
    {
        return ( activeBuildings );
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

    public override string ToString() {
        string enumString = buildingType.ToString();
        List<int> spaceIndeces = new List<int>();
        for (int i = 1; i < enumString.Length; i++) {
            if(enumString[i] >= 'A' && enumString[i] <= 'Z')
            spaceIndeces.Add(i);
        }
        spaceIndeces.Add(enumString.Length);
        string resultString = enumString;
        if(spaceIndeces.Count > 0) {
            resultString = enumString.Substring(0, spaceIndeces[0]);
            for (int i = 1; i < spaceIndeces.Count; i++) {
                resultString = resultString + " " + enumString.Substring(spaceIndeces[i-1], spaceIndeces[i] - spaceIndeces[i-1]);
            }
        }
        return resultString;
    }
}
