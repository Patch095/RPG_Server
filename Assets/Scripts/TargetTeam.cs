using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTeam : MonoBehaviour
{
    BattleStateMachine bsm;
    string teamTag;

    public void setBattleStateMachine(BattleStateMachine bsm, string teamTag)
    {
        this.bsm = bsm;
        this.teamTag = teamTag;
    }
    public List<BaseClass> GetTeam()
    {
        if (teamTag == "BlueTeam")
        {
            return bsm.BlueTeamInBattle;
        }
        else if (teamTag == "RedTeam")
        {
            return bsm.RedTeamInBattle;
        }

        return new List<BaseClass>();

    }
}
