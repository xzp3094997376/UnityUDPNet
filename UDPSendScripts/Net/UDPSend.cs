using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UDPSend : MonoBehaviour
{
    UDPManager manager;
    // Start is called before the first  update
    void Start()
    {
        //获取到UDPManager 脚本内部自己开启了线程无需再次开启
        manager = GetComponent<UDPManager>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<UDPManager>();
        }
        //设置好解析的方法
    }
    // Update is called once per 
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            manager.UDPSendMessage("127.0.0.1", 5002, "点击了鼠标右键!!");
            //print("点击了鼠标右键");
        }
    }
}