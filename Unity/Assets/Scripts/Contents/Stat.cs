아래 코드에 주석을 추가했습니다:

csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level; // 캐릭터의 레벨

    [SerializeField]
    protected int _hp; // 캐릭터의 현재 체력

    [SerializeField]
    protected int _maxHp; // 캐릭터의 최대 체력

    [SerializeField]
    protected int _attack; // 캐릭터의 공격력

    [SerializeField]
    protected int _defense; // 캐릭터의 방어력

    [SerializeField]
    protected float _moveSpeed; // 캐릭터의 이동 속도

    public int Level { get { return _level; } set { _level = value; } } // 캐릭터의 레벨을 가져오거나 설정하는 프로퍼티
    public int Hp { get { return _hp; } set { _hp = value; } } // 캐릭터의 현재 체력을 가져오거나 설정하는 프로퍼티
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } } // 캐릭터의 최대 체력을 가져오거나 설정하는 프로퍼티
    public int Attack { get { return _attack; } set { _attack = value; } } // 캐릭터의 공격력을 가져오거나 설정하는 프로퍼티
    public int Defense { get { return _defense; } set { _defense = value; } } // 캐릭터의 방어력을 가져오거나 설정하는 프로퍼티
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } } // 캐릭터의 이동 속도를 가져오거나 설정하는 프로퍼티

    private void Start()
    {
        _level = 1; // 초기 레벨을 1로 설정
        _hp = 100; // 초기 체력을 100으로 설정
        _maxHp = 100; // 초기 최대 체력을 100으로 설정
        _attack = 10; // 초기 공격력을 10으로 설정
        _defense = 5; // 초기 방어력을 5로 설정
        _moveSpeed = 5.0f; // 초기 이동 속도를 5.0으로 설정
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense); // damage 변수에는 공격자의 공격력에서 방어력을 뺀 값이 저장됩니다.
        Hp -= damage; // 현재 체력에서 damage만큼 감소시킵니다.

        if (Hp <= 0) // 만약 현재 체력이 0 이하라면
        {
            Hp = 0; // 체력을 0으로 설정합니다.
            OnDead(attacker); // OnDead 메서드를 호출합니다.
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat; // attacker를 PlayerStat 타입으로 캐스팅하여 playerStat 변수에 저장합니다.

        if (playerStat != null) // playerStat이 null이 아니라면 (attacker가 PlayerStat 타입이라면)
        {
            playerStat.Exp += 15; // attacker의 경험치를 15 증가시킵니다.
        }

        Managers.Game.Despawn(gameObject); // 게임 매니저의 Despawn 메서드를 호출하여 자신을 제거합니다.
    }
}