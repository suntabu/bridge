using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityToolbag;

public class CameraActions : CacheBehaviour
{
    public static void FocusAt(Camera camera, Vector3 pos, Vector3 dir, float distance, float t,
        Action<Camera> onFinished)
    {
        camera.transform.DOMove(pos - dir.normalized * distance, t);
        camera.transform.DORotate(Quaternion.LookRotation(dir).eulerAngles, t)
            .OnComplete((() =>
            {
                if (onFinished != null)
                {
                    onFinished(camera);
                }
            }));
    }

    public static IEnumerator FocusAtCoroutine(Camera camera, Vector3 pos, Vector3 dir, float distance, float t,
        Action<Camera> onFinished)
    {
        camera.transform.DOMove(pos - dir.normalized * distance, t);
        yield return camera.transform.DORotate(Quaternion.LookRotation(dir).eulerAngles, t).WaitForCompletion();

        if (onFinished != null)
        {
            onFinished(camera);
        }

        yield return null;
    }
}