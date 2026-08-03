using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services
{
    /// <summary>
    /// XML ファイルにタスクを保存するリポジトリ。
    /// 既定の保存先は %APPDATA%\TaskManager\tasks.xml。
    /// </summary>
    public class XmlTaskRepository : ITaskRepository
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(List<TodoItem>), new XmlRootAttribute("Tasks"));

        private readonly string _filePath;

        public XmlTaskRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("保存先のパスが空です。", "filePath");
            }
            _filePath = Path.GetFullPath(filePath);
        }

        /// <summary>既定の保存先(%APPDATA%\TaskManager\tasks.xml)を使うリポジトリを作る。</summary>
        public static XmlTaskRepository CreateDefault()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TaskManager");
            return new XmlTaskRepository(Path.Combine(dir, "tasks.xml"));
        }

        public string Location
        {
            get { return _filePath; }
        }

        public IList<TodoItem> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TodoItem>();
            }

            try
            {
                using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var items = (List<TodoItem>)Serializer.Deserialize(stream);
                    return items ?? new List<TodoItem>();
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is XmlException)
            {
                string backupPath = MoveAsideBrokenFile();
                throw new TaskStorageException(
                    string.Format(
                        "タスクファイルを読み込めませんでした。{0}{1}{0}壊れたファイルは次の場所に退避しました:{0}{2}",
                        Environment.NewLine, _filePath, backupPath),
                    ex);
            }
            catch (IOException ex)
            {
                throw new TaskStorageException("タスクファイルの読み込みに失敗しました: " + _filePath, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new TaskStorageException("タスクファイルを読み取る権限がありません: " + _filePath, ex);
            }
        }

        public void Save(IEnumerable<TodoItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            var list = items.ToList();

            try
            {
                string directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 書き込み中の異常終了で既存データを失わないよう、一時ファイルへ書いてから置き換える。
                string tempPath = _filePath + ".tmp";
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    Encoding = new UTF8Encoding(false)
                };

                using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    Serializer.Serialize(writer, list);
                }

                if (File.Exists(_filePath))
                {
                    File.Replace(tempPath, _filePath, null);
                }
                else
                {
                    File.Move(tempPath, _filePath);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
            {
                throw new TaskStorageException("タスクファイルの保存に失敗しました: " + _filePath, ex);
            }
        }

        private string MoveAsideBrokenFile()
        {
            string backupPath = _filePath + "." +
                DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + ".corrupt";
            try
            {
                File.Move(_filePath, backupPath);
                return backupPath;
            }
            catch (IOException)
            {
                return _filePath + " (退避に失敗しました)";
            }
            catch (UnauthorizedAccessException)
            {
                return _filePath + " (退避に失敗しました)";
            }
        }
    }
}
