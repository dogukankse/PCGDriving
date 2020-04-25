using System;
using UnityEngine;
using System.Collections;

namespace _Scripts
{
    public class DelayExecutor
    {
        private bool isCoroutineExecuting;
        private Action task;
        private float time;

        public DelayExecutor(Action task, float time)
        {
            this.time = time;
            this.task = task;
        }

        public IEnumerator getEnumerator()
        {
            if (isCoroutineExecuting)
                yield break;
            isCoroutineExecuting = true;
            yield return new WaitForSeconds(time);
            task();
            isCoroutineExecuting = false;
        }
    }

    public class RepeatExecutor
    {
        private bool isCoroutineExecuting;
        private Action<int> task;
        private float time;
        private bool finished;
        private int called;

        public RepeatExecutor(Action<int> task, float intervalTime)
        {
            this.time = intervalTime;
            this.task = task;
        }

        public void finish()
        {
            finished = true;
        }

        public IEnumerator getEnumerator()
        {
            while (!finished)
            {
                yield return new WaitForSeconds(time);
                task(called);
                called++;
            }
        }
    }
}