using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_Gun : TestBase
{
    public Revolver revolver;
    public Shotgun shotgun;
    public AssaultRifle assaultRifle;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        revolver.Test_Fire();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        revolver.Reload();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        shotgun.Test_Fire();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        assaultRifle.Test_Fire(!context.canceled);
    }
}

/// 실습_240530
/// Revolver, AssaultRifle, Shotgun 구현하기
