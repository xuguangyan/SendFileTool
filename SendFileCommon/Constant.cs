using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendFileCommon
{
    /// <summary>
    /// 系统常量表
    /// </summary>
    public class Constant
    {
        #region 消息类型

        /// <summary>
        /// 命令消息
        /// </summary>
        public static int MSG_TYPE_CMD = 1;
        /// <summary>
        /// 文本消息
        /// </summary>
        public static int MSG_TYPE_TXT = 2;

        #endregion

        #region 会话指令

        /// <summary>
        /// 退出会话
        /// </summary>
        public static string SESSION_CODE_EXIT = "session_exit";

        /// <summary>
        /// 登录会话
        /// </summary>
        public static string SESSION_CODE_LOGIN = "session_login";

        #endregion

        #region CMD指令

        /// <summary>
        /// 响应已接收
        /// </summary>
        public static string CMD_RECEIVE_RSP = "cmd_receive_rsp";
        /// <summary>
        /// 请求检索文件断点
        /// </summary>
        public static string CMD_FILE_BREAKPOINT_REQ = "cmd_file_breakpoint_req";
        /// 响应检索文件断点
        /// </summary>
        public static string CMD_FILE_BREAKPOINT_RSP = "cmd_file_breakpoint_rsp";
        /// <summary>
        /// 请求传输文件
        /// </summary>
        public static string CMD_FILE_TRANSFER_REQ = "cmd_file_transfer_req";
        /// <summary>
        /// 响应传输完成
        /// </summary>
        public static string CMD_FILE_FINISH_RSP = "cmd_file_finish_rsp";
        /// 响应传输失败
        /// </summary>
        public static string CMD_FILE_FAILD_RSP = "cmd_file_faild_rsp";
        /// <summary>
        /// 响应传输继续
        /// </summary>
        public static string CMD_FILE_CONTINUE_RSP = "cmd_file_continue_rsp";

        /// <summary>
        /// 发送状态：发送中
        /// </summary>
        public static string STATE_SEND_ON = "state_send_on";
        /// <summary>
        /// 发送状态：发送结束
        /// </summary>
        public static string STATE_SEND_END = "state_send_end";

        /// <summary>
        /// 文件状态：未开始
        /// </summary>
        public static int STATE_FILE_NOBEGIN = 0;
        /// <summary>
        /// 文件状态：发送中
        /// </summary>
        public static int STATE_FILE_SENDING = 1;
        /// <summary>
        /// 文件状态：已完成
        /// </summary>
        public static int STATE_FILE_FINISH = 2;
        /// <summary>
        /// 文件状态：已失败
        /// </summary>
        public static int STATE_FILE_FAILD = 3;

        #endregion

        #region 其他常量

        /// <summary>
        /// 扩展数据的缓冲区大小（KB）
        /// IP数据报 =  [IP_HEADER=20][[UDP_HEADER=8][SEND_DATA]] &lt; 1024*64
        /// SEND_DATA = [LEN1=4][[LEN2=4][MSG&lt;1024*3][EXDATA]]
        /// </summary>
        public static int EXDATA_SIZE = 60; // EXDATA_SIZE < 1024 * (64 - 3) - 36;

        /// <summary>
        /// 配置文件命名
        /// </summary>
        public static string FILENAME_CONFIG = "Config.ini";

        /// <summary>
        /// 发送信息命名
        /// </summary>
        public static string FILENAME_SENDINFO = "SendInfo.xml";

        #endregion
    }
}
