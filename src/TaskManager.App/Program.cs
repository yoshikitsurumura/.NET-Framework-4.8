using System;
using System.Windows.Forms;
using TaskManager.App.Forms;

namespace TaskManager.App
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // 高 DPI 対応は app.manifest の dpiAwareness 設定で行っている。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += (sender, e) => ShowUnexpectedError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => ShowUnexpectedError(e.ExceptionObject as Exception);

            Application.Run(new MainForm());
        }

        private static void ShowUnexpectedError(Exception exception)
        {
            string message = exception == null
                ? "原因不明のエラーが発生しました。"
                : exception.Message;

            MessageBox.Show(
                "予期しないエラーが発生しました。" + Environment.NewLine + Environment.NewLine + message,
                "タスク管理",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
