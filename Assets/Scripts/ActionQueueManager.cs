using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum ActionQueueType
{
    Explode,
    Move
}

public struct ActionQueueRequest
{
    public ActionQueueType Type;
    public int ChainNumber;
    public Func<CancellationToken, UniTask> Task;

    public ActionQueueRequest(ActionQueueType type, Func<CancellationToken, UniTask> task)
    {
        Type = type;
        Task = task;
    }
}

public class ActionQueueManager : MonoBehaviour
{
    // Queue storing functions that return a UniTask
    private readonly Queue<ActionQueueRequest> taskQueue = new();
    private bool isProcessing = false;
    private CancellationTokenSource cts;

    private void Awake()
    {
        cts = new CancellationTokenSource();
    }

    // Add a new async task to the back of the queue
    public void EnqueueTask(ActionQueueRequest request)
    {
        taskQueue.Enqueue(request);
    }

    public void ClearQueue()
    {
        taskQueue.Clear();
    }

    public async UniTaskVoid ProcessQueueLoop()
    {
        if (isProcessing) return;
        isProcessing = true;

        while (taskQueue.Count > 0)
        {
            // Dequeue the next task
            var nextRequest = taskQueue.Dequeue();
            
            var nextTask = nextRequest.Task;
            try
            {
                // Run and completely await the task before continuing the loop
                // Depending on the type of task we have here, we need to send it to the right places

                switch (nextRequest.Type)
                {
                    case ActionQueueType.Explode:
                        case ActionQueueType.Move:
                        // Send this to the animator
                        break;
                    default:
                        break;
                }
                
                await nextTask.Invoke(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Task queue processing was canceled.");
                break;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        isProcessing = false;
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}