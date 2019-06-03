using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTeam : MonoBehaviour
{
    BattleStateMachine bsm;
    string teamTag;
    public void SetTargets(BattleStateMachine bsm, string teamTag)
    {
        this.bsm = bsm;
        this.teamTag = teamTag;
    }

    UIManager UImanager;
    public void SetUIManager(UIManager uiMng)
    {
        UImanager = uiMng;
    }

    private List<BaseClass> GetTeam()
    {
        if (teamTag == "BlueTeam")
            return bsm.BlueTeamInBattle;
        else if (teamTag == "RedTeam")
            return bsm.RedTeamInBattle;

        return new List<BaseClass>();
    }

    public void SelectTeam()
    {
        UImanager.AoETargetSelection(GetTeam());
    }

    public void ToggleAllSelector(bool selection)
    {
        List<BaseClass> targets = GetTeam();
        foreach(BaseClass target in targets)
            target.GetFSM().OnSelection(selection);
    }

}
