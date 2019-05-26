using SendFileCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SendFileServer
{
    public partial class MainForm : Form
    {
        private NetClient receiveUpdClient;

        private Thread receiveThread;
        private Thread sendThread;

        private IPEndPoint remoteIpEndPoint;

        //是否已监听
        private bool bListen = false;

        //总耗时（毫秒）
        private long milliseconds = 0;
        //已完成字节数
        private long completeBytes = 0;
        //文件总字节数
        private long totalBytes = 0;
        //是否已完成发送
        private bool finished = false;
        //缓冲区大小（KB）
        private int buffersize = 0;
        //间歇时间（毫秒）
        private int interval = 0;
        //计数器
        private int counter = 0;
        private long countBytes = 0;

        //发送文件集合
        private SendFileInfo sendFileInfo;

        //ini配置结点名
        private string iniSession = "DS_SendFileServer";

        //关于窗体
        AboutForm aboutForm = new AboutForm();

        public MainForm()
        {
            InitializeComponent();

            //读取配置文件
            ReadConfig();

            //初始化数据
            ResourceManager rs = new ResourceManager("SendFileCommon.Resource", typeof(Resource).Assembly);
            picBoxClean.Image = (Image)rs.GetObject("clean");

            btnSendFile.Enabled = false;
            btnSendMsg.Enabled = false;
            btnPause.Enabled = false;
            rdBtnUDP.Enabled = true;
            rdBtnTCP.Enabled = true;
            trackBarInterval.Value = interval;
            lbMilliSecond.Text = interval + " ms";

            //初始化控件状态
            if (!finished && totalBytes > 0 && completeBytes > 0 && totalBytes > completeBytes)
            {
                btnSendFile.Text = "续传文件";
                if (completeBytes <= totalBytes)
                {
                    prgBar1.Value = (int)Math.Ceiling(100.0 * completeBytes / totalBytes);
                }
                else
                {
                    prgBar1.Value = 100;
                }
                prgBar1.Text = string.Format("发送完成{0} %", prgBar1.Value);

                lbFinish.Text = FileUtils.getFinished(completeBytes, totalBytes);
                lbSeconds.Text = FileUtils.getUseTime(milliseconds);
            }

            //启动自动保存定时器
            timerSaveCfg.Start();
        }

        //监听按钮
        private void btnListen_Click(object sender, EventArgs e)
        {
            if (!bListen)
            {
                // 创建接收套接字
                IPAddress localIp = IPAddress.Parse(txtIP.Text);
                IPEndPoint localIpEndPoint = new IPEndPoint(localIp, int.Parse(txtPort.Text));

                try
                {
                    //连接类型选择
                    if (rdBtnTCP.Checked)
                    {
                        receiveUpdClient = new NetClient(localIpEndPoint, "TCP", true);
                    }
                    else
                    {
                        receiveUpdClient = new NetClient(localIpEndPoint);
                    }
                }
                catch (Exception ex)
                {
                    Log("监听失败：" + ex.Message);
                    return;
                }

                Log("监听成功");

                receiveThread = new Thread(ReceiveMessage);

                receiveThread.Start();

                rdBtnUDP.Enabled = false;
                rdBtnTCP.Enabled = false;

                btnListen.Text = "解除";
                bListen = true;
            }
            else
            {
                if (remoteIpEndPoint != null)
                {
                    //发送断开连接信息
                    MsgEntity msg = new MsgEntity(Constant.MSG_TYPE_CMD, Constant.SESSION_CODE_EXIT);
                    byte[] sendbytes = FileUtils.GetSendBytes(msg);
                    SendByteData(sendbytes);
                }

                //断开连接
                if (receiveThread != null)
                {
                    receiveThread.Abort();
                }
                if (receiveUpdClient != null)
                {
                    receiveUpdClient.Close();
                }
                remoteIpEndPoint = null;

                rdBtnUDP.Enabled = true;
                rdBtnTCP.Enabled = true;

                Log("已解除监听");

                btnListen.Text = "监听";
                bListen = false;
            }
        }

        #region 接收消息处理

        // 接收消息方法
        private void ReceiveMessage()
        {
            if (rdBtnTCP.Checked)
            {
                receiveUpdClient.AcceptNew();
            }

            while (true)
            {
                try
                {
                    // 关闭receiveUdpClient时此时会产生异常
                    byte[] receiveBytes = receiveUpdClient.Receive(ref remoteIpEndPoint);
                    if (receiveBytes.Length <= 0)
                        continue;

                    byte[] sendbytes;
                    byte[] extendData = new byte[0];
                    MsgEntity msg = (MsgEntity)FileUtils.GetMsgEntity(receiveBytes, ref extendData);

                    if (msg.msgType == Constant.MSG_TYPE_TXT)//收到文本消息
                    {
                        string message = msg.msgBody;
                        // 显示消息内容
                        Log(string.Format("{0}[{1}]", remoteIpEndPoint, message));

                        //响应收到
                        MsgEntity msg2 = new MsgEntity();
                        msg2.msgType = Constant.MSG_TYPE_CMD;
                        msg2.msgBody = Constant.CMD_RECEIVE_RSP;
                        msg2.args = new Dictionary<string, object>();
                        msg2.args.Add("rspText", "收到");
                        sendbytes = FileUtils.GetSendBytes(msg2);
                        SendByteData(sendbytes);
                    }
                    else if (msg.msgType == Constant.MSG_TYPE_CMD)//收到命令消息
                    {
                        if (msg.msgBody == Constant.SESSION_CODE_EXIT)//收到退出指令
                        {
                            Log("客户端主动断开：" + remoteIpEndPoint);
                            receiveUpdClient.AcceptNew();

                            InvokeCtrl.UpdateCtrlEnabled(btnSendFile, false);
                            InvokeCtrl.UpdateCtrlEnabled(btnSendMsg, false);
                            InvokeCtrl.UpdateCtrlEnabled(btnPause, false);
                            InvokeCtrl.UpdateCtrlEnabled(rdBtnUDP, true);
                            InvokeCtrl.UpdateCtrlEnabled(rdBtnTCP, true);

                            continue;
                        }
                        else if (msg.msgBody == Constant.SESSION_CODE_LOGIN)//收到登录指令
                        {
                            Log("客户端连接：" + remoteIpEndPoint);

                            InvokeCtrl.UpdateCtrlEnabled(btnSendFile, true);
                            InvokeCtrl.UpdateCtrlEnabled(btnSendMsg, true);
                            InvokeCtrl.UpdateCtrlEnabled(btnPause, false);
                            InvokeCtrl.UpdateCtrlEnabled(rdBtnUDP, false);
                            InvokeCtrl.UpdateCtrlEnabled(rdBtnTCP, false);

                            continue;
                        }
                        else if (msg.msgBody == Constant.CMD_RECEIVE_RSP) //响应已接收
                        {
                            if (chkShowLog.Checked)
                            {
                                // 显示响应内容
                                string rspText = (string)msg.args["rspText"];
                                Log(string.Format("{0}[{1}]", remoteIpEndPoint, rspText));
                            }
                        }
                        else if (msg.msgBody == Constant.CMD_FILE_BREAKPOINT_REQ)//收到检索文件断点请求
                        {
                            MsgEntity msg2 = new MsgEntity();
                            msg2.msgType = Constant.MSG_TYPE_CMD;
                            msg2.msgBody = Constant.CMD_FILE_BREAKPOINT_RSP;//响应请求
                            msg2.args = new Dictionary<string, object>();
                            msg2.args.Add("fileexits", false);

                            string hashcode = (string)msg.args["hashcode"];
                            FileInfoEntity entity = FileUtils.getFileInfo(sendFileInfo, hashcode);
                            if (entity != null)
                            {
                                string destFile = entity.filepath + entity.filename;
                                if (File.Exists(destFile))
                                {
                                    long filesize = FileUtils.GetFileSize(destFile);
                                    msg2.args["fileexits"] = true;
                                    msg2.args.Add("breakpoint", filesize);
                                }
                            }
                            sendbytes = FileUtils.GetSendBytes(msg2);
                            SendByteData(sendbytes);
                            continue;
                        }
                        else if (msg.msgBody == Constant.CMD_FILE_TRANSFER_REQ)//收到文件传输请求
                        {
                            if (chkShowLog.Checked)
                            {
                                Log("收到文件流len=" + (receiveBytes.Length + 4) + "来自：" + remoteIpEndPoint);
                            }
                            //响应收到
                            MsgEntity msg2 = new MsgEntity();
                            msg2.msgType = Constant.MSG_TYPE_CMD;
                            msg2.msgBody = Constant.CMD_RECEIVE_RSP;
                            msg2.args = new Dictionary<string, object>();
                            msg2.args.Add("rspText", "收到");
                            sendbytes = FileUtils.GetSendBytes(msg2);
                            SendByteData(sendbytes);

                            string filename = (string)msg.args["filename"];
                            string hashcode = (string)msg.args["hashcode"];
                            string sendstate = (string)msg.args["sendstate"];
                            long total = (long)msg.args["total"];
                            bool reseed = (bool)msg.args["reseed"];
                            long offset = (long)msg.args["offset"];

                            string folder = txtFolder.Text.Trim();
                            if (folder == "")
                            {
                                folder = AppDomain.CurrentDomain.BaseDirectory;
                            }
                            folder = folder.TrimEnd('\\') + "\\";

                            FileInfoEntity fileInfoEntity = FileUtils.getFileInfo(sendFileInfo, hashcode);
                            if (fileInfoEntity == null)
                            {
                                fileInfoEntity = new FileInfoEntity(hashcode, folder, filename);
                                sendFileInfo.fileList.Add(fileInfoEntity);
                            }

                            //非续传，且非发送中状态，更新文件路径
                            if (!reseed && fileInfoEntity.filestate != Constant.STATE_FILE_SENDING)
                            {
                                fileInfoEntity.filepath = folder;
                                fileInfoEntity.filename = filename;
                            }

                            string destFile = fileInfoEntity.filepath + fileInfoEntity.filename;

                            if (File.Exists(destFile))
                            {
                                //文件存在，非续传，且非发送中状态，则覆盖
                                if (!reseed && fileInfoEntity.filestate != Constant.STATE_FILE_SENDING)
                                {
                                    File.Delete(destFile);
                                }
                            }
                            else
                            {
                                if (reseed)//文件不存在，续传则取消
                                {
                                    SendMessage("要续传的文件已丢失");
                                    continue;
                                }
                            }

                            try
                            {
                                if (sendstate == Constant.STATE_SEND_ON)
                                {
                                    FileUtils.WriteToFile(destFile, extendData, offset);
                                    fileInfoEntity.offset = offset + extendData.Length;
                                    fileInfoEntity.filestate = Constant.STATE_FILE_SENDING;

                                    MsgEntity msg3 = new MsgEntity();
                                    msg3.msgType = Constant.MSG_TYPE_CMD;
                                    msg3.msgBody = Constant.CMD_FILE_CONTINUE_RSP;//响应传输继续
                                    msg3.args = new Dictionary<string, object>();
                                    msg3.args.Add("breakpoint", fileInfoEntity.offset);
                                    sendbytes = FileUtils.GetSendBytes(msg3);
                                    SendByteData(sendbytes);
                                }
                                else if (sendstate == Constant.STATE_SEND_END)
                                {
                                    FileUtils.WriteToFile(destFile, extendData, offset);
                                    fileInfoEntity.offset = offset + extendData.Length;

                                    //进行文件校验
                                    string filecode = FileUtils.GetHashCode(destFile);
                                    if (filecode.Equals(hashcode))
                                    {
                                        fileInfoEntity.filestate = Constant.STATE_FILE_FINISH;

                                        //发送接收完成信息
                                        msg = new MsgEntity(Constant.MSG_TYPE_CMD, Constant.CMD_FILE_FINISH_RSP);
                                        sendbytes = FileUtils.GetSendBytes(msg);
                                        SendByteData(sendbytes);

                                        Log("文件校验成功！");
                                        Log("已保存路径为：" + destFile);
                                    }
                                    else
                                    {
                                        fileInfoEntity.filestate = Constant.STATE_FILE_FAILD;

                                        //发送接收失败信息
                                        msg = new MsgEntity(Constant.MSG_TYPE_CMD, Constant.CMD_FILE_FAILD_RSP);
                                        sendbytes = FileUtils.GetSendBytes(msg);
                                        SendByteData(sendbytes);

                                        Log("文件校验失败！");
                                        Log("已保存路径为：" + destFile);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                fileInfoEntity.filestate = Constant.STATE_FILE_FAILD;

                                //发送接收失败信息
                                msg = new MsgEntity(Constant.MSG_TYPE_CMD, Constant.CMD_FILE_FAILD_RSP);
                                sendbytes = FileUtils.GetSendBytes(msg);
                                SendByteData(sendbytes);

                                Log("接收消息线程出错：" + e.Message);
                            }

                            //发送文件过程中，需要及时保存文件断点信息，以免程序异常退出
                            string path = AppDomain.CurrentDomain.BaseDirectory;
                            string strSendInfo = path + Constant.FILENAME_SENDINFO;
                            FileUtils.SerializeFile(strSendInfo, sendFileInfo);

                            int finish = (int)Math.Ceiling(100.0 * fileInfoEntity.offset / total);
                            InvokeCtrl.UpdatePrgBarValue(prgBar1, finish, Color.OrangeRed, string.Format("接收完成{0} %", finish));

                            InvokeCtrl.UpdateCtrlText(lbFinish, FileUtils.getFinished(fileInfoEntity.offset, total));
                        }
                        else if (msg.msgBody == Constant.CMD_FILE_CONTINUE_RSP)//收到继续传输的响应
                        {
                            long breakpoint = (long)msg.args["breakpoint"];
                            sendThread = new Thread(SendFile);
                            sendThread.Start(breakpoint);
                        }
                        else if (msg.msgBody == Constant.CMD_FILE_FINISH_RSP)//收到文件传输成功响应
                        {
                            InvokeCtrl.UpdateCtrlText(btnSendFile, "发送文件");
                            InvokeCtrl.UpdateCtrlEnabled(btnSendFile, true);
                            InvokeCtrl.UpdateCtrlEnabled(btnPause, false);
                            finished = true;
                            timerSendState.Stop();

                            Log(string.Format("{0}[{1}]", remoteIpEndPoint, "文件接收完毕"));
                        }
                        else if (msg.msgBody == Constant.CMD_FILE_FAILD_RSP)//收到文件传输失败响应
                        {
                            InvokeCtrl.UpdateCtrlEnabled(btnSendFile, true);
                            InvokeCtrl.UpdateCtrlEnabled(btnPause, false);
                            finished = false;
                            timerSendState.Stop();
                            Log(string.Format("{0}[{1}]", remoteIpEndPoint, "文件接收失败"));
                        }
                        else if (msg.msgBody == Constant.CMD_FILE_BREAKPOINT_RSP)//收到检索文件断点响应
                        {
                            bool fileexits = (bool)msg.args["fileexits"];
                            if (fileexits)
                            {
                                long breakpoint = (long)msg.args["breakpoint"];
                                sendThread = new Thread(SendFile);
                                sendThread.Start(breakpoint);

                                timerSendState.Start();
                                InvokeCtrl.UpdateCtrlEnabled(btnPause, true);

                                milliseconds = 0;
                                finished = false;
                                countBytes = completeBytes = breakpoint;
                            }
                            else
                            {
                                DialogResult dlgResult = MessageBox.Show("对方文件不存在，是否重新上传？", "提示", MessageBoxButtons.YesNo);
                                if (DialogResult.Yes == dlgResult)
                                {
                                    sendThread = new Thread(SendFile);
                                    sendThread.Start(0);
                                    InvokeCtrl.UpdateCtrlText(btnSendFile, "发送文件");

                                    timerSendState.Start();
                                    InvokeCtrl.UpdateCtrlEnabled(btnPause, true);

                                    milliseconds = 0;
                                    finished = false;
                                    countBytes = completeBytes = 0;
                                }
                                else
                                {
                                    InvokeCtrl.UpdateCtrlEnabled(btnSendFile, true);
                                    InvokeCtrl.UpdateCtrlEnabled(btnPause, false);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log("接收消息线程出错：" + e.Message);

                    InvokeCtrl.UpdateCtrlEnabled(btnSendFile, true);
                    InvokeCtrl.UpdateCtrlEnabled(btnPause, false);
                    timerSendState.Stop();

                    break;
                }
            }
        }

        #endregion

        #region 发送消息处理

        //按钮事件：发送消息
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            if (txtFile.Text == string.Empty)
            {
                MessageBox.Show("发送内容不能为空", "提示");
                return;
            }

            sendThread = new Thread(SendMessage);

            sendThread.Start(txtFile.Text);
        }

        /// <summary>
        /// 发送字节数据
        /// </summary>
        /// <param name="sendbytes"></param>
        private void SendByteData(byte[] sendbytes)
        {
            if (rdBtnTCP.Checked)
            {
                receiveUpdClient.Send(sendbytes, sendbytes.Length);
            }
            else
            {
                receiveUpdClient.Send(sendbytes, sendbytes.Length, remoteIpEndPoint);
            }
        }

        //发送消息
        private void SendMessage(object param)
        {
            string message = (string)param;
            byte[] sendbytes = FileUtils.GetSendBytes(message);

            SendByteData(sendbytes);
            //Log("消息发送成功！");
        }

        //按钮事件：发送文件
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (txtFile.Text == string.Empty)
            {
                MessageBox.Show("请选择要传输的文件", "提示");
                return;
            }
            if (!File.Exists(txtFile.Text))
            {
                MessageBox.Show("要传输的文件不存在", "提示");
                return;
            }

            btnSendFile.Enabled = false;
            btnPause.Enabled = true;


            if (btnSendFile.Text == "续传文件")
            {
                string hashcode = FileUtils.GetHashCode(txtFile.Text);

                //发送检索文件断点请求
                MsgEntity msg = new MsgEntity(Constant.MSG_TYPE_CMD, Constant.CMD_FILE_BREAKPOINT_REQ);
                msg.args = new Dictionary<string, object>();
                msg.args.Add("hashcode", hashcode);
                byte[] sendbytes = FileUtils.GetSendBytes(msg);
                SendByteData(sendbytes);
            }
            else
            {
                sendThread = new Thread(SendFile);
                sendThread.Start(0);

                timerSendState.Start();
                InvokeCtrl.UpdateCtrlEnabled(btnPause, true);

                milliseconds = 0;
                finished = false;
                countBytes = completeBytes = 0;
            }
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="param">续传断点</param>
        private void SendFile(object param)
        {
            long breakpoint = Convert.ToInt64(param);
            string srcFile = InvokeCtrl.GetCtrlText(txtFile);
            string hashcode = FileUtils.GetHashCode(srcFile);
            string filename = FileUtils.GetFileName(srcFile);
            totalBytes = FileUtils.GetFileSize(srcFile);

            if (totalBytes - breakpoint <= 0)
            {
                return;
            }
            //间歇时间
            Thread.Sleep(interval);

            MsgEntity msg = new MsgEntity();
            msg.msgType = Constant.MSG_TYPE_CMD;
            msg.msgBody = Constant.CMD_FILE_TRANSFER_REQ;
            msg.args = new Dictionary<string, object>();
            msg.args.Add("filename", filename); //文件名
            msg.args.Add("hashcode", hashcode); //文件hash值
            msg.args.Add("total", totalBytes);  //文件大小
            msg.args.Add("reseed", (breakpoint > 0));//是否为续传
            msg.args.Add("sendstate", "");      //传输状态（发送中/发送结束）
            msg.args.Add("offset", 0);          //传输流的偏移量

            int size = buffersize * 1024;

            if (breakpoint + size > totalBytes)
            {
                msg.args["sendstate"] = Constant.STATE_SEND_END;
                timerSendState.Stop();
            }
            else
            {
                msg.args["sendstate"] = Constant.STATE_SEND_ON;
            }

            msg.args["offset"] = breakpoint;

            byte[] extendData = FileUtils.ReadFromFile(srcFile, size, breakpoint);
            byte[] sendbytes = FileUtils.GetSendBytes(msg, extendData);

            SendByteData(sendbytes);
            if (chkShowLog.Checked)
            {
                Log("发送文件流len=" + sendbytes.Length);
            }

            completeBytes += extendData.Length;

            int finish = (int)Math.Ceiling(100.0 * completeBytes / totalBytes);
            InvokeCtrl.UpdatePrgBarValue(prgBar1, finish, Color.FromKnownColor(KnownColor.Highlight), string.Format("发送完成{0} %", finish));

            InvokeCtrl.UpdateCtrlText(lbFinish, FileUtils.getFinished(completeBytes, totalBytes));
        }

        #endregion

        delegate void ShowLog(string msg);
        /// <summary>
        /// 写状态日志
        /// </summary>
        /// <param name="msg">消息</param>
        private void Log(string msg)
        {
            try
            {
                if (txtStatus.InvokeRequired)
                {
                    txtStatus.Invoke(new ShowLog(Log), new string[] { msg });
                }
                else
                {
                    txtStatus.AppendText(msg + "\r\n");
                }
            }
            catch { }
        }

        //窗体关闭
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bListen && remoteIpEndPoint != null)
            {
                try
                {
                    //发送断开连接信息
                    MsgEntity msg = new MsgEntity(Constant.MSG_TYPE_CMD, Constant.SESSION_CODE_EXIT);
                    byte[] sendbytes = FileUtils.GetSendBytes(msg);
                    SendByteData(sendbytes);
                }
                catch { }
            }

            if (receiveThread != null)
            {
                receiveThread.Abort();
            }

            if (receiveUpdClient != null)
            {
                receiveUpdClient.Close();
            }

            //写入配置文件
            WriteConfig();
        }

        //打开文件
        private void btnFileOpen_Click(object sender, EventArgs e)
        {

            DialogResult dlgResult = openFileDlg.ShowDialog();
            if (DialogResult.OK == dlgResult)
            {
                string srcFile = openFileDlg.FileName;
                txtFile.Text = srcFile;
                btnSendFile.Text = "发送文件";
            }
        }

        //打开文件目录
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string path = txtFolder.Text.Trim();
            if (path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }

            openFolderDlg.SelectedPath = path;
            DialogResult dlgResult = openFolderDlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                txtFolder.Text = openFolderDlg.SelectedPath.TrimEnd('\\') + "\\";
            }
        }

        #region 定时器

        //自动保存定时器
        private void timerSaveCfg_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //写入配置文件
            WriteConfig();
        }

        //传输状态定时器
        private void timerSendState_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //传输速度更新
            if (counter == 0)
            {
                double speed = 1.0 * (completeBytes - countBytes) / (1024 * 5);
                tssLabel.Text = String.Format("传输速度：{0:0.0} kps", speed);
                countBytes = completeBytes;
            }

            //传输用时更新：100 * 10 * 5 毫秒 = 5秒 后，重新统计发送字节数
            milliseconds += 100;
            counter = (counter + 1) % (10 * 5);
            lbSeconds.Text = FileUtils.getUseTime(milliseconds);
        }

        #endregion

        #region 读写配置文件操作
        
        /// <summary>
        /// 写入配置文件
        /// </summary>
        private void WriteConfig()
        {
            //写入配置文件
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string strIniFile = path + Constant.FILENAME_CONFIG;
            IniFiles ini = new IniFiles(strIniFile);

            ini.WriteString(iniSession, "ip", txtIP.Text);
            ini.WriteString(iniSession, "port", txtPort.Text);
            ini.WriteString(iniSession, "folder", txtFolder.Text);
            ini.WriteString(iniSession, "file", txtFile.Text);
            ini.WriteBool(iniSession, "udp", rdBtnUDP.Checked);
            ini.WriteBool(iniSession, "tcp", rdBtnTCP.Checked);

            ini.WriteBool(iniSession, "finished", finished);
            ini.WriteLong(iniSession, "completeBytes", completeBytes);
            ini.WriteLong(iniSession, "totalBytes", totalBytes);
            ini.WriteLong(iniSession, "milliseconds", milliseconds);
            ini.WriteInteger(iniSession, "buffersize", buffersize);
            ini.WriteInteger(iniSession, "interval", interval);

            ini.WriteBool(iniSession, "showlog", chkShowLog.Checked);
            ini.WriteBool(iniSession, "nothing", rdBtnNothing.Checked);
            ini.WriteBool(iniSession, "exitapp", rdBtnExitApp.Checked);
            ini.WriteBool(iniSession, "shutdown", rdBtnShutdown.Checked);

            //写入上传文件信息
            string strSendInfo = path + Constant.FILENAME_SENDINFO;
            FileUtils.SerializeFile(strSendInfo, sendFileInfo);
        }
        

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void ReadConfig()
        {
            //读取配置文件
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string strIniFile = path + Constant.FILENAME_CONFIG;
            IniFiles ini = new IniFiles(strIniFile);

            txtIP.Text = ini.ReadString(iniSession, "ip", "");
            txtPort.Text = ini.ReadString(iniSession, "port", "");
            txtFolder.Text = ini.ReadString(iniSession, "folder", "");
            txtFile.Text = ini.ReadString(iniSession, "file", "");
            rdBtnUDP.Checked = ini.ReadBool(iniSession, "udp", true);
            rdBtnTCP.Checked = ini.ReadBool(iniSession, "tcp", false);

            finished = ini.ReadBool(iniSession, "finished", false);
            completeBytes = ini.ReadLong(iniSession, "completeBytes", 0);
            totalBytes = ini.ReadLong(iniSession, "totalBytes", 0);
            milliseconds = ini.ReadLong(iniSession, "milliseconds", 0);
            buffersize = ini.ReadInteger(iniSession, "buffersize", Constant.EXDATA_SIZE);
            interval = ini.ReadInteger(iniSession, "interval", 0);

            chkShowLog.Checked = ini.ReadBool(iniSession, "showlog", true);
            rdBtnNothing.Checked = ini.ReadBool(iniSession, "nothing", true);
            rdBtnExitApp.Checked = ini.ReadBool(iniSession, "exitapp", false);
            rdBtnShutdown.Checked = ini.ReadBool(iniSession, "shutdown", false);

            //读取上传文件信息
            string strSendInfo = path + Constant.FILENAME_SENDINFO;
            if (File.Exists(strSendInfo))
            {
                sendFileInfo = (SendFileInfo)FileUtils.DeserializeFile(strSendInfo, typeof(SendFileInfo));
            }
            else
            {
                sendFileInfo = new SendFileInfo();
            }
        }

        #endregion

        //暂停按钮
        private void btnPause_Click(object sender, EventArgs e)
        {
            timerSendState.Stop();

            btnSendFile.Text = "续传文件";
            btnSendFile.Enabled = true;
            btnPause.Enabled = false;

            if (sendThread != null)
            {
                sendThread.Abort();
            }
        }

        //清除状态信息
        private void picBoxClean_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "";
        }

        //拖动间歇时间条
        private void trackBarInterval_Scroll(object sender, EventArgs e)
        {
            interval = trackBarInterval.Value;
            lbMilliSecond.Text = interval + " ms";
        }

        #region 支持系统托盘功能

        //菜单项：显示、隐藏界面
        private void menuItemShow_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon.Visible = true;
                this.Hide();
                menuItemShow.Text = "显示界面(&V)";
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                menuItemShow.Text = "隐藏界面(&H)";
            }
        }

        //菜单项：关于软件
        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            aboutForm.Visible = true;
            aboutForm.Activate();
        }

        //菜单项：退出程序
        private void menuItemExit_Click(object sender, EventArgs e)
        {
            FormClosedEventArgs args = new FormClosedEventArgs(CloseReason.UserClosing);
            this.MainForm_FormClosed(sender, args);

            this.notifyIcon.Dispose();
            this.Dispose();
            System.Environment.Exit(0);
        }

        //双击通知栏图标
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon.Visible = true;
                this.Hide();
                menuItemShow.Text = "显示界面(&V)";
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                menuItemShow.Text = "隐藏界面(&H)";
            }
        }

        //关闭窗口前
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true;
                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon.Visible = true;
                this.Hide();
                menuItemShow.Text = "显示界面(&V)";
                this.notifyIcon.ShowBalloonTip(1000, "文件发送-服务端", "程序已最小化到这里，双击可以打开", ToolTipIcon.Info);
                return;
            }
        }

        #endregion

        #region 支持拖动文件到窗口

        //拖动对象到窗口（未松开）
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //拖动对象到窗口（并松开）
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                String[] files = e.Data.GetData(DataFormats.FileDrop, false) as String[];
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        txtFile.Text = file;
                    }
                    else if (Directory.Exists(file))
                    {
                        txtFolder.Text = file.TrimEnd('\\') + "\\";
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtFile.BackColor = Color.FromKnownColor(KnownColor.Window);
            txtFolder.BackColor = Color.FromKnownColor(KnownColor.Window);
        }

        //拖入窗口边界
        private void MainForm_DragOver(object sender, DragEventArgs e)
        {
            txtFile.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
            txtFolder.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
        }

        //拖出窗口边界
        private void MainForm_DragLeave(object sender, EventArgs e)
        {
            txtFile.BackColor = Color.FromKnownColor(KnownColor.Window);
            txtFolder.BackColor = Color.FromKnownColor(KnownColor.Window);
        }

        #endregion
    }
}
