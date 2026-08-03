using System;
using System.Collections.Generic;

namespace TaskManager.Tests
{
    /// <summary>
    /// 外部パッケージに依存しない簡易テストランナー。
    /// テスト名と本体を登録し、まとめて実行して結果を標準出力に書き出す。
    /// </summary>
    public class TestRunner
    {
        private readonly List<string> _failures = new List<string>();
        private int _passed;

        public int Failed
        {
            get { return _failures.Count; }
        }

        public int Passed
        {
            get { return _passed; }
        }

        public void Run(string name, Action test)
        {
            try
            {
                test();
                _passed++;
                Console.WriteLine("  PASS  " + name);
            }
            catch (Exception ex)
            {
                _failures.Add(name + ": " + ex.Message);
                Console.WriteLine("  FAIL  " + name);
                Console.WriteLine("        " + ex.Message.Replace(Environment.NewLine, " / "));
            }
        }

        public void PrintSummary()
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(string.Format("成功 {0} 件 / 失敗 {1} 件", _passed, _failures.Count));

            if (_failures.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("失敗したテスト:");
                foreach (string failure in _failures)
                {
                    Console.WriteLine("  - " + failure);
                }
            }
        }
    }

    /// <summary>テスト失敗を表す例外。</summary>
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }

    /// <summary>最低限のアサーション。</summary>
    public static class Assert
    {
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void IsFalse(bool condition, string message)
        {
            IsTrue(!condition, message);
        }

        public static void AreEqual<T>(T expected, T actual, string message)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                throw new AssertionException(string.Format(
                    "{0} (期待値: {1} / 実際: {2})", message, expected, actual));
            }
        }

        public static void IsNull(object value, string message)
        {
            IsTrue(value == null, message);
        }

        public static void IsNotNull(object value, string message)
        {
            IsTrue(value != null, message);
        }

        public static void Throws<TException>(Action action, string message) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new AssertionException(string.Format(
                    "{0} ({1} を期待しましたが {2} が発生しました)", message, typeof(TException).Name, ex.GetType().Name));
            }

            throw new AssertionException(string.Format(
                "{0} ({1} が発生しませんでした)", message, typeof(TException).Name));
        }
    }
}
