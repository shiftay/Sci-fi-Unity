using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GalaxyInfo {
    public string name;
    public string rulingfaction;
    public PlanetInfo[] planets;

    public static GalaxyInfo FromJSON(string json) {
        return JsonUtility.FromJson<GalaxyInfo>(json);
    }

}


[System.Serializable]
public class PlanetInfo {
    public string name;
    public CityInfo[] cities;
}

[System.Serializable]
public class CityInfo {

    public string name;
    public bool hasBountyOffice;

}
