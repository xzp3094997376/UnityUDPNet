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
        //���캯��
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
        //����������Ϣ�߳�
        //ThreadRecvive();
    }
    private void Update()
    {
        if (ReceiveCallBack != null &&
        !string.IsNullOrEmpty(receiveData))
        {
            //���ô�����ȥ���ݽ��д���
            ReceiveCallBack(receiveData);
            //ʹ��֮����ս��ܵ�����
            receiveData = string.Empty;
        }
    }
    public void SetReceiveCallBack(Action<string> action)
    {
        ReceiveCallBack = action;
    }
    UDPData data;
    ///

    /// ��ʼ�߳̽���
    /// 
    public void ThreadRecvive()
    {
        //��һ�����߳̽���UDP���͵�����
        RecviveThread = new Thread(() =>
        {
            //ʵ����һ��IPEndPoint������IP�Ͷ�Ӧ�˿� �˿������޸�
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 5002);
            UdpClient udpReceive = new UdpClient(endPoint);
            data = new UDPData(endPoint, udpReceive);
            //�����첽����
            udpReceive.BeginReceive(CallBackRecvive, data);
        })
        {
            //����Ϊ��̨�߳�
            IsBackground = true
        };
        //�����߳�
        RecviveThread.Start();
    }
    ///
    /// �첽���ջص�
    /// 
    /// 
    private void CallBackRecvive(IAsyncResult ar)
    {
        try
        {
            //�����������첽���תΪ������Ҫ����������
            UDPData state = ar.AsyncState as UDPData;
            IPEndPoint ipEndPoint = state.EndPoint;
            //�����첽���� �������ᵼ���ظ������߳̿���
            byte[] data = state.UDPClient.EndReceive(ar, ref ipEndPoint);
            //�������� �����Լ������ݶ�ΪĬ�� ���ͻ��˴������ı������
            receiveData = Encoding.Default.GetString(data);
            Debug.Log(receiveData);
            //���ݵĽ�����Update��ִ�� Unity��Thread�޷��������̵߳ķ���
            //�ٴο����첽��������
            state.UDPClient.BeginReceive(CallBackRecvive, state);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    ///
    /// ����UDP��Ϣ
    /// 
    /// ���͵�ַ
    /// ���Ͷ˿�
    /// ��Ҫ���͵���Ϣ
    public void UDPSendMessage(string remoteIP, int remotePort, string message)
    {
        //����Ҫ���͵�����תΪbyte���� ���������ն�Ϊ���������޸�
        //byte[] sendbytes = Encoding.GetEncoding("GB2312").GetBytes(message);
        byte[] sendbytes = Encoding.Default.GetBytes(message);
        IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        UdpClient udpSend = new UdpClient();
        //�������ݵ���ӦĿ��
        udpSend.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
        print("IP=" + remoteIP + ";remotePort=" + remotePort + ";message=" + message);
        //�ر�
        udpSend.Close();
    }
    //private void OnApplicationQuit()
    //{
    //    OnDestroy();
    //}
    private void OnDestroy()
    {
        print("�ر�UDP����");
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
