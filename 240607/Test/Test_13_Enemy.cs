using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_13_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }
}

/// 실습_240607
/// 적의 상태 구현하기
/// 1. Wander : 랜덤으로 계속 이동 (자기 현재 위치에서 일정 반경 안을 랜덤하게 계속 이동하기)
/// 기즈모 그리기
/// ----------------------------------------------------------------------------
/// 2. Chase
/// 3. Find
/// 4. Attack
/// 5. Dead
