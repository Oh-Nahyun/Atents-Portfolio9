using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    /// <summary>
    /// 한 번에 발사되는 총알 개수
    /// </summary>
    public int pellet = 6;

    protected override void FireProcess(bool isFireStart = true)
    {
        if (isFireStart)
        {
            base.FireProcess(isFireStart);

            for (int i = 0; i < pellet; i++)
            {
                HitProcess(); // 여러번 Hit 처리
            }

            FireRecoil();
        }
    }
}

/// 실습_240530
/// 한 번 Fire 할 때마다 여러 발의 총알이 나간다.
