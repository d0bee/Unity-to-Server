using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

	Vector3 _destPos;

    void Start()
    {
		Managers.Input.MouseAction -= OnMouseClicked;
		Managers.Input.MouseAction += OnMouseClicked;
	}

	public enum PlayerState
	{
		Die,
		Moving,
		Idle,
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

			float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
			nma.Move(dir.normalized * moveDist);

			// 만약 Raycast Block true -> 벽을 만나 더 이상 갈 수 없음.
			// transform.position -> 발바닥 따라서 + @
			if (Physics.Raycast(transform.position + Vector3.up * 0.1f, dir, 1.0f, LayerMask.GetMask("Block")))
			{
				_state = PlayerState.Idle;
				return;
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}

		// 애니메이션
		Animator anim = GetComponent<Animator>();
		// 현재 게임 상태에 대한 정보를 넘겨준다
		anim.SetFloat("speed", _speed);
	}

	void UpdateIdle()
	{
		// 애니메이션
		Animator anim = GetComponent<Animator>();

		anim.SetFloat("speed", 0);
	}

	void Update()
    {
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

	void OnMouseClicked(Define.MouseEvent evt)
    {

		if (_state == PlayerState.Die)
			return;
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
		{
			_destPos = hit.point;
			_state = PlayerState.Moving;
		}
	}
}
