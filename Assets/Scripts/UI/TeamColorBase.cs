using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorBase : MonoBehaviour
{
    public GameObject Blue;
    public GameObject Red;

    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        Blue.SetActive(false);
        Red.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.CompareTag("BlueTeam"))
        {
            Blue.SetActive(true);
            rend.material.SetColor("_Color", Color.blue);
        }
        else if (this.gameObject.CompareTag("RedTeam"))
        {
            Red.SetActive(true);
            rend.material.SetColor("_Color", Color.red);
        }
        else if (this.gameObject.CompareTag("Dead"))
        {
            Blue.SetActive(false);
            Red.SetActive(false);
            rend.material.SetColor("_Color", Color.grey);
        }
    }
}
