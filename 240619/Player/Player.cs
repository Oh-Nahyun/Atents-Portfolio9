using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 유니티가 제공하는 시작용 코드 (입력 처리용 함수를 모아놓은 클래스)
    /// </summary>
    StarterAssetsInputs starterAssets;

    /// <summary>
    /// 유니티가 제공하는 입력 처리용 코드
    /// </summary>
    FirstPersonController controller;

    /// <summary>
    /// PlayerInput 컴포넌트
    /// </summary>
    PlayerInput playerInpuut;

    /// <summary>
    /// 총만 촬영하는 카메라가 있는 게임 오브젝트
    /// </summary>
    GameObject gunCamera;

    /// <summary>
    /// 총은 카메라 기준으로 발사됨
    /// </summary>
    public Transform FireTransform => transform.GetChild(0); // 카메라 루트

    /// <summary>
    /// 플레이어가 장비할 수 있는 모든 총
    /// </summary>
    GunBase[] guns;

    /// <summary>
    /// 현재 장비하고 있는 총
    /// </summary>
    GunBase activeGun;

    /// <summary>
    /// 최대 HP
    /// </summary>
    public float MaxHP = 100.0f;

    /// <summary>
    /// 현재 HP
    /// </summary>
    float hp;

    /// <summary>
    /// 현재 HP 확인 및 설정용 프로퍼티
    /// </summary>
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                Die();                      // HP가 0 이하면 사망
            }
            hp = Math.Clamp(hp, 0, MaxHP);  // HP 최대 최소 안벗어나게 만들기
            Debug.Log($"HP : {hp}");
            onHPChange?.Invoke(hp);         // HP 변화 알리기
        }
    }

    /// <summary>
    /// 총이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<GunBase> onGunChange;

    /// <summary>
    /// 공격을 받았을 때 실행될 델리게이트 (float : 공격 받은 각도. 플레이어 forward와 적으로 가는 방향 벡터 사이의 각도 (시계 방향))
    /// </summary>
    public Action<float> onAttacked;

    /// <summary>
    /// HP가 변경되었을 때 실행될 델리게이트 (float : 현재 HP)
    /// </summary>
    public Action<float> onHPChange;

    /// <summary>
    /// 플레이어가 맵의 가운데 배치되었을 때 실행될 델리게이트
    /// </summary>
    public Action onSpawn;

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie;

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();
        playerInpuut = GetComponent<PlayerInput>();

        gunCamera = transform.GetChild(2).gameObject;

        Transform child = transform.GetChild(3);
        guns = child.GetComponentsInChildren<GunBase>(true); // 모든 총 찾기
    }

    private void Start()
    {
        starterAssets.onZoom += DisableGunCamera; // 줌할 때 실행될 함수 연결

        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        foreach (GunBase gun in guns)
        {
            gun.onFire += controller.FireRecoil;                        // 화면 튕기는 효과
            gun.onFire += (expend) => crosshair.Expend(expend * 10);    // 조준선 확장 효과
            gun.onAmmoDepleted += () =>
            {
                if (!(activeGun is Revolver))
                {
                    GunChange(GunType.Revolver);                        // 총알이 다 떨어지면 기본 총으로 변경
                }
            };
        }

        activeGun = guns[0];                // 기본 총 설정
        activeGun.Equip();                  // 기본 총 장비
        onGunChange?.Invoke(activeGun);     // 총 변경 알림

        HP = MaxHP;

        GameManager.Instance.onGameEnd += (_) => InputDisable();   // 게임이 클리어 되면 입력 막기

        Spawn();
    }

    /// <summary>
    /// 총 표시하는 카메라 활성화 설정 함수
    /// </summary>
    /// <param name="disable">true면 비활성화(총이 안보인다.), false면 활성화(총이 보인다.)</param>
    void DisableGunCamera(bool disable = true)
    {
        gunCamera.SetActive(!disable);
    }

    /// <summary>
    /// 장비 중인 총을 변경하는 함수
    /// </summary>
    /// <param name="gunType">총의 종류</param>
    public void GunChange(GunType gunType)
    {
        activeGun.gameObject.SetActive(false);  // 이전 총 비활성화하고 장비 해제학기
        activeGun.UnEquip();

        activeGun = guns[(int)gunType];         // 새총 설정하고 장비하고 활성화하기
        activeGun.Equip();
        activeGun.gameObject.SetActive(true);

        onGunChange?.Invoke(activeGun);         // 총이 변경되었음을 알림
    }

    /// <summary>
    /// 장비 중인 총을 발사하는 함수
    /// </summary>
    /// <param name="isFireStart">true면 발사 버튼을 눌렀다, false면 발사 버튼을 뗐다.</param>
    public void GunFire(bool isFireStart)
    {
        activeGun.Fire(isFireStart);
    }

    /// <summary>
    /// 리볼버를 재장전하는 함수
    /// </summary>
    public void RevolverReload()
    {
        Revolver revolver = activeGun as Revolver;
        if (revolver != null) // activeGun이 리볼버일 때만 재장전
        {
            revolver.Reload();
        }
    }

    /// <summary>
    /// 총알 개수가 변경될 때 실행되는 델리게이트에 콜백 함수 추가
    /// </summary>
    /// <param name="callback">추가할 콜백(응답용) 함수</param>
    public void AddAmmoCountChangeDelegate(Action<int> callback)
    {
        foreach (var gun in guns)
        {
            gun.onAmmoCountChange += callback;
        }
    }

    /// <summary>
    /// 공격을 받았을 때 실행되는 함수
    /// </summary>
    /// <param name="enemy">공격을 한 적</param>
    public void OnAttacked(Enemy enemy)
    {
        Vector3 dir = enemy.transform.position - transform.position;

        // 공격 당한 각도 (시계 방향)
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        onAttacked?.Invoke(-angle);
        HP -= enemy.attackPower;
    }

    /// <summary>
    /// 플레이어를 맵에 배치시키는 함수
    /// </summary>
    public void Spawn()
    {
        GameManager gameManager = GameManager.Instance;
        Vector3 centerPos = MazeVisualizer.GridToWorld(gameManager.MazeWidth / 2, gameManager.MazeHeight / 2);
        transform.position = centerPos;  // 플레이어를 미로의 가운데 위치로 옮기기
        onSpawn?.Invoke();
    }

    /// <summary>
    /// 플레이어 사망 처리용 함수
    /// </summary>
    private void Die()
    {
        onDie?.Invoke();                // 죽었음을 알림
        gameObject.SetActive(false);    // 플레이어 오브젝트 비활성화
    }

    /// <summary>
    /// 입력을 막는 함수
    /// </summary>
    void InputDisable()
    {
        // starterAssets.enabled = false;

        // [방법 1]
        playerInpuut.actions.actionMaps[0].Disable(); // 액션맵이 1개만 있기 때문에 그냥 처리

        // [방법 2]
        // InputActionMap playerActionMap = playerInpuut.actions.FindActionMap("Player");
        // playerActionMap.Disable();
    }
}
