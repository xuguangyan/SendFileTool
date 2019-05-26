using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SendFileCommon
{
    /// <summary>
    /// 网络连接类（封装了TCP、UDP传输协议）
    /// </summary>
    public class NetClient
    {
        private string netType;         //传输协议类型
        private UdpClient udpClient;    //UDP传输对象
        private TcpClient tcpClient;    //TCP传输对象
        private TcpListener tcpListener;//TCP监听器

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ep">绑定本地的网络结点</param>
        /// <param name="type">传输类型(TCP、UDP)</param>
        /// <param name="listener">是否作为监听对象</param>
        public NetClient(IPEndPoint ep, string type = "UDP", bool listener = false)
        {
            netType = type.ToUpper();
            if (netType.Equals("TCP"))
            {
                if (listener)
                {
                    //TCP协议作为服务器端有监听功能
                    tcpListener = new TcpListener(ep);
                    tcpListener.Start();
                }
                else
                {
                    tcpClient = new TcpClient(ep);
                }
            }
            else
            {
                udpClient = new UdpClient(ep);
            }
        }

        /// <summary>
        /// 连接远程终端/服务器
        /// </summary>
        /// <param name="ep">远程终端</param>
        public void Connect(IPEndPoint ep)
        {
            if (netType.Equals("TCP"))
            {
                tcpClient.Connect(ep);
            }
            else
            {
                udpClient.Connect(ep);
            }
        }

        /// <summary>
        /// 接收新客户端连接（阻塞）
        /// </summary>
        public void AcceptNew() 
        {
            if (netType.Equals("TCP"))
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                }

                //接收客户端连接（阻塞）
                tcpClient = tcpListener.AcceptTcpClient();
            }
        }

        /// <summary>
        /// 接收字节流数据
        /// </summary>
        /// <param name="ep">远程终端</param>
        /// <returns></returns>
        public byte[] Receive(ref IPEndPoint ep)
        {
            byte[] buffer = new byte[0];

            if (netType.Equals("TCP"))
            {
                NetworkStream ns = tcpClient.GetStream();
                ep = (IPEndPoint)tcpClient.Client.RemoteEndPoint;

                if (ns.CanRead)
                {
                    //接收数据（阻塞）

                    //读取前4个字节
                    buffer = new byte[4];
                    int readLen = ns.Read(buffer, 0, buffer.Length);
                    if (readLen < 4)
                    {
                        return new byte[0];
                    }
                    //获取数据长度
                    int dataSize = BitConverter.ToInt32(buffer, 0);
                    if (dataSize < 4)
                    {
                        return new byte[0];
                    }
                    //读取剩下字节
                    dataSize -= 4;
                    buffer = new byte[dataSize];
                    int offset = 0;
                    while (offset < dataSize)
                    {
                        readLen = ns.Read(buffer, offset, dataSize - offset);
                        offset += readLen;
                    }
                }
            }
            else
            {
                //接收数据（阻塞）
                buffer = udpClient.Receive(ref ep);
                int readLen = buffer.Length;
                if (readLen < 4)
                {
                    return new byte[0];
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buffer, 4, readLen - 4);
                    buffer = ms.ToArray();
                }
            }

            return buffer;
        }

        /// <summary>
        /// 发送字节流数据
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="bytes">字节数</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int bytes)
        {
            if (netType.Equals("TCP"))
            {
                NetworkStream ns = tcpClient.GetStream();
                ns.Write(buffer, 0, bytes);
            }
            else
            {
                //IP数据报的最大长度是65535字节（64K）
                //Udp协议中，IP数据报是由 IP首部（20字节）+ Udp数据报（包含8个字节的Udp首部和实际数据）组成
                //所以就是用65535-20-8=65507这是Udp数据的最大值
                udpClient.Send(buffer, bytes);
            }
            return 0;
        }

        /// <summary>
        /// 发送字节流数据
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="bytes">字节数</param>
        /// <param name="ep">远程终端</param>
        /// <returns></returns>
        public int Send(byte[] buffer, int bytes, IPEndPoint ep)
        {
            if (netType.Equals("TCP"))
            {
                tcpClient.Connect(ep);
                NetworkStream ns = tcpClient.GetStream();
                ns.Write(buffer, 0, bytes);
            }
            else
            {
                udpClient.Send(buffer, bytes , ep);
            }
            return 0;
        }

        /// <summary>
        /// 关闭网络连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (udpClient != null)
                {
                    udpClient.Close();
                }
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                }
            }
            catch { }
        }
    }
}
