using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public int buildingID;

    //dimensions of building
    public float width = 0; 
    public float length = 0;
    public float affinityRadius = 0;
    public Dictionary<Building, int> affinities = new Dictionary<Building, int>();

    public BuildingType building = BuildingType.Default;
    public enum BuildingType
    {
        Default,
        House,
        CityHall,

    }
}
