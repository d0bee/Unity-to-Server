using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    PlayerStat _stat;
    bool _stopSkill = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        // 만약 _lockTarget이 null이 아니라면
        if (_lockTarget != null)
        {
            // _destPos를 _lockTarget의 위치로 설정
            _destPos = _lockTarget.transform.position;
            // 현재 위치와 _destPos 사이의 거리를 계산
            float distance = (_destPos - transform.position).magnitude;
            // 거리가 1 이하라면
            if (distance <= 1)
            {
                // 상태를 스킬 상태로 변경하고 종료
                State = Define.State.Skill;
                return;
            }
        }
        // 이동 방향을 계산
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;

        // 이동 방향의 크기가 0.1보다 작다면
        if (dir.magnitude < 0.1f)
        {
            // 상태를 대기 상태로 변경
            State = Define.State.Idle;
        }
        else
        {
            // 디버그 라인을 그리기 위한 코드
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            // 블록 레이어와 충돌 검사
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                // 마우스 왼쪽 버튼이 눌려있지 않다면
                if (Input.GetMouseButton(0) == false)
                    // 상태를 대기 상태로 변경하고 종료
                    State = Define.State.Idle;
                return;
            }

            // 이동 거리를 계산하고 이동
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            // 캐릭터의 회전을 부드럽게 보정
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        // 만약 _lockTarget이 null이 아니라면
        if (_lockTarget != null)
        {
            // _lockTarget의 위치와 현재 위치 사이의 방향을 계산
            Vector3 dir = _lockTarget.transform.position - transform.position;
            // 해당 방향으로 회전하는 Quaternion을 계산
            Quaternion quat = Quaternion.Look(dir);
            // 캐릭터의 회전을 부드럽게 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        // _lockTarget이 null이 아니라면
        if (_lockTarget != null)
        {
            // _lockTarget의 Stat 컴포넌트를 가져옴
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            // targetStat의 OnAttacked 메서드를 호출하여 _stat을 인자로 전달
            targetStat.OnAttacked(_stat);
        }

        // _stopSkill이 true라면
        if (_stopSkill)
        {
            // 상태를 대기 상태로 변경
            State = Define.State.Idle;
        }
        else
        {
            // 상태를 스킬 상태로 변경
            State = Define.State.Skill;
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    // 이벤트가 PointerUp인 경우 _stopSkill을 true로 설정
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        // 마우스 위치를 기준으로 레이캐스트를 발사하여 충돌 여부를 검사
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    // 레이가 충돌한 경우
                    if (raycastHit)
                    {
                        // _destPos를 충돌 지점으로 설정
                        _destPos = hit.point;
                        // 상태를 이동 상태로 변경
                        State = Define.State.Moving;
                        // _stopSkill을 false로 설정
                        _stopSkill = false;

                        // 충돌한 객체의 레이어가 몬스터인 경우
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            // _lockTarget을 충돌한 객체로 설정
                            _lockTarget = hit.collider.gameObject;
                        else
                            // _lockTarget을 null로 설정
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    // _lockTarget이 null이고 레이가 충돌한 경우
                    if (_lockTarget == null && raycastHit)
                        // _destPos를 충돌 지점으로 설정
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                // _stopSkill을 true로 설정
                _stopSkill = true;
                break;
        }
    }
}
