using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    TextMeshProUGUI current;
    TextMeshProUGUI max;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        current = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        max = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.AddAmmoCountChangeDelegate(OnAmmoCountChange);
        player.onGunChange += OnGunChange;
    }

    /// <summary>
    /// 총알 개수 변경 시 실행되는 함수
    /// </summary>
    void OnAmmoCountChange(int count)
    {
        current.text = count.ToString();
    }

    /// <summary>
    /// 총이 변경될 때 실행되는 함수
    /// </summary>
    void OnGunChange(GunBase gun)
    {
        max.text = gun.clipSize.ToString();
    }
}

/// 실습_240604
/// BulletCount 완성시키기
/// 1. 총을 쏠 때마다 남은 총알 개수 갱신하기
/// 2. 총을 장비할 때마다 최대 총알 개수 표시하기
/// +. CrossHair 적용하기
