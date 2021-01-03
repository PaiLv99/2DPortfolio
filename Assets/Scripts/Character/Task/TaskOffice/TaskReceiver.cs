using UnityEngine;

public abstract class TaskReceiver<Task> : MonoBehaviour, IRecevier where Task : BaseTask
{
    public abstract void Execute(Task task);

    public void Init(TaskMng taskMng)
    {
        taskMng.ReceiverRegister<Task>(this);
    }

    void IRecevier.Handle(BaseTask message)
    {
        Execute((Task)message);
    }
}


