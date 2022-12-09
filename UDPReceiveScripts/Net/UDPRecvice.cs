using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UDPRecvice : MonoBehaviour
{
    void Start()
    {
        //获取到UDPManager 脚本内部自己开启了线程无需再次开启
        UDPManager manager = GetComponent<UDPManager>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<UDPManager>();
        }
        //设置好解析的方法
        manager.SetReceiveCallBack(ReceiveUDPMessage);
        manager.ThreadRecvive();
    }
    void ReceiveUDPMessage(string receiveData)
    {
        //接下来就看自己如何解析了
        Debug.Log("数据解析 "+receiveData);
    }
}