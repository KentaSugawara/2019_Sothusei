﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField]
    public static InputManager Instance;

    [SerializeField]
    private bool _MouseMode = true;

    [SerializeField]
    private Transform _LeftHandTransform;

    [SerializeField]
    private Transform _RightHandTransform;

    [SerializeField]
    private Transform _ShotPosObj;

    [SerializeField]
    private Transform _HandPosObj;

    [SerializeField]
    private float _SpeedSqrThreshold;

    #region Properties
    public Vector3 HandPosition { get; private set; }

    public Vector3 ShotPosition { get; private set; }

    public Vector3 HandFieldPosition { get; private set; }

    public bool TriggerDown { get; private set; }

    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    void Start () {
		if (!_MouseMode)
        {
            StartCoroutine(InputCoroutine());
        }
        else
        {
            StartCoroutine(InputCoroutine_Mouse());
        }
	}

    private IEnumerator InputCoroutine()
    {
        Ray ray;
        Ray Handray;
        Vector3 HandScreenPos = Vector3.zero;
        RaycastHit hit;

        float SqrHandVelocity;
        Vector3 HandLastPosition = Vector3.zero;
        while (true)
        {
            HandPosition = _RightHandTransform.position;


            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            //Debug.DrawRay(ray.origin, ray.direction * 10.0f);
            if (Physics.Raycast(ray, out hit, 100.0f, 1 << (int)(GameEnum.Layers.Filed)))
            {
                ShotPosition = hit.point + Vector3.up * 0.5f;
                HandScreenPos = Camera.main.WorldToScreenPoint(HandPosition);
                _ShotPosObj.position = ShotPosition;
            }


            Handray = Camera.main.ScreenPointToRay(HandScreenPos);

            if (Physics.Raycast(Handray, out hit, 100.0f, 1 << (int)(GameEnum.Layers.Filed)))
            {
                HandFieldPosition = hit.point;
                _HandPosObj.position = hit.point;
            }

            SqrHandVelocity = Vector3.SqrMagnitude((HandPosition - HandLastPosition) / Time.deltaTime);
            TriggerDown = SqrHandVelocity >= _SpeedSqrThreshold;

            HandLastPosition = HandPosition;

            yield return null;
        }
    }

    private IEnumerator InputCoroutine_Mouse()
    {
        Ray ray;
        RaycastHit hit;
        while (true)
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
            Debug.DrawRay(ray.origin, ray.direction * 10.0f);

            //手の座標
            HandPosition = ray.GetPoint(1.0f);

            //トリガー入力
            TriggerDown = Input.GetMouseButtonDown(0);

            //着弾点の座標
            if (Physics.Raycast(ray, out hit, 100.0f, 1 << (int)(GameEnum.Layers.Filed)))
            {
                ShotPosition = hit.point;
                HandFieldPosition = hit.point;
                _ShotPosObj.position = hit.point;
                _HandPosObj.position = hit.point;
            }

            yield return null;
        }
    }

}
