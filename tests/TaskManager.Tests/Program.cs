using System;
using System.Text;

namespace TaskManager.Tests
{
    /// <summary>
    /// テストランナーの実行入口。すべて成功なら終了コード 0、失敗があれば 1 を返す。
    /// </summary>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch (Exception)
            {
                // コンソールが無い環境では無視する。
            }

            Console.WriteLine("TaskManager.Core テスト");
            Console.WriteLine(new string('-', 60));

            var runner = new TestRunner();
            CoreTests.Register(runner);
            runner.PrintSummary();

            return runner.Failed == 0 ? 0 : 1;
        }
    }
}
