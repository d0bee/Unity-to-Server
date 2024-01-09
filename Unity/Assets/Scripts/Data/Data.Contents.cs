csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat
    [Serializable]
    public class Stat
    {
        public int level;       // 레벨 변수
        public int maxHp;       // 최대 체력 변수
        public int attack;      // 공격력 변수
        public int totalExp;    // 총 경험치 변수
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();   // Stat 클래스의 리스트

        // MakeDict 메서드: stats 리스트를 이용하여 레벨을 키로 하는 Dictionary 생성
        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }
    #endregion
}