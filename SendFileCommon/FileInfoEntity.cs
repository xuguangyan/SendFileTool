using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendFileCommon
{
    /// <summary>
    /// 文件信息类
    /// </summary>
    public class FileInfoEntity
    {
        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string hashcode;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string filepath;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string filename;
        /// <summary>
        /// 文件存储状态
        /// </summary>
        public int filestate;
        /// <summary>
        /// 当前写入偏移量
        /// </summary>
        public long offset;

        public FileInfoEntity()
        {
        }

        public FileInfoEntity(string hasecode, string filepath, string filename, int filestate = 0, long offset = 0)
        {
            this.hashcode = hasecode;
            this.filepath = filepath;
            this.filename = filename;
            this.filestate = filestate;
            this.offset = offset;
        }
    }
}
