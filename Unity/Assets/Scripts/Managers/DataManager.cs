using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 참조 인터페이스
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager 
{
    // 관리하기 용이하도록 int level, Stat 형태의 Dictionary 생성
    public Dictionary<int, Data.Stat> StatDict {  get; private set; } = new Dictionary<int, Data.Stat>();
    public void init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        // local에서 json 파일 Load
        TextAsset testAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        // from Load한 json 파일 -> testAsset.text형식 반환
        return JsonUtility.FromJson<Loader>(testAsset.text);
    }
}
