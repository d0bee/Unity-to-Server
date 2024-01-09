using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp; // 경험치를 저장하는 변수

    [SerializeField]
    protected int _gold; // 골드를 저장하는 변수

    public int Exp // 경험치를 가져오고 설정하는 프로퍼티
    {
        get { return _exp; }
        set
        {
            _exp = value;

            int level = 1;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }

            if (level != Level)
            {
                Debug.Log("Level Up!");
                Level = level;
                SetStat(Level);
            }
        }
    }

    public int Gold { get { return _gold; } set { _gold = value; } } // 골드를 가져오고 설정하는 프로퍼티

    private void Start()
    {
        _level = 1; // 플레이어 레벨 초기화
        _exp = 0; // 경험치 초기화
        _defense = 5; // 방어력 초기화
        _moveSpeed = 5.0f; // 이동 속도 초기화
        _gold = 0; // 골드 초기화

        SetStat(_level); // 플레이어 스탯 설정
    }

    public void SetStat(int level) // 플레이어 스탯을 설정하는 메서드
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict; // 스탯 데이터를 담고 있는 딕셔너리 가져오기
        Data.Stat stat = dict[level]; // 해당 레벨에 맞는 스탯 데이터 가져오기
        _hp = stat.maxHp; // 체력 설정
        _maxHp = stat.maxHp; // 최대 체력 설정
        _attack = stat.attack; // 공격력 설정
    }

    protected override void OnDead(Stat attacker) // 플레이어가 사망했을 때 호출되는 메서드
    {
        Debug.Log("Player Dead");
    }
}