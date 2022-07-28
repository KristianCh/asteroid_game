using System;
using System.Collections;
using UnityEngine;

namespace Utilities
{
    public static class Utility
    {
        #region InvokeAfter

            public static void InvokeAfter(this MonoBehaviour monoBehaviour, Action func, float delayTime)
            {
                monoBehaviour.StartCoroutine(InvokeRoutine(func, delayTime));
            }
            
            private static IEnumerator InvokeRoutine(Action func, float delayTime)
            {
                yield return new WaitForSeconds(delayTime);
                func();
            }

        #endregion
        
    }
}