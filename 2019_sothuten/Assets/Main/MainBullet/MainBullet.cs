using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet : MonoBehaviour {
    public static MainBullet Instance;

    [SerializeField]
    private float _ShotNeedSeconds;

    [SerializeField]
    private EasingLerps.EasingLerpsType _Shot_EasingType;

    [SerializeField]
    private EasingLerps.EasingInOutType _Shot_InOutType;

    [Space(5)]
    [SerializeField]
    private float _ReturnNeedSeconds;

    [SerializeField]
    private EasingLerps.EasingLerpsType _Return_EasingType;

    [SerializeField]
    private EasingLerps.EasingInOutType _Return_InOutType;

    [Space(5)]
    [SerializeField]
    private GameObject _Prefab_Bomb;

    [SerializeField]
    private TrailRenderer _TrailRenderer;

    [SerializeField]
    private int _MaxBombs = 5;

    [SerializeField]
    private float _BulletSettingPositionSeconds;

    [SerializeField]
    private float _BulletMoveSeconds;

    private List<Vector3> _MovePositionList = new List<Vector3>();
    private MainBullet_Bomb[] _BombInstances;
    private int _NextBombIndex = 0;
    private Rigidbody _Rigidbody;

    //手元にある状態
    private bool _isSetBullet;

    private Vector3 _NextShotposition;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        //爆弾を予め作っておく
        _BombInstances = new MainBullet_Bomb[_MaxBombs];
        for (int i = 0; i < _MaxBombs; ++i)
        {
            var obj = Instantiate(_Prefab_Bomb);
            _BombInstances[i] = obj.GetComponent<MainBullet_Bomb>();
        }

        StartCoroutine(RunningCoroutine());
    }

    private IEnumerator RunningCoroutine()
    {
        while (true)
        {
            yield return ReturnBullletCoroutine();
            yield return StayBullletCoroutine();
            yield return ShotBullletCoroutine();
            yield return BombBullletCoroutine();
            yield return BulletSettingPositionCoroutine();
            yield return BulletMoveCoroutine();
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
        //Vector3 v;
        //float d;
        //while (true)
        //{
        //    v = (transform.position - InputManager.Instance.HandPosition);
        //    d = v.sqrMagnitude;

        //    if (d < 0.01f)
        //    {
        //        ここで手元に戻ってることにする
        //        break;
        //    }

        //    //ここで手元に戻ってる
        //    yield return null;
        //}

        //移動
        Vector3 StartPos = transform.position;
        for (float t = 0.0f, e = 0.0f; t < _ReturnNeedSeconds; t += Time.deltaTime)
        {
            e = EasingLerps.EasingLerp(_Return_EasingType, _Return_InOutType, _ShotNeedSeconds, t / _ReturnNeedSeconds, 0.0f, 1.0f);
            _Rigidbody.MovePosition(Vector3.Lerp(StartPos, InputManager.Instance.HandPosition, e));
            yield return null;
        }
        _Rigidbody.MovePosition(InputManager.Instance.HandPosition);

       _isSetBullet = true;
    }

    /// <summary>
    /// 手元にある状態
    /// </summary>
    /// <returns></returns>
    private IEnumerator StayBullletCoroutine()
    {
        Debug.Log("Stay");
        _TrailRenderer.Clear();
        _TrailRenderer.enabled = false;
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
        //Vector3 v;
        //float d;
        //while (true)
        //{
        //    v = (transform.position - _NextShotposition);
        //    d = v.sqrMagnitude;
        //    if (d < 0.01f)
        //    {
        //        //ここで着弾した
        //        break;
        //    }

        //    //移動
        //    //_Rigidbody.MovePosition(Vector3.Lerp(transform.position, _NextShotposition, _ShotSpeedRatio));

        //    //移動


        //    yield return null;
        //}
        _MovePositionList.Clear();
        Vector3 StartPos = transform.position;
        for (float t = 0.0f, e = 0.0f; t < _ShotNeedSeconds; t += Time.deltaTime)
        {
            e = EasingLerps.EasingLerp(_Shot_EasingType, _Shot_InOutType, _ShotNeedSeconds, t / _ShotNeedSeconds, 0.0f, 1.0f);
            _Rigidbody.MovePosition(Vector3.Lerp(StartPos, _NextShotposition, e));
            //ここでも座標保存
            _MovePositionList.Add(InputManager.Instance.HandFieldPosition);
            yield return null;
        }

        _Rigidbody.MovePosition(_NextShotposition);

        //ここで着弾した
    }

    /// <summary>
    /// 着弾して爆発
    /// </summary>
    /// <returns></returns>
    private IEnumerator BombBullletCoroutine()
    {
        Debug.Log("Bomb");
        //発射される前
        //bool isWaiting = true;
        //_Bomb.StartBomb(() => isWaiting = false);
        //while (isWaiting) yield return null;

        _BombInstances[_NextBombIndex].StartBomb(_NextShotposition);

        ++_NextBombIndex;
        if (_NextBombIndex >= _BombInstances.Length)
        {
            _NextBombIndex = 0;
        }

        yield break;
        //ここで爆発が終了
    }


    /// <summary>
    /// 移動座標保存中
    /// </summary>
    /// <returns></returns>
    private IEnumerator BulletSettingPositionCoroutine()
    {
        Debug.Log("BulletSettingPosition");
        //_MovePositionList.Clear();
        for (float t = 0.0f; t < _BulletSettingPositionSeconds; t += Time.deltaTime)
        {
            _MovePositionList.Add(InputManager.Instance.HandFieldPosition);
            yield return null;
        }
    }

    /// <summary>
    /// 移動中
    /// </summary>
    /// <returns></returns>
    private IEnumerator BulletMoveCoroutine()
    {
        Debug.Log("BulletMoveCoroutine");
        int cntList = _MovePositionList.Count - 1;
        _Rigidbody.velocity = Vector3.zero;
        _TrailRenderer.enabled = true;
        //int Target = 0;
        for (float t = 0.0f; t < _BulletMoveSeconds; t += Time.deltaTime)
        {
            _Rigidbody.MovePosition(_MovePositionList[(int)(t / _BulletMoveSeconds * cntList)]);

            //Target = (int)((t / _BulletMoveSeconds) * cntList) + 1;
            //if (Target >= cntList) Target = cntList - 1;
            //_Rigidbody.AddForce((_MovePositionList[Target] - _MovePositionList[(int)(t / _BulletMoveSeconds * cntList)]).normalized * 100.0f, ForceMode.Impulse);

            yield return null;
        }
        _Rigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        var comp = other.GetComponent<MainObject>();
        if (comp != null)
        {
            comp.Get();
        }
    }

    #endregion
}
