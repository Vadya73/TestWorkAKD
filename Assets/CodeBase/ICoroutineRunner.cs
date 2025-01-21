using System.Collections;
using UnityEngine;

namespace CodeBase
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}