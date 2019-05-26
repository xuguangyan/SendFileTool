using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SendFileCommon
{
    /// <summary>
    /// 文件操作工具类
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// 从文件中读取字节数组
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="len">读取长度（值为0表示读取偏移量后所有字节）</param>
        /// <param name="offset">起始偏移量</param>
        /// <returns></returns>
        public static byte[] ReadFromFile(string filepath, int len = 0, long offset = 0)
        {
            byte[] buffer = new byte[0];
            try
            {
                int readLen = 0;
                using (FileStream fout = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    if (len == 0)
                    {
                        //类型强转意义：一次读取len < 2^32=4G 强转将有效
                        len = (int)(fout.Length - offset);
                    }
                    buffer = new byte[len];

                    fout.Seek(offset, SeekOrigin.Begin);
                    readLen = fout.Read(buffer, 0, len);
                    fout.Close();
                }
                if (readLen <= 0)
                {
                    return new byte[0];
                }
                else if (readLen < len)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(buffer, 0, readLen);
                        buffer = ms.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return buffer;
        }


        /// <summary>
        /// 将字节数组写入到文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">起始偏移量</param>
        public static void WriteToFile(string filepath, byte[] buffer, long offset = 0)
        {
            if (buffer.Length <= 0)
                return;

            //若目录不存在，先创建
            CreatePath(filepath);

            try
            {
                using (FileStream fin = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fin.Seek(offset, SeekOrigin.Begin);
                    fin.Write(buffer, 0, buffer.Length);
                    fin.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="filepath">路径</param>
        public static void CreatePath(string filepath)
        {
            int pos = filepath.LastIndexOf('\\');
            if (pos > 0)
            {
                string dir = filepath.Substring(0, pos + 1);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static long GetFileSize(string filepath)
        {
            long filesize = 0;
            try
            {
                using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    filesize = file.Length;
                    file.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return filesize;
        }

        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static string GetFileName(string filepath)
        {
            string filename = "";

            int pos = filepath.Replace("/", "\\").LastIndexOf("\\") + 1;
            if (pos > 0)
            {
                filename = filepath.Substring(pos, filepath.Length - pos);
            }

            return filename;
        }

        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>返回字节数组</returns>
        public static byte[] GetSendBytes(string message)
        {
            MsgEntity msg = new MsgEntity(message);
            return GetSendBytes(msg);
        }

        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <param name="msg">消息对象</param>
        /// <returns>返回字节数组</returns>
        public static byte[] GetSendBytes(MsgEntity msg)
        {
            byte[] msgbytes = SerializeBytes(msg);

            byte[] databytes = BitConverter.GetBytes(msgbytes.Length);
            databytes = databytes.Concat(msgbytes).ToArray();

            byte[] sendbytes = BitConverter.GetBytes(databytes.Length + 4);
            sendbytes = sendbytes.Concat(databytes).ToArray();

            return sendbytes;
        }

        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <param name="msg">消息对象</param>
        /// <param name="extendData">扩展字节数据</param>
        /// <returns>返回字节数组</returns>
        public static byte[] GetSendBytes(MsgEntity msg,byte[] extendData)
        {
            /*
             * sendbytes = [LEN1][[LEN2][MSG][EXDATA]]
             * LEN1 = sendbytes总长........（占4字节）
             * LEN2 = MSG长度..............（占4字节）
             * MSG  = 序列化后的MsgEntity..（不定长<1024*3）
             * EXDATA = 扩展的extendData...（不定长）
             * */
            byte[] msgbytes = SerializeBytes(msg);

            byte[] databytes = BitConverter.GetBytes(msgbytes.Length);
            databytes = databytes.Concat(msgbytes).ToArray();
            databytes = databytes.Concat(extendData).ToArray();

            byte[] sendbytes = BitConverter.GetBytes(databytes.Length + 4);
            sendbytes = sendbytes.Concat(databytes).ToArray();

            return sendbytes;
        }

        /// <summary>
        /// 反序列化消息
        /// </summary>
        /// <param name="msgBytes">消息字节数组</param>
        /// <returns>返回消息对象</returns>
        public static MsgEntity GetMsgEntity(byte[] receiveBytes)
        {
            int msgLen = BitConverter.ToInt32(receiveBytes, 0);
            byte[] msgbytes = receiveBytes.Skip(4).Take(msgLen).ToArray();
            MsgEntity msg = (MsgEntity)DeserializeBytes(msgbytes, typeof(MsgEntity));

            return msg;
        }

        /// <summary>
        /// 反序列化消息
        /// </summary>
        /// <param name="msgBytes">消息字节数组</param>
        /// <param name="extendData">扩展字节数组</param>
        /// <returns>返回消息对象</returns>
        public static MsgEntity GetMsgEntity(byte[] receiveBytes, ref byte[] extendData)
        {
            int msgLen = BitConverter.ToInt32(receiveBytes, 0);
            byte[] msgbytes = receiveBytes.Skip(4).Take(msgLen).ToArray();
            extendData = receiveBytes.Skip(msgLen + 4).ToArray();
            MsgEntity msg = (MsgEntity)DeserializeBytes(msgbytes, typeof(MsgEntity));

            return msg;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="param">要序列化的对象</param>
        /// <returns>返回流</returns>
        public static Stream SerializeStream(object param)
        {
            DataContractSerializer serializer = new DataContractSerializer(param.GetType());
            MemoryStream stream = new MemoryStream();

            serializer.WriteObject(stream, param);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="param">要序列化的对象</param>
        /// <returns>返回字节数组</returns>
        public static byte[] SerializeBytes(object param)
        {
            MemoryStream stream = (MemoryStream)SerializeStream(param);

            long bufferSize = stream.Length;
            byte[] buffer = new byte[bufferSize];
            int readLen = stream.Read(buffer, 0, buffer.Length);

            if (readLen <= 0)
            { 
                buffer = new byte[0];
            }
            else if (readLen < bufferSize)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buffer, 0, readLen);
                    buffer = ms.ToArray();
                }
            }

            return buffer;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="filepath">要写入文件</param>
        /// <param name="param">要序列化的对象</param>
        public static void SerializeFile(string filepath, object param)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            using (FileStream stream = File.OpenWrite(filepath))
            {
                DataContractSerializer serializer = new DataContractSerializer(param.GetType());
                serializer.WriteObject(stream, param);
                stream.Close();
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <param name="paramType">反序列化为类型</param>
        /// <returns>返回对象</returns>
        public static object DeserializeStream(Stream stream, Type paramType)
        {
            stream.Seek(0, SeekOrigin.Begin);

            DataContractSerializer serializer = new DataContractSerializer(paramType);

            object obj = serializer.ReadObject(stream);

            stream.Close();

            return obj;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="buffer">输入字节数组</param>
        /// <param name="paramType">反序列化为类型</param>
        /// <returns>返回对象</returns>
        public static object DeserializeBytes(byte[] buffer, Type paramType)
        {
            Stream stream = new MemoryStream(buffer);

            return DeserializeStream(stream, paramType);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="filepath">要读取的文件</param>
        /// <param name="paramType">反序列化为类型</param>
        /// <returns>返回对象</returns>
        public static object DeserializeFile(string filepath, Type paramType)
        {
            FileStream stream = File.OpenRead(filepath);

            return DeserializeStream(stream, paramType);
        }

        /// <summary>
        /// 获取文件HashCode
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <returns></returns>
        public static string GetHashCode(FileStream objFile)
        {
            string strHashData = "";
            try
            {
                //从文件中取得Hash描述 
                byte[] HashData;
                System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                strHashData = Convert.ToBase64String(HashData);
                objFile.Close();
            }
            catch{}

            return strHashData;
        }

        /// <summary>
        /// 获取文件HashCode
        /// </summary>
        /// <param name="filepath">待签名的文件路径</param>
        /// <returns></returns>
        public static string GetHashCode(string filepath)
        {
            string strHashData = "";
            try
            {
                using (FileStream objFile = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    //从文件中取得Hash描述 
                    byte[] HashData;
                    System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                    HashData = MD5.ComputeHash(objFile);
                    objFile.Close();
                    strHashData = Convert.ToBase64String(HashData);
                    objFile.Close();
                }
            }
            catch { }

            return strHashData;
        }

        /// <summary>
        /// 校验文件是否相同（通过文件哈希值）
        /// </summary>
        /// <param name="srcFile">源文件</param>
        /// <param name="destFile">目标文件</param>
        /// <returns></returns>
        public static bool VerifyFile(string srcFile, string destFile)
        {
            bool blSameFile = false;
            string hashcode1 = GetHashCode(srcFile);
            string hashcode2 = GetHashCode(destFile);
            if (hashcode1.Equals(hashcode2))
            {
                blSameFile = true;
            }
            return blSameFile;
        }

        /// <summary>
        /// 获取耗费时间
        /// </summary>
        /// <param name="milliseconds">毫秒数</param>
        /// <returns></returns>
        public static string getUseTime(long milliseconds)
        {
            int hour = (int)Math.Floor(1.0 * milliseconds / 3600000);
            int minute = (int)Math.Floor(1.0 * (milliseconds - hour * 3600000) / 60000);
            int second = (int)Math.Floor(1.0 * (milliseconds - hour * 3600000 - minute * 60000) / 1000);
            int millisecond = (int)(milliseconds - hour * 3600000 - minute * 60000 - second * 1000);
            string strTime = string.Format("{0:D2}时 {1:D2}分 {2:D2}秒 {3:D1}", hour, minute, second, millisecond / 100);
            return strTime;
        }

        /// <summary>
        /// 获取完成比率
        /// </summary>
        /// <param name="completeBytes">已完成字节</param>
        /// <param name="totalBytes">总字节</param>
        /// <returns></returns>
        public static string getFinished(long completeBytes, long totalBytes)
        {
            string strFinished = String.Format("完成：{0}/{1}", getByteSize(completeBytes), getByteSize(totalBytes));
            return strFinished;
        }

        /// <summary>
        /// 获取字节大小表示
        /// </summary>
        /// <param name="totalBytes">总字节数</param>
        /// <returns></returns>
        public static string getByteSize(long totalBytes)
        {
            double size = totalBytes;
            string strSize = size + " b";

            if (size > 1024 * 1024 * 1024)
            {
                size = size / (1024 * 1024 * 1024);
                strSize = string.Format("{0:0.00} gb", size);
            }
            else if (size > 1024 * 1024)
            {
                size = size / (1024 * 1024);
                strSize = string.Format("{0:0.0} mb", size);
            }
            else if (size > 1024)
            {
                size = size / 1024;
                strSize = string.Format("{0:0} kb", size);
            }

            return strSize;
        }

        /// <summary>
        /// 移除文件信息实体
        /// </summary>
        /// <param name="sendFileInfo">发送文件集合</param>
        /// <param name="hashcode">哈希值</param>
        public static void removeFileInfo(SendFileInfo sendFileInfo, string hashcode)
        {
            for (int i = 0; i < sendFileInfo.fileList.Count; i++)
            {
                FileInfoEntity entity = sendFileInfo.fileList[i];
                if (hashcode.Equals(entity.hashcode))
                {
                    sendFileInfo.fileList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 更新文件信息实体
        /// </summary>
        /// <param name="sendFileInfo">发送文件集合</param>
        /// <param name="fileInfoEntity">新对象</param>
        public static void updateFileInfo(SendFileInfo sendFileInfo, FileInfoEntity fileInfoEntity)
        {
            for (int i = 0; i < sendFileInfo.fileList.Count; i++)
            {
                FileInfoEntity entity = sendFileInfo.fileList[i];
                if (fileInfoEntity.hashcode.Equals(entity.hashcode))
                {
                    sendFileInfo.fileList[i] = fileInfoEntity;
                    break;
                }
            }
        }

        /// <summary>
        /// 获取文件信息实体
        /// </summary>
        /// <param name="sendFileInfo">发送文件集合</param>
        /// <param name="hashcode">哈希值</param>
        /// <returns></returns>
        public static FileInfoEntity getFileInfo(SendFileInfo sendFileInfo, string hashcode)
        {
            for (int i = 0; i < sendFileInfo.fileList.Count; i++)
            {
                FileInfoEntity entity = sendFileInfo.fileList[i];
                if (hashcode.Equals(entity.hashcode))
                {
                    return entity;
                }
            }

            return null;
        }
    }
}
