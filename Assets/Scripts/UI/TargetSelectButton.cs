using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectButton : MonoBehaviour
{
    private BaseClass target;
    public BaseClass Target { get { return target; } }
    public void SetTarget(BaseClass newTarget)
    {
        target = newTarget;
    }

    UIManager UImanager;
    public void SetUIManager(UIManager uiMng)
    {
        UImanager = uiMng;
    }

    public void SelectTarget()
    {
        UImanager.TargetSelection(Target);
    }

    public void ToggleSelector(bool selection)
    {
        Target.GetFSM().OnSelection(selection);
    }
}
