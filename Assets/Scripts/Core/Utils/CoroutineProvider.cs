using System.Collections;
using UnityEngine;

namespace Core.Utils
{
    /// <summary>
    /// Can be injected via Zenject and allows to use Coroutines in plain C# classes
    /// </summary>
    public class CoroutineProvider : MonoBehaviour
    {
        public new Coroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = base.StartCoroutine(routine);
            return coroutine;
        }
        
        public new void StopCoroutine(IEnumerator routine)
        {
            if (WasDisposed())
                return;

            base.StopCoroutine(routine);
        }

        public new void StopCoroutine(Coroutine routine)
        {
            if (WasDisposed())
                return;

            base.StopCoroutine(routine);
        }

        public void Dispose()
        {
            if (WasDisposed())
                return;

            StopAllCoroutines();

            if (Application.isPlaying)
                DestroyImmediate(gameObject);
            else
                Destroy(gameObject);
        }

        private bool WasDisposed()
        {
            // Yeap... this is the way to check if a MonoBehaviour was destroyed via a Destroy method
            return this == null;
        }
    }
}