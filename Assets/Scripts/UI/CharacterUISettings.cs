using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUISettings : MonoBehaviour
{
    public BaseClass Owner;

    TextMeshProUGUI characaterName;

    Image HPBar;
    TextMeshProUGUI HPvalues;

    Image MPBar;
    TextMeshProUGUI MPvalues;

    // Start is called before the first frame update
    void Start()
    {
        Transform characterInfoPanel = this.transform.GetChild(0);
        characaterName = characterInfoPanel.GetComponent<TextMeshProUGUI>();

        //Player name Init       
        characaterName.text = Owner.Name;
        if (characaterName.text == "Default")
            characaterName.text += "_" + Owner.ClassName;

        //HP Bar Init
        Transform HPinfo = characterInfoPanel.GetChild(0);
        HPBar = HPinfo.GetChild(0).GetComponentInChildren<Image>();
        HPvalues = HPinfo.GetComponentInChildren<TextMeshProUGUI>();

        //Mp Bar Init
        Transform MPinfo = characterInfoPanel.GetChild(1);
        MPBar = MPinfo.GetChild(0).GetComponent<Image>();
        MPvalues = MPinfo.GetComponentInChildren<TextMeshProUGUI>();

        //ATB bar Init
        Transform ATBbarBackground = this.transform.GetChild(1);
        Owner.GetFSM().CharacterPannel = this.transform.GetComponent<Image>();
        Owner.GetFSM().ATBbar = ATBbarBackground.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float hpFillGauge = Owner.CurrentHp / Owner.MaxHp;
        HPvalues.text = Owner.CurrentHp + " / " + Owner.MaxHp;
        HPBar.transform.localScale = new Vector3(Mathf.Clamp01(hpFillGauge), 1f, 1f);
        if (hpFillGauge > 0.6f)
            HPBar.color = Color.green;
        else if (hpFillGauge < 0.2f)
            HPBar.color = Color.red;
        else
            HPBar.color = Color.yellow;

        float mpFillGauge = Owner.CurrentMp / Owner.MaxMp;
        MPvalues.text = Owner.CurrentMp + " / " + Owner.MaxMp;
        MPBar.transform.localScale = new Vector3(Mathf.Clamp01(mpFillGauge), 1f, 1f);
        if (mpFillGauge > 0.6f)
            MPBar.color = Color.blue;
        else if (mpFillGauge < 0.2f)
            MPBar.color = Color.grey;
        else
            MPBar.color = Color.magenta;
    }
}
