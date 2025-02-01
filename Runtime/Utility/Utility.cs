using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    public static class Utility
    {
        public static Coroutine Invoke(this MonoBehaviour mb, Action f, float delay)
        {
            return mb.StartCoroutine(InvokeRoutine(f, delay));
        }
 
        private static IEnumerator InvokeRoutine(System.Action f, float delay)
        {
            yield return new WaitForSeconds(delay);
            f();
        }
    
    }
}