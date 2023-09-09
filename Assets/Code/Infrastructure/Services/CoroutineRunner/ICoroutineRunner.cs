using System.Collections;
using UnityEngine;

namespace Code.Infrastructure.Services.CoroutineRunner
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}