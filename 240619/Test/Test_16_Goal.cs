using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_16_Goal : TestBase
{
    private void Start()
    {
        //Goal goal = FindAnyObjectByType<Goal>();
        //goal.SetRandomPosition(GameManager.Instance.MazeWidth, GameManager.Instance.MazHeight);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.HP -= 100000;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();

        int size = GameManager.Instance.MazeWidth * GameManager.Instance.MazeHeight;
        int[] counter = new int[size];

        for (int i = 0; i < 1000000; i++)
        {
            Vector2Int result = goal.TestSetRandomPosition(GameManager.Instance.MazeWidth, GameManager.Instance.MazeHeight);
            if (result.x == 1 && result.y == 1)
            {
                Debug.LogError("Not Valid");
            }

            int index = result.x + result.y * GameManager.Instance.MazeWidth;
            counter[index]++;
        }
        Debug.Log("Check Complete");

        for (int i = 0; i < size; i++)
        {
            Debug.Log($"{i} : {counter[i]}");
        }
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();
        goal.SetRandomPosition(GameManager.Instance.MazeWidth, GameManager.Instance.MazeHeight);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.onGameEnd += (_) => Debug.Log("Goal In");
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        GameManager.Instance.GameStart();
    }
}

/// 실습_240614
/// 1. TestSetRandomPosition 이용해서 각 그리드별로 몇번씩 선택되는지 확인하기
/// 2. Goal 지점에 플레이어가 들어가면 onGameEnd 델리게이트 실행하기
/// 3. 결과 패널 구상하기
