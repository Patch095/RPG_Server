using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorBase : MonoBehaviour
{
    public GameObject Blue;
    public GameObject Red;

    // Start is called before the first frame update
    void Start()
    {
        Blue.SetActive(false);
        Red.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.CompareTag("BlueTeam"))
            Blue.SetActive(true);
        else if (this.gameObject.CompareTag("RedTeam"))
            Red.SetActive(true);
    }
}
