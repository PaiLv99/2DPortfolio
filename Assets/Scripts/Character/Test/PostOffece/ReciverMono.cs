using UnityEngine;

//public abstract class ReciverMono<TMessage> : MonoBehaviour, IRecevier where TMessage : ITask
//{
//    public abstract void Handle(TMessage message);
//    protected abstract void OnAwake();

//    void IRecevier.Handle(ITask message)
//    {
//        Handle((TMessage)message);
//    }

//    private void Awake()
//    {
//        MessageQueue.Instance.Register<TMessage>(this);
//        OnAwake();
//    }
//}
