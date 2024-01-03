using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            // 가져올 객체 name = 전체 path
            string name = path;
            // 마지막 폴더 이후 name = index
            int index = name.LastIndexOf('/');
            // 만약 index가 존재한다면 저장
            if (index>=0) 
                name = name.Substring(index+1);

            // 해당 Object name을 Pool에서 가져오기
            GameObject go = Managers.Pool.GetOriginal(name);
            // 존재하면 return go
            if (go != null)
                return go as T;
        }
        // 없다면 path를 통해서 직접 불러오기
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {

        // 1. originaml 이미 들고 있으면 바로 사용 -> Load에 작성됨.
        // 2. 혹시 풀링된 애가 있을까? -> Instantiate에 작성됨.
        // 3. Destroy하는데 나중에 사용할지도 모르는 Poolable 객체라면?

        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // Pool에 대기 중인 객체가 있다면,
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 만약 Poolable 객체라면,
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
