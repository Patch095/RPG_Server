using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectButton : MonoBehaviour
{
    public BaseClass Target;
    public UIManager UImanager;
    
    public void SelectTarget()
    {
        UImanager.TargetSelection(Target);
    }

    public void ToggleSelector(bool selection)
    {
        Target.GetFSM().OnSelection(selection);
    }
}
