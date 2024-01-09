using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    // 몬스터의 스캔 범위
    [SerializeField]
    float _scanRange = 10;

    // 몬스터의 공격 범위
    [SerializeField]
    float _attackRange = 2;

    // BaseController 클래스의 Init() 메서드 재정의
    public override void Init()
    {
        // 월드 오브젝트 타입 설정
        WorldObjectType = Define.WorldObject.Monster;

        // Stat 컴포넌트 가져오기
        _stat = gameObject.GetComponent<Stat>();

        // 자식 오브젝트에서 UI_HPBar 컴포넌트 가져오기
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    // BaseController 클래스의 UpdateIdle() 메서드 재정의
    protected override void UpdateIdle()
    {
        // 플레이어 오브젝트 가져오기
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        // 몬스터와 플레이어 사이의 거리 계산
        float distance = (player.transform.position - transform.position).magnitude;

        // 거리가 스캔 범위 이내라면
        if (distance <= _scanRange)
        {
            // 타겟을 플레이어로 설정하고 상태를 Moving으로 변경
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    // BaseController 클래스의 UpdateMoving() 메서드 재정의
    protected override void UpdateMoving()
    {
        // 타겟이 설정되어 있다면
        if (_lockTarget != null)
        {
            // 목적지 설정
            _destPos = _lockTarget.transform.position;

            // 목적지까지의 거리 계산
            float distance = (_destPos - transform.position).magnitude;

            // 거리가 공격 범위 이내라면
            if (distance <= _attackRange)
            {
                // NavMeshAgent 컴포넌트 가져오기
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

                // 현재 위치를 목적지로 설정하여 이동을 멈추도록 함
                nma.SetDestination(transform.position);

                // 상태를 Skill로 변경
                State = Define.State.Skill;
                return;
            }
        }

        // 이동 방향 계산
        Vector3 dir = _destPos - transform.position;

        // 목적지에 도착했다면 상태를 Idle로 변경
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            // NavMeshAgent 컴포넌트 가져오기
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            // 목적지 설정 및 이동 속도 설정
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;

            // 몬스터의 회전 처리
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    // BaseController 클래스의 UpdateSkill() 메서드 재정의
    protected override void UpdateSkill()
    {
        // 타겟이 설정되어 있다면
        if (_lockTarget != null)
        {
            // 타겟의 위치와의 방향 계산
            Vector3 dir = _lockTarget.transform.position - transform.position;

            // 몬스터의 회전 처리
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    // 몬스터가 공격을 받았을 때 호출되는 이벤트 메서드
    void OnHitEvent()
    {
        // 타겟이 설정되어 있다면
        if (_lockTarget != null)
        {
            // 타겟의 Stat 컴포넌트 가져오기
            Stat targetStat = _lockTarget.GetComponent<Stat>();

            // 타겟의 체력 감소
            targetStat.OnAttacked(_stat);

            // 타겟의 체력이 0보다 크다면
            if (targetStat.Hp > 0)
            {
                // 타겟과 몬스터 사이의 거리 계산
                float distance = (_lockTarget.transform.position - transform.position).magnitude;

                // 거리가 공격 범위 이내라면 상태를 Skill로 변경
                if (distance <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
            {
                // 타겟의 체력이 0이라면 상태를 Idle로 변경
                State = Define.State.Idle;
            }
        }
        else
        {
            // 타겟이 설정되어 있지 않다면 상태를 Idle로 변경
            State = Define.State.Idle;
        }
    }
}