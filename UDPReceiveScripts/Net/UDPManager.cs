using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
public class UDPManager : MonoBehaviour
{
    class UDPData
    {
        private UdpClient udpClient;
        public UdpClient UDPClient
        {
            get { return udpClient; }
        }
        private readonly IPEndPoint endPoint;
        public IPEndPoint EndPoint
        {
            get { return endPoint; }
        }
        //构造函数
        public UDPData(IPEndPoint endPoint, UdpClient udpClient)
        {
            this.endPoint = endPoint;
            this.udpClient = udpClient;
        }
    }
    public static UDPManager m_instance;
    string receiveData = string.Empty;
    private Action<string> ReceiveCallBack = null;
    private Thread RecviveThread;
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        //开启接收消息线程
        //ThreadRecvive();
    }
    private void Update()
    {
        if (ReceiveCallBack != null &&
        !string.IsNullOrEmpty(receiveData))
        {
            //调用处理函数去数据进行处理
            ReceiveCallBack(receiveData);
            //使用之后清空接受的数据
            receiveData = string.Empty;
        }
    }
    public void SetReceiveCallBack(Action<string> action)
    {
        ReceiveCallBack = action;
    }
    UDPData data;
    ///

    /// 开始线程接收
    /// 
    public void ThreadRecvive()
    {
        //开一个新线程接收UDP发送的数据
        RecviveThread = new Thread(() =>
        {
            //实例化一个IPEndPoint，任意IP和对应端口 端口自行修改
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 5002);
            UdpClient udpReceive = new UdpClient(endPoint);
            data = new UDPData(endPoint, udpReceive);
            //开启异步接收
            udpReceive.BeginReceive(CallBackRecvive, data);
        })
        {
            //设置为后台线程
            IsBackground = true
        };
        //开启线程
        RecviveThread.Start();
    }
    ///
    /// 异步接收回调
    /// 
    /// 
    private void CallBackRecvive(IAsyncResult ar)
    {
        try
        {
            //将传过来的异步结果转为我们需要解析的类型
            UDPData state = ar.AsyncState as UDPData;
            IPEndPoint ipEndPoint = state.EndPoint;
            //结束异步接受 不结束会导致重复挂起线程卡死
            byte[] data = state.UDPClient.EndReceive(ar, ref ipEndPoint);
            //解析数据 编码自己调整暂定为默认 依客户端传过来的编码而定
            receiveData = Encoding.Default.GetString(data);
            Debug.Log(receiveData);
            //数据的解析再Update里执行 Unity中Thread无法调用主线程的方法
            //再次开启异步接收数据
            state.UDPClient.BeginReceive(CallBackRecvive, state);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    ///
    /// 发送UDP信息
    /// 
    /// 发送地址
    /// 发送端口
    /// 需要发送的信息
    public void UDPSendMessage(string remoteIP, int remotePort, string message)
    {
        //将需要发送的内容转为byte数组 编码依接收端为主，自行修改
        //byte[] sendbytes = Encoding.GetEncoding("GB2312").GetBytes(message);
        byte[] sendbytes = Encoding.Default.GetBytes(message);
        IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        UdpClient udpSend = new UdpClient();
        //发送数据到对应目标
        udpSend.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
        print("IP=" + remoteIP + ";remotePort=" + remotePort + ";message=" + message);
        //关闭
        udpSend.Close();
    }
    //private void OnApplicationQuit()
    //{
    //    OnDestroy();
    //}
    private void OnDestroy()
    {
        print("关闭UDP连接");
        if (RecviveThread != null)
        {
            RecviveThread.Abort();
        }
        if (data != null || data.UDPClient != null)
        {
            data.UDPClient.Close();
        }
    }
}
