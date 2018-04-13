using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityToolbag;
using UnityToolbag.CoordinateSystems.SphericalCoordinate;
using UnityToolbag.EventTrigger;
using UnityToolBag.Utils;

public class SceneController : CacheBehaviour
{
    private static readonly Vector3 pos = new Vector3(0, 0.121f, 0);
    private static readonly Vector3 dir = new Vector3(1, -1, 0);


    private Vector3 mCurrentFocus;

    private static readonly FocusInfo[] infos = new[]
    {
        new FocusInfo()
        {
            pos = new Vector3(7.48f, 1.82f, -19.01f),
            desc = "bad...",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(44.48f, 7.13f, -19.01f),
            desc = "bad...",
            dir = new Vector3(-1, 0, 0),
            dist = 10
        },
    };


    private List<RectTransform> m_Icons = new List<RectTransform>();

    struct FocusInfo
    {
        public Vector3 pos;
        public Vector3 dir;
        public string desc;
        public float dist;
    }


    public RectTransform FloatingRoot;
    public RectTransform BackBtn;


    IEnumerator Start()
    {
        BackBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        mCurrentFocus = pos;
        yield return CameraActions.FocusAtCoroutine(camera, pos, dir, 80, .5f, t =>
        {
            ShowUI();

            mSphericalPos = SphericalCoordinateSystem.FromCartesian(camera.transform.localPosition - mCurrentFocus);
        });

        yield return null;
    }

    void ShowUI()
    {
        EventTriggerListener.Get(BackBtn.gameObject).OnClick += (go, data) =>
        {
            mCurrentFocus = pos;
            mSphericalPos.r = 80;


            CameraActions.FocusAt(camera, mCurrentFocus, dir, mSphericalPos.r, .5f, t =>
            {
                foreach (var mIcon in m_Icons)
                {
                    mIcon.GetComponent<Image>().DOFade(1, .5f);
                }
            });
        };


        m_Icons.Clear();
        var floatIconPrefab = Resources.Load<GameObject>("FloatIcon");
        foreach (var focusInfo in infos)
        {
            var go = Instantiate(floatIconPrefab);
            go.transform.SetParent(FloatingRoot);

            m_Icons.Add(go.GetComponent<RectTransform>());

            EventTriggerListener.Get(go).OnClick += (o, data) =>
            {
                BackBtn.GetComponent<Image>().DOFade(1, .5f);

                foreach (var mIcon in m_Icons)
                {
                    mIcon.GetComponent<Image>().DOFade(0, .5f);
                }

                mSphericalPos.r = focusInfo.dist;
                mCurrentFocus = focusInfo.pos;
                CameraActions.FocusAt(Camera.main, focusInfo.pos, focusInfo.dir, focusInfo.dist, .5f, t => { });
            };
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (var index = 0; index < m_Icons.Count; index++)
        {
            var focusInfo = infos[index];
            var go = m_Icons[index];
            var pos = Camera.main.WorldToScreenPoint(focusInfo.pos) - .5f * new Vector3(Screen.width, Screen.height);
            go.anchoredPosition = pos;
        }
    }

    private const float RATIO = 0.005f;

    private SphericalCoordinateSystem.SphericalCoordinate mSphericalPos;
    private Vector3 mLastPos;
    private bool mIsDown;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InputUtils.CheckMouseOnUI())
        {
            mIsDown = true;
        }

        if (Input.GetMouseButton(0) && mIsDown)
        {
            if (mLastPos != Vector3.zero)
            {
                var delta = RATIO * (Input.mousePosition - mLastPos);

                mSphericalPos.phi -= delta.x;
                mSphericalPos.theta += delta.y;

                if (mSphericalPos.theta < 0.15f * Mathf.PI)
                {
//                    Debug.LogError("Cs");
                    mSphericalPos.theta = Mathf.PI * 0.15f;
                }

//                Debug.Log(delta + "   " + mSphericalPos);
            }

            mLastPos = Input.mousePosition;
            var a = camera.transform.localPosition;
            var b = SphericalCoordinateSystem.ToCartesian(mSphericalPos) + mCurrentFocus;

            if (b.y <= mCurrentFocus.y)
            {
                b = new Vector3(b.x, mCurrentFocus.y, b.z);
            }
//            camera.transform.localPosition = b;
            camera.transform.localPosition = Vector3.Lerp(a, b, Time.deltaTime * 4);
            camera.transform.LookAt(mCurrentFocus);
        }

        if (Input.GetMouseButtonUp(0))
        {
            mLastPos = Vector3.zero;
            mIsDown = false;
        }

        UpdateUI();
    }
}