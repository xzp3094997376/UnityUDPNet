using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UDPSend : MonoBehaviour
{
    UDPManager manager;
    // Start is called before the first  update
    void Start()
    {
        //��ȡ��UDPManager �ű��ڲ��Լ��������߳������ٴο���
        manager = GetComponent<UDPManager>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<UDPManager>();
        }
        //���úý����ķ���
    }
    // Update is called once per 
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            manager.UDPSendMessage("127.0.0.1", 5002, "���������Ҽ�!!");
            //print("���������Ҽ�");
        }
    }
}