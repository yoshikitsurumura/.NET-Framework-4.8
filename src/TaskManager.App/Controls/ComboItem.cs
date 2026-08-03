namespace TaskManager.App.Controls
{
    /// <summary>
    /// ComboBox に「表示名」と「値」を持たせるための小さな入れ物。
    /// </summary>
    internal sealed class ComboItem
    {
        public ComboItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; private set; }

        public object Value { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
