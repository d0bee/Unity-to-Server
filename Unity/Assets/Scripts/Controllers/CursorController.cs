using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;
    Texture2D _basicIcon;

    enum CursorType
    {
        None,
        Basic,
        Attack,
    }
    CursorType _cursorType = CursorType.None;

    // if Ground/Monster == true
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    
    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _basicIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Basic");
    }

    void Update()
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
                    _cursorType = CursorType.Basic;
                }
            }
        }
    }
}
