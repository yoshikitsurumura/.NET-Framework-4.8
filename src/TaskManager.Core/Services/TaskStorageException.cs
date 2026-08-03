using System;
using System.Runtime.Serialization;

namespace TaskManager.Core.Services
{
    /// <summary>
    /// タスクファイルの読み書きに失敗したときに投げられる。
    /// </summary>
    [Serializable]
    public class TaskStorageException : Exception
    {
        public TaskStorageException()
        {
        }

        public TaskStorageException(string message)
            : base(message)
        {
        }

        public TaskStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TaskStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
