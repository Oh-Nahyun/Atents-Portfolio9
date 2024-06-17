using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Crosshair : TestBase
{
    public Crosshair crosshair;
    public float expendAmount = 30.0f;

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        crosshair.Expend(expendAmount);
    }
}
