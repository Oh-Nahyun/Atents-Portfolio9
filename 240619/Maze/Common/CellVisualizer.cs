using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    /// <summary>
    /// 셀 한 변의 크기
    /// </summary>
    public const float CellSize = 10.0f;

    GameObject[] walls;
    GameObject[] corners;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        walls = new GameObject[child.childCount];
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = child.GetChild(i).gameObject;
        }

        child = transform.GetChild(2);
        corners = new GameObject[child.childCount];
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = child.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// 입력받은 데이터에 맞게 벽의 활성화 여부 재설정 함수
    /// </summary>
    /// <param name="data"></param>
    public void RefreshWall(byte data)
    {
        // data : 북(첫번째 비트)/동/남/서 순서대로 1이 세팅되어있으면 길이고 (= 벽이 없다.), 0이 세팅되어있으면 벽이다.

        for (int i = 0; i < walls.Length; i++)
        {
            int mask = 1 << i;
            walls[i].SetActive(!((data & mask) != 0)); // 순서대로 마스크를 생성한 후 & 연산으로 결과 확인
        }
    }

    /// <summary>
    /// 입력받은 데이터에 맞게 코너의 활성화 여부 재설정
    /// </summary>
    /// <param name="data"></param>
    public void RefreshCorner(int data)
    {
        for (int i = 0; i < corners.Length; i++)
        {
            int mask = 1 << i;
            corners[i].SetActive((data & mask) != 0);
        }
    }

    /// <summary>
    /// 현재 셀의 상태를 확인해서 벽 활성화 정보를 리턴하는 함수
    /// </summary>
    /// <returns>4bit 북동남서 순으로, 벽은 0, 길은 1로 세팅되어있는 데이터</returns>
    public Direction GetPath()
    {
        int mask = 0;                   // << (Shift 연산)의 결과는 int이기 때문에 int로 설정

        for (int i = 0; i < walls.Length; i++)
        {
            if (!walls[i].activeSelf)   // 활성화 되어있는지 확인해서
            {
                mask |= 1 << i;         // 비활성화 되어있으면 1로 세팅
            }
        }

        return (Direction)mask;
    }
}
