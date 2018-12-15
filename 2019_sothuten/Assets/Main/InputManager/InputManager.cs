using System.Collections;
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

    #region Properties
    public Vector3 HandPosition { get; private set; }

    public Vector3 ShotPosition { get; private set; }

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
        yield return null;
    }

    private IEnumerator InputCoroutine_Mouse()
    {
        Ray ray;
        RaycastHit hit;
        while (true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10.0f);

            //手の座標
            HandPosition = ray.GetPoint(1.0f);

            //トリガー入力
            TriggerDown = Input.GetMouseButtonDown(0);

            //着弾点の座標
            if (Physics.Raycast(ray, out hit, 100.0f, 1 << (int)(GameEnum.Layers.Filed)))
            {
                ShotPosition = hit.point;
                Debug.Log(ShotPosition);
            }

            yield return null;
        }
    }

}
