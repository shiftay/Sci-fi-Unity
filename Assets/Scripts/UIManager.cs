using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
   
    [SerializeField] private CanvasGroup eventDescCG;
    [SerializeField] private CanvasGroup planetDescCG;
    [SerializeField] private CanvasGroup EventPopUpCG;

    [SerializeField] private Animator _descriptorAnimator;

// ----- Planet Descriptor Variables ------
    public TextMeshProUGUI planetName;
    public TextMeshProUGUI planetDesc;
    public List<TextMeshProUGUI> citieNames;
    public List<Image> ammenities;
// ----- Planet Descriptor Variables ------
    public void SetupPlanetDescription(PlanetInfo planet) {

        if(planet != null) {
            planetDescCG.alpha = 0;

            planetName.text = planet.name;
            // planetDesc.text = "";

            // bool m_hasOffice = false;

            // for(int i = 0; i < planet.cities.Length; i++) {

            // }
        }

        _descriptorAnimator.SetTrigger("Open");
    }

    public void PlanetOpenCB() {

    }

    public void PlanetClose() {

    }


}
