using System;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace.Simplified;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class ActionQueueManager : MonoBehaviour
    {
        private Queue<Func<CancellationToken, Awaitable>> taskQueue = new();
        private bool isProcessing = false;
        
        public int QueueCount => taskQueue.Count;

        private CancellationToken destroyToken => destroyCancellationToken;

        public void EnqueueTask(Func<CancellationToken, Awaitable> taskFactory)
        {
            taskQueue.Enqueue(taskFactory);
        }

        public async Awaitable ProcessNextQueueAsync()
        {
            if (!isProcessing) return;
            isProcessing = true;
            try
            {
                var taskFactory = taskQueue.Dequeue();
                await taskFactory(destroyToken);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Task Cancelled");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            isProcessing = false;
        }
    }
}