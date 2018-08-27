using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class G20_ReloadScene : G20_Singleton<G20_ReloadScene> {
    public void ReloadScene()
    {
        // 現在のScene名を取得する
        Scene loadScene = SceneManager.GetActiveScene();
        // Sceneの読み直し
        SceneManager.LoadScene(loadScene.name);
    }
}
