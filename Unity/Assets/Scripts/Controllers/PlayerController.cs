using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }


    PlayerStat _stat;

	Vector3 _destPos;

	Texture2D _attackIcon;
	Texture2D _basicIcon;


    [SerializeField]
    PlayerState _state = PlayerState.Idle;

    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Idle:
                    anim.SetFloat("speed", 0);
                    if (_lockTarget != null)
                        anim.SetBool("attack", true);
                    else
                        anim.SetBool("attack", false);
                    break;
                case PlayerState.Moving:
                    anim.SetFloat("speed", _stat.MoveSpeed);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Skill:
                    anim.SetBool("attack", true);
                    break;
            }
        }
    }

    // if Ground/Monster == true
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    
	GameObject _lockTarget;

    void Start()
    {
        _stat = gameObject.GetComponent<PlayerStat>();

		Managers.Input.MouseAction -= OnMouseEvent;
		Managers.Input.MouseAction += OnMouseEvent;
	}

	void UpdateDie()
	{
		// 아무것도 못함

	}

	void UpdateMoving()
	{
        Vector3 dir = _destPos - transform.position;

        // 마우스 클릭 타겟이 존재한다면
        if (_lockTarget != null)
		{
			float distance = (_destPos - transform.position).magnitude;
            Debug.Log(distance);
			if (distance <= 2)
			{
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 1);
                State = PlayerState.Skill;
                return;
			}
		}
		
        if (dir.magnitude <= 0.1)
		{
            State = PlayerState.Idle;
		}
		else
		{
			// window - ai - navigation
			NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

			float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
			nma.Move(dir.normalized * moveDist);

			// 만약 Raycast Block true -> 벽을 만나 더 이상 갈 수 없음.
			// transform.position -> 발바닥 따라서 + @
			if (Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, 1.0f, LayerMask.GetMask("Block")))
			{
				if (Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;
				return;
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}
	}

	void OnHitEvent()
	{
        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else 
        {
            State = PlayerState.Skill;
        }
	}

	void Update()
    {
        Animator anim = GetComponent<Animator>();
        switch (State)
		{
			case PlayerState.Die:
				UpdateDie();
				break;
            case PlayerState.Moving:
				UpdateMoving();
                Debug.Log("RunRun");
				break;
			case PlayerState.Idle:
				break;
            case PlayerState.Skill:
                State = PlayerState.Idle;
                break;
        }
	}

    bool _stopSkill = false;
	void OnMouseEvent(Define.MouseEvent evt)
    {

        switch(State) 
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }

	void OnMouseEvent_IdleRun(Define.MouseEvent evt) 
	{
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        float distance = (_destPos - transform.position).magnitude;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                        State = PlayerState.Moving;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                    break;
                }
        }
    }
}
