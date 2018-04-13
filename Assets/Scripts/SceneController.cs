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
            pos = new Vector3(-58.91f, 1.82f, -7.35f),
            name = "QDCZ01",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(-43.1f, 1.82f, -10.38f),
            name = "QDCZ02",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(-28.32f, 1.82f, -13.76f),
            name = "QDCZ03",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(7.48f, 1.82f, -19.01f),
            name = "QDCZ04",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(21.79f, 1.82f, -21.6f),
            name = "QDCZ05",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(38.05f, 1.82f, -24.249f),
            name = "QDCZ06",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, 1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(41.7f, 2.3f, -3.751f),
            name = "QDCZ07",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, -1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(25.4f, 2.3f, -0.6f),
            name = "QDCZ08",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, -1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(10.476f, 2.3f, 1.658f),
            name = "QDCZ09",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, -1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(-24.7f, 2.3f, 7.692f),
            name = "QDCZ10",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, -1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(-39.5f, 2.3f, 10.6f),
            name = "QDCZ11",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, -1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(-55.3f, 2.3f, 13.11f),
            name = "QDCZ12",
            desc = "监测数据正常",
            dir = new Vector3(0, 0, -1),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(44.48f, 7.13f, -19.01f),
            name = "QDCZ13",
            desc = "监测数据正常",
            dir = new Vector3(-1, 0, 0),
            dist = 10
        },

        new FocusInfo()
        {
            pos = new Vector3(-59.29f, 8.4f, 2.81f),
            name = "QDCZ014",
            desc = "监测数据正常",
            dir = new Vector3(1, 0, 0),
            dist = 10
        },
        new FocusInfo()
        {
            pos = new Vector3(-4.5f, 18.64f, -5.6f),
            name = "QDCZ15",
            desc = "监测数据正常",
            dir = new Vector3(1, -1, 0),
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
        public string name;
        public string type;
    }


    public RectTransform FloatingRoot;
    public RectTransform BackBtn;
    public RectTransform bridgeInfo;
    public Image InfoBtn;
    public InfoPanelScript panelScript;

    IEnumerator Start()
    {
        bridgeInfo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        BackBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        panelScript.gameObject.SetActive(false);
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
        EventTriggerListener.Get(InfoBtn.gameObject).OnClick += (go, data) =>
        {
            bridgeInfo.GetComponent<Image>().DOFade(1, .3f);
        };


        EventTriggerListener.Get(BackBtn.gameObject).OnClick += (go, data) =>
        {
            mCurrentFocus = pos;
            mSphericalPos.r = 80;
            panelScript.gameObject.SetActive(false);
            BackBtn.GetComponent<Image>().DOFade(0, .3f);
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
                panelScript.gameObject.SetActive(true);

                panelScript.SetText(focusInfo.name, focusInfo.pos.y + "m", focusInfo.desc);


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

            bridgeInfo.GetComponent<Image>().DOFade(0, .8f);
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