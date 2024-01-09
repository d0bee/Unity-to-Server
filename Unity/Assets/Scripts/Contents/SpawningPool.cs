csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0; // 몬스터의 현재 수량을 나타내는 변수

    int _reserveCount = 0; // 대기 중인 몬스터 수량을 나타내는 변수

    [SerializeField]
    int _keepMonsterCount = 0; // 유지해야 할 몬스터 최대 수량을 나타내는 변수

    [SerializeField]
    Vector3 _spawnPos; // 몬스터 스폰 위치

    [SerializeField]
    float _spawnRadius = 15.0f; // 몬스터 스폰 반경

    [SerializeField]
    float _spawnTime = 5.0f; // 몬스터 스폰 주기

    public void AddMonsterCount(int value) { _monsterCount += value; } // 몬스터 수량을 증가시키는 메서드
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; } // 유지해야 할 몬스터 수량을 설정하는 메서드

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
        // 게임 매니저의 OnSpawnEvent 이벤트에 AddMonsterCount 메서드를 델리게이트로 등록하여 몬스터 생성 시 몬스터 수량을 증가시킴
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
            // 대기 중인 몬스터 수량과 현재 몬스터 수량의 합이 유지해야 할 몬스터 수량보다 작을 때까지 몬스터를 대기 시킴
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++; // 대기 중인 몬스터 수량 증가
        yield return new WaitForSeconds(Random.Range(0, _spawnTime)); // 랜덤한 시간만큼 대기

        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight"); // 게임 매니저를 통해 몬스터 생성

        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>(); // 생성된 몬스터에 NavMeshAgent 컴포넌트 추가

        Vector3 randPos;
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius); // 랜덤한 방향과 거리로 이동할 위치 설정
            randDir.y = 0; // y축 값은 0으로 설정하여 평면 상에 몬스터를 이동시킴
            randPos = _spawnPos + randDir; // 스폰 위치와 랜덤한 위치를 더하여 최종적인 이동 위치 설정

            // 목적지까지 이동 가능한 경로인지 확인
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break; // 이동 가능한 경로인 경우 반복문 종료
        }

        obj.transform.position = randPos; // 몬스터의 위치를 설정한 랜덤 위치로 이동
        _reserveCount--; // 대기 중인 몬스터 수량 감소
    }
}