using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

// 일종의 오브젝트 대기실, 오브젝트 캐시
public class PoolManager 
{
    // 구조 -> Pool Manager -> Object Pool -> Object

    // Object Pool
    class Pool
    {
        // Original Object
        public GameObject Original {  get; private set; }
        // Original Transform, ex) 스폰 위치
        public Transform Root { get; set; }
        // 특정 Object가 저장될 pool ex) tank_pool, unitychan_pool
        Stack<Poolable> _poolStack = new Stack<Poolable>();

        // 생성자
        public void init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            // TEST CODE
            for (int i = 0; i < count; i++)
                Push(Create());
            
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        // Pool에 Push
        // Poolable poolable은 Unity Object에서 사용하기 위한 임시 객체
        // 기본 값이 설정되어 있지 않으므로 Push에서 각 값을 지정 후 PoolStack에 최종 Push
        public void Push(Poolable poolable)
        {
            // pool에 넣을 수 없는 object라면?
            if (poolable == null)
                return;

            // Root == 현 오브젝트 최상단, PoolManager의 Transform
            poolable.transform.parent = Root;
            // Pool에 대기시키는 것이기 때문에 setActive false
            poolable.gameObject.SetActive(false);
            // 아직 사용되지 않았기 때문에 IsUsing false
            poolable.IsUsing = false;

            // 생성한 poolable 객체 poolStack에 push
            _poolStack.Push(poolable);
        }

        // Pool에서 Pop
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            // 만약 poolStack이 비어있지 않다면 Pop
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else // 없으면? 만들어주면 된다. 왜? 어쨋든 필요한 것이기 때문에 push 등의 기존 작업을 해주면 됨.
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad 해제 용도
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }

    // Unity Object, Prefab 이름과 Pool
    // PoolManager
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;
    
    // Unity에 PoolManager Object 생성
    public void init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.init(original, count);
        pool.Root.parent = _root.transform;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    // 복사할 original, 상위 parent가 있는지 혹은 독자적인지, 디폴트 null
    public Poolable Pop(GameObject original, Transform parent = null) 
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name) 
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
