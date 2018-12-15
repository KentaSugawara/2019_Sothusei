using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {
	void Start () {
        StartCoroutine(MainCoroutine());
	}

    private IEnumerator MainCoroutine()
    {
        yield return InitCoroutine();
    }

    /// <summary>
    /// 全GameComponentを初期化する
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitCoroutine()
    {
        yield return null;
    }
}
