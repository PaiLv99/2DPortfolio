using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class MessageQueue : MonoBehaviour
//{
//    public static MessageQueue _instance;

//    public static MessageQueue Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = FindObjectOfType<MessageQueue>();
//                if (_instance == null)
//                {
//                    GameObject go = new GameObject("MessageQueue");
//                    go.AddComponent<MessageQueue>();

//                    return _instance;
//                }
//            }
//            return _instance;
//        }
//    }

//    private Dictionary<Type, List<IRecevier>> _recivers;

//    private void Awake()
//    {
//        _recivers = new Dictionary<Type, List<IRecevier>>();
//    }

//    public void Register<TMessage>(IRecevier reciver) where TMessage : ITask
//    {
//        if (!_recivers.ContainsKey(typeof (TMessage)))
//        {
//            _recivers[typeof(TMessage)] = new List<IRecevier>();
//        }

//        var list = _recivers[typeof(TMessage)];
//        if (!list.Contains(reciver))
//            list.Add(reciver);
//    }

//    public void Post(BaseMessage message)
//    {
//        if (!_recivers.ContainsKey(message.GetType()))
//        {
//            return;
//        }

//        var list = _recivers[message.GetType()];
//        foreach(var reciver in list)
//        {
//            reciver.Handle(message);
//        }
//    }
//}
