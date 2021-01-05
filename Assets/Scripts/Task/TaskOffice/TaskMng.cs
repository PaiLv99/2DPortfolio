using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskMng
{
    protected Queue<BaseTask> _tasks = new Queue<BaseTask>();
    protected Dictionary<Type, IRecevier> _receivers = new Dictionary<Type, IRecevier>();
    public bool IsActive { get; set; } = false;

    public void TaskRegister(BaseTask task)
    {
        _tasks.Enqueue(task);
    }

    public void ReceiverRegister<Task>(IRecevier receiver) where Task : BaseTask
    {
        if (!_receivers.ContainsKey(typeof(Task)))
            _receivers.Add(typeof(Task), receiver);
    }

    public void Execute()
    {
        if (_tasks.Count > 0)
        {
            BaseTask task = _tasks.Dequeue();
            _receivers[task.GetType()].Handle(task);
        }
    }

    public void CurrentExecute(BaseTask task)
    {
        if (_receivers.ContainsKey(task.GetType()))
            _receivers[task.GetType()].Handle(task);
    }

    public void TaskClear()
    {
        _tasks.Clear();
    }
}



