using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1つしか存在してはいけない&&MonoBehaviourを継承させたい、時に継承させる。

public class G20_Singleton<T> : MonoBehaviour
where T : G20_Singleton<T>
{

    static T instance;
    //debug用
    static T Instance
    {
        set { instance = value;}
        get { return instance; }
    }
    public static T GetInstance()
    {
        if (!Instance &&!(Instance = G20_ComponentUtility.FindComponentOnScene<T>()))
        {
            var obj = new GameObject("SingletonEmpty");
            obj.transform.SetParent(G20_ComponentUtility.Root);
            Instance = obj.AddComponent<T>();

        }
        return Instance;
    }
    //継承classでAwakeを使う場合はbase.Awake()を呼び出す。
    protected virtual void Awake()
    {
        //staticのinstanceがnullの場合自分を入れる。
        if (Instance == null)
        {
            Instance = (T)this;
        }

        //staticのinstanceが自分じゃない場合自殺
        if (Instance != this)
        {
            //Debug.Log(
            //typeof(T) +
            //   " は既に" + Instance.gameObject.name + "にアタッチされているため" +
            //   "このinstanceを破棄しました。");
            //Destroy(this);
        }
    }
    protected void OnDestroy()
    {
        instance = null;
    }
}

