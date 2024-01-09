using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    GameObject _player; // 플레이어 게임 오브젝트를 저장하는 변수

    HashSet<GameObject> _monsters = new HashSet<GameObject>(); // 몬스터 게임 오브젝트들을 저장하는 HashSet 자료구조

    public Action<int> OnSpawnEvent; // 스폰 이벤트가 발생할 때 호출되는 델리게이트 변수

    public GameObject GetPlayer()
    {
        return _player; // 플레이어 게임 오브젝트 반환
    }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent); // 주어진 경로(path)에 있는 프리팹을 인스턴스화하여 게임 오브젝트(go)에 저장

        switch (type)
        {
            case Define.WorldObject.Monster: // 월드 오브젝트가 몬스터인 경우
                _monsters.Add(go); // 몬스터 HashSet에 게임 오브젝트 추가

                if (OnSpawnEvent != null) // 스폰 이벤트에 등록된 메소드가 있다면
                    OnSpawnEvent.Invoke(1); // 스폰 이벤트 호출 및 인자로 1을 전달 (몬스터 스폰)

                break;

            case Define.WorldObject.Player: // 월드 오브젝트가 플레이어인 경우
                _player = go; // 플레이어 게임 오브젝트를 _player 변수에 저장
                break;
        }

        return go; // 생성된 게임 오브젝트 반환
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>(); // 게임 오브젝트에서 BaseController 컴포넌트를 가져옴

        if (bc == null) // BaseController 컴포넌트가 없는 경우
            return Define.WorldObject.Unknown; // 월드 오브젝트 타입을 알 수 없는 상태로 반환

        return bc.WorldObjectType; // BaseController 컴포넌트에서 가져온 월드 오브젝트 타입 반환
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go); // 게임 오브젝트의 월드 오브젝트 타입을 가져옴

        switch (type)
        {
            case Define.WorldObject.Monster: // 월드 오브젝트가 몬스터인 경우
                {
                    if (_monsters.Contains(go)) // 몬스터 HashSet에 게임 오브젝트가 포함되어 있는 경우
                    {
                        _monsters.Remove(go); // 몬스터 HashSet에서 게임 오브젝트를 제거

                        if (OnSpawnEvent != null) // 스폰 이벤트에 등록된 메소드가 있다면
                            OnSpawnEvent.Invoke(-1); // 스폰 이벤트 호출 및 인자로 -1을 전달 (몬스터 제거)
                    }
                }
                break;

            case Define.WorldObject.Player: // 월드 오브젝트가 플레이어인 경우
                {
                    if (_player == go) // 플레이어 게임 오브젝트와 인자로 받은 게임 오브젝트가 같은 경우
                        _player = null; // 플레이어 변수를 null로 초기화
                }
                break;
        }

        Managers.Resource.Destroy(go); // 게임 오브젝트 제거
    }
}