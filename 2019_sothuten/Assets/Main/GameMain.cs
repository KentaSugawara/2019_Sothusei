using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {
    [SerializeField]
    private ObjectSpawner _ObjectSpawner;


    void Start () {
        StartCoroutine(MainCoroutine());
	}

    private IEnumerator MainCoroutine()
    {
        yield return InitCoroutine();

        _ObjectSpawner.StartRunning(() =>{  });
    }

    /// <summary>
    /// 全GameComponentを初期化する
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitCoroutine()
    {
        yield return null; //全Startを待つため
        //Init
        {
            yield return _ObjectSpawner.Init();
        }
    }
}
