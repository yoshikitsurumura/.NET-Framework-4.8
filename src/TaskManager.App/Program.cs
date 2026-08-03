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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 高 DPI 対応 (.NET Framework 4.8 で追加された API)。
            // 古い OS などで利用できない場合は既定の動作のまま続行する。
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
            }
            catch (Exception)
            {
                // 無視して既定の DPI モードで起動する。
            }

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
