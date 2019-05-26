using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendFileCommon
{
    /// <summary>
    /// 文件发送相关信息
    /// </summary>
    [Serializable]
    public class SendFileInfo
    {
        public List<FileInfoEntity> fileList = new List<FileInfoEntity>();
    }
}
