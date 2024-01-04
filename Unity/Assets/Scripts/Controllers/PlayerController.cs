using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	PlayerStat _stat;

	Vector3 _destPos;

	Texture2D _attackIcon;
	Texture2D _basicIcon;

	enum CursorType
	{
		None,
		Basic,
		Attack,
	}
	CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _basicIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Basic");

        _stat = gameObject.GetComponent<PlayerStat>();

		Managers.Input.MouseAction -= OnMouseEvent;
		Managers.Input.MouseAction += OnMouseEvent;
	}

	public enum PlayerState
	{
		Die,
		Moving,
		Idle,
		Skill,
	}

	PlayerState _state = PlayerState.Idle;

	void UpdateDie()
	{
		// 아무것도 못함

	}

	void UpdateMoving()
	{
		Vector3 dir = _destPos - transform.position;
		if (dir.magnitude < 0.1f)
		{
			_state = PlayerState.Idle;
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
					_state = PlayerState.Idle;
				return;
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}

		// 애니메이션
		Animator anim = GetComponent<Animator>();
		// 현재 게임 상태에 대한 정보를 넘겨준다
		anim.SetFloat("speed", _stat.MoveSpeed);
	}

	void UpdateIdle()
	{
		// 애니메이션
		Animator anim = GetComponent<Animator>();

		anim.SetFloat("speed", 0);
	}

	void Update()
    {
		UpdateMouseCursor();

		switch (_state)
		{
			case PlayerState.Die:
				UpdateDie();
				break;
			case PlayerState.Moving:
				UpdateMoving();
				break;
			case PlayerState.Idle:
				UpdateIdle();
				break;
		}
	}

	// 마우스 피아식별
	void UpdateMouseCursor()
	{
		if (Input.GetMouseButton(0))
			return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
				if (_cursorType != CursorType.Attack)
				{
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
					_cursorType = CursorType.Attack;
                }
			}
            else
            {
				if (_cursorType != CursorType.Basic)
				{
					Cursor.SetCursor(_basicIcon, new Vector2(_basicIcon.width / 3, 0), CursorMode.Auto);
					_cursorType= CursorType.Basic;
                }
            }
        }
    }

	// if Ground/Monster == true
	int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
	GameObject _lockTarget;
	void OnMouseEvent(Define.MouseEvent evt)
    {

		if (_state == PlayerState.Die)
			return;

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
                        _state = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
							_lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget= null;
                    }
				}
				break;
            case Define.MouseEvent.Press:
				{
					if (_lockTarget != null)
						_destPos = _lockTarget.transform.position;
					else if (raycastHit)
						_destPos = hit.point;
                    break;
                }
            case Define.MouseEvent.PointerUp:
				{
                    _lockTarget = null;
                    break;
                }
        }
    }
}
