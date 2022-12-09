using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UDPRecvice : MonoBehaviour
{
    void Start()
    {
        //��ȡ��UDPManager �ű��ڲ��Լ��������߳������ٴο���
        UDPManager manager = GetComponent<UDPManager>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<UDPManager>();
        }
        //���úý����ķ���
        manager.SetReceiveCallBack(ReceiveUDPMessage);
        manager.ThreadRecvive();
    }
    void ReceiveUDPMessage(string receiveData)
    {
        //�������Ϳ��Լ���ν�����
        Debug.Log("���ݽ��� "+receiveData);
    }
}