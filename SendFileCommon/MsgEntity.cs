using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendFileCommon
{
    /// <summary>
    /// 网络传输的消息体
    /// </summary>
    [Serializable]
    public class MsgEntity
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public int msgType;
        /// <summary>
        /// 消息主体
        /// </summary>
        public string msgBody;
        /// <summary>
        /// 参数列表
        /// </summary>
        public Dictionary<string, object> args;

        /// <summary>
        /// 构造消息对象
        /// </summary>
        public MsgEntity()
        {
        }

        /// <summary>
        /// 构造消息对象
        /// </summary>
        /// <param name="msgBody">消息主体</param>
        public MsgEntity(string msgBody)
        {
            this.msgType = Constant.MSG_TYPE_TXT;
            this.msgBody = msgBody;
        }

        /// <summary>
        /// 构造消息对象
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="msgBody">消息主体</param>
        public MsgEntity(int msgType, string msgBody)
        {
            this.msgType = msgType;
            this.msgBody = msgBody;
        }
    }

    
}
