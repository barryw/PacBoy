using System;
using System.Collections;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// Give us a nice, easy way of waiting around for a specified
    /// number of seconds and then calling an action
    /// </summary>
    /// <param name="seconds">The number of seconds to wait</param>
    /// <param name="action">The action to call when the time has elapsed</param>
    public void Wait(float seconds, Action action) {
        StartCoroutine (_wait (seconds, action));
    }

    private static IEnumerator _wait(float time, Action callback) {
        yield return new WaitForSeconds (time);
        callback ();
    }
}

