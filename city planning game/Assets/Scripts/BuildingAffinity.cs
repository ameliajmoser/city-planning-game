using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingAffinity
{
    [SerializeField]
    public Building.BuildingType buildingType;

    [SerializeField]
    public int points;

    public BuildingAffinity( Building.BuildingType type, int points )
    {
        this.buildingType = type;
        this.points = points;
    }
}
