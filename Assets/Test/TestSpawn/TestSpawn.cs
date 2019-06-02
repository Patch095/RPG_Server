using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    public GameObject prefab;

    bool hasInstancite;

    public GameObject gameLogic;
    BattleStateMachine bsm;

    public Transform bTeam;

    // Start is called before the first frame update
    void Start()
    {
        hasInstancite = false;
        //gameLogic.SetActive(false);
        bsm = gameLogic.GetComponent<BattleStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        gameLogic.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Space) && !hasInstancite)
        {
            GameObject obj = Instantiate(prefab, bTeam);
            BaseClass objClass = obj.GetComponent<BaseClass>();
            objClass.TeamTag = "BlueTeam";
            obj.GetComponent<CharacterStateMachine>().BSM = bsm;
            obj.SetActive(true);

            bsm.AddToTeamList(objClass);

            bsm.ActiveUI();

            hasInstancite = true;
        }
    }
}
