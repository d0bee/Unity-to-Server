using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos; // 이동할 목적지의 좌표를 저장하는 변수입니다.

    [SerializeField]
    protected Define.State _state = Define.State.Idle; // 현재 캐릭터의 상태를 저장하는 변수입니다.

    [SerializeField]
    protected GameObject _lockTarget; // 현재 캐릭터가 공격 대상으로 잠긴 타겟을 저장하는 변수입니다.

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown; // 캐릭터의 월드 오브젝트 타입을 나타내는 열거형 변수입니다.

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f); // 애니메이터의 WAIT 애니메이션을 재생합니다.
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f); // 애니메이터의 RUN 애니메이션을 재생합니다.
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0); // 애니메이터의 ATTACK 애니메이션을 재생합니다.
                    break;
            }
        }
    }

    private void Start()
    {
        Init(); // Init 메서드를 호출하여 초기화 작업을 수행합니다.
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie(); // Die 상태 업데이트 메서드를 호출합니다.
                break;
            case Define.State.Moving:
                UpdateMoving(); // Moving 상태 업데이트 메서드를 호출합니다.
                break;
            case Define.State.Idle:
                UpdateIdle(); // Idle 상태 업데이트 메서드를 호출합니다.
                break;
            case Define.State.Skill:
                UpdateSkill(); // Skill 상태 업데이트 메서드를 호출합니다.
                break;
        }
    }

    public abstract void Init(); // 추상 메서드로, 파생 클래스에서 반드시 구현해야 합니다.

    protected virtual void UpdateDie() { } // 가상 메서드로, 필요에 따라 파생 클래스에서 재정의할 수 있습니다.
    protected virtual void UpdateMoving() { } // 가상 메서드로, 필요에 따라 파생 클래스에서 재정의할 수 있습니다.
    protected virtual void UpdateIdle() { } // 가상 메서드로, 필요에 따라 파생 클래스에서 재정의할 수 있습니다.
    protected virtual void UpdateSkill() { } // 가상 메서드로, 필요에 따라 파생 클래스에서 재정의할 수 있습니다.
}