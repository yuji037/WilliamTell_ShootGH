﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class G20_ReloadScene : G20_Singleton<G20_ReloadScene>
{
    [SerializeField] string MainSceneName;
    [SerializeField] string SubSceneName;
    public void ReloadScene()
    {
        //MainSceneの読み直し
        if (MainSceneName.Length > 0)
        {
            SceneManager.LoadScene(MainSceneName,LoadSceneMode.Single);
        }
        //SubSceneの読み直し
        if (SubSceneName.Length > 0)
        {
            SceneManager.LoadScene(SubSceneName,LoadSceneMode.Additive);
        }
    }
}
