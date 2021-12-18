using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyManager : MonoBehaviour
{

    public TextAsset galaxyAsset;
    public GalaxyInfo galaxyInfo;

    private void Awake() {
        // Debug.Log(galaxyAsset.ToString());

        galaxyInfo = GalaxyInfo.FromJSON(galaxyAsset.ToString());
    }



    public PlanetInfo GetPlanet(string name) {
        for(int i = 0; i < galaxyInfo.planets.Length; i++) 
        {
            if(galaxyInfo.planets[i].name == name) return galaxyInfo.planets[i];
        }
        

        return null;
    }
}
