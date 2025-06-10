
namespace Joy.Test.Server
{
    public class ByteArray
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        public byte[] bytes { get; private set; }

        /// <summary>
        /// 容量(bytes数据长度)
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// 读取位置
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 写位置
        /// </summary>
        public int WriteIndex { get; set; }

        /// <summary>
        /// 剩余容量
        /// </summary>
        public int RemainCount => Capacity - WriteIndex;

        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length => WriteIndex - StartIndex;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ByteArray(int capacity)
        {
            bytes = new byte[capacity];
            Capacity = capacity;
            StartIndex = 0;
            WriteIndex = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ByteArray(byte[] newBytes)
        {
            bytes = newBytes;
            Capacity = newBytes.Length;
            StartIndex = 0;
            WriteIndex = newBytes.Length;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ByteArray(byte[] newBytes, int readIndex, int length)
        {
            bytes = newBytes;
            Capacity = newBytes.Length;
            StartIndex = readIndex;
            WriteIndex = readIndex + length;
        }

        /// <summary>
        /// 把数据移动到头部
        /// </summary>
        public void MoveToHead()
        {
            if (StartIndex != 0)
            {
                Array.Copy(bytes, StartIndex, bytes, 0, Length);
                WriteIndex = Length;
                StartIndex = 0;
            }
        }

        /// <summary>
        /// 扩容，双倍扩容
        /// </summary>
        public void ExpandCapacity()
        {
            var tmpBytes = bytes;
            Capacity *= 2;
            bytes = new byte[Capacity];
            Array.Copy(tmpBytes, StartIndex, bytes, 0, Length);
            WriteIndex = Length;
            StartIndex = 0;
        }
    }
}
