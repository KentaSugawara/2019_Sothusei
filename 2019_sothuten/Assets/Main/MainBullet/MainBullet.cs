using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet : MonoBehaviour {
    public static MainBullet Instance;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _SpeedRatio;

    //手元にある状態
    private bool _isSetBullet;

    private Vector3 _NextShotposition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(RunningCoroutine());
    }

    private IEnumerator RunningCoroutine()
    {
        while (true)
        {
            yield return ReturnBullletCoroutine();
            yield return StayBullletCoroutine();
            yield return ShotBullletCoroutine();
        }
    }

    #region 

    /// <summary>
    /// 手元に戻すコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnBullletCoroutine()
    {
        Debug.Log("Return");
        _isSetBullet = false;
        //まだ手に戻ってない状態
        Vector3 v;
        float d;
        while (true)
        {
            v = (transform.position - InputManager.Instance.HandPosition);
            d = v.sqrMagnitude;

            if (d < 0.1f)
            {
                //ここで手元に戻ってることにする
                break;
            }

            //移動
            transform.position = Vector3.Lerp(transform.position, InputManager.Instance.HandPosition, _SpeedRatio);

            yield return null;
        }

        _isSetBullet = true;
    }

    /// <summary>
    /// 手元にある状態
    /// </summary>
    /// <returns></returns>
    private IEnumerator StayBullletCoroutine()
    {
        Debug.Log("Stay");
        while (true)
        {
            transform.position = InputManager.Instance.HandPosition;
            if (InputManager.Instance.TriggerDown)
            {
                //ここで発射された
                _NextShotposition = InputManager.Instance.ShotPosition;
                break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 発射されている状態
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotBullletCoroutine()
    {
        Debug.Log("Shot");
        _isSetBullet = true;
        //発射される前
        Vector3 v;
        float d;
        while (true)
        {
            v = (transform.position - _NextShotposition);
            d = v.sqrMagnitude;
            Debug.Log(d + " " + _NextShotposition);
            if (d < 0.1f)
            {
                //ここで手元に戻ってることにする
                break;
            }

            //移動
            transform.position = Vector3.Lerp(transform.position, _NextShotposition, _SpeedRatio);

            yield return null;
        }
    }

    #endregion
}
