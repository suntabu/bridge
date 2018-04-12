using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;
using UnityToolbag.CoordinateSystems.SphericalCoordinate;

public class SceneController : CacheBehaviour
{
    private static readonly Vector3 pos = new Vector3(0, 0.121f, 0);
    private static readonly Vector3 dir = new Vector3(1, -1, 0);


    private Vector3 mCurrentFocus;

    private static readonly FocusInfo[] infos = new[]
    {
        new FocusInfo()
        {
            pos = new Vector3()
        },
    };


    struct FocusInfo
    {
        public Vector3 pos;
        public Vector3 dir;
        public string desc;
    }


    IEnumerator Start()
    {
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
    }

    private const float RATIO = 0.005f;

    private SphericalCoordinateSystem.SphericalCoordinate mSphericalPos;
    private Vector3 mLastPos;

    void Update()
    {
        if (Input.GetMouseButton(0))
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
            camera.transform.localPosition = Vector3.Lerp(a, b, Time.deltaTime * 3);
            camera.transform.LookAt(mCurrentFocus);
        }

        if (Input.GetMouseButtonUp(0))
        {
            mLastPos = Vector3.zero;
        }
    }
}