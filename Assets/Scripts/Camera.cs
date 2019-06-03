using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    bool activeblueCam = true;
    public GameObject blueCam;
    public GameObject redcam;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        blueCam.SetActive(activeblueCam);
        redcam.SetActive(!activeblueCam);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeblueCam = !activeblueCam;
        }

    }
}
