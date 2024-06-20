using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_13_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    public Enemy.BehaviorState behaviorState = Enemy.BehaviorState.Wander;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }

#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Vector3 pos = enemy.Test_GetRandomPosition();
        Debug.Log(pos);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        enemy.Test_StateChange(behaviorState);

        // OnStateEnter에서 눈 색깔 변경하기
        // 배회 : 초록색
        // 추적 : 노란색
        // 탐색 : 파란색
        // 공격 : 빨간색
        // 사망 : 검정색
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        enemy.Test_EnemyStop();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Factory.Instance.GetDropItem(Enemy.ItemTable.Shotgun, respawn.position);
    }
#endif
}

/// 실습_240607
/// 1. 적의 상태 구현하기
///     1-1. Wander : 랜덤으로 계속 이동 (자기 현재 위치에서 일정 반경 안을 랜덤하게 계속 이동하기)
/// 2. 기즈모 그리기
/// ----------------------------------------------------------------------------
/// 실습_240610
/// 1. 적의 상태 구현하기
///     1-2. Chase : 플레이어가 마지막으로 목격된 장소를 향해 계속 이동하기 (IsPlayerInSight 구현하기)
/// ----------------------------------------------------------------------------
///     1-3. Find : 플레이어가 시야 범위에서 벗어나면 몇초 동안 두리번거리기 (시간이 다 되면 Wander 상태로 변경, 플레이어를 찾으면 다시 Chase로 변경)
/// ----------------------------------------------------------------------------
/// 4. Attack
/// 5. Dead
