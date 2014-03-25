using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiteDevelop.Framework
{

    public class EmptyEnumerator<T> : IEnumerator<T>, IEnumerator
    {
        public T Current
        {
            get { return default(T); }
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return default(T); }
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }
    }
}
