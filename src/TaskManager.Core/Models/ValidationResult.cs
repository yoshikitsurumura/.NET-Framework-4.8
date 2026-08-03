using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskManager.Core.Models
{
    /// <summary>
    /// 入力検証の結果。エラーメッセージをまとめて保持する。
    /// </summary>
    public class ValidationResult
    {
        private readonly List<string> _errors = new List<string>();

        public bool IsValid
        {
            get { return _errors.Count == 0; }
        }

        public ReadOnlyCollection<string> Errors
        {
            get { return _errors.AsReadOnly(); }
        }

        public void AddError(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("メッセージが空です。", "message");
            }
            _errors.Add(message);
        }

        public string ToMessage()
        {
            return string.Join(Environment.NewLine, _errors.ToArray());
        }
    }

    /// <summary>
    /// <see cref="TodoItem"/> の入力検証。
    /// </summary>
    public static class TodoItemValidator
    {
        public static ValidationResult Validate(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(item.Title))
            {
                result.AddError("タイトルを入力してください。");
            }
            else if (item.Title.Length > TodoItem.MaxTitleLength)
            {
                result.AddError(string.Format(
                    "タイトルは {0} 文字以内で入力してください。(現在 {1} 文字)",
                    TodoItem.MaxTitleLength, item.Title.Length));
            }

            if (item.Notes != null && item.Notes.Length > TodoItem.MaxNotesLength)
            {
                result.AddError(string.Format(
                    "メモは {0} 文字以内で入力してください。", TodoItem.MaxNotesLength));
            }

            if (item.Category != null && item.Category.Length > TodoItem.MaxCategoryLength)
            {
                result.AddError(string.Format(
                    "カテゴリは {0} 文字以内で入力してください。", TodoItem.MaxCategoryLength));
            }

            if (!Enum.IsDefined(typeof(TaskPriority), item.Priority))
            {
                result.AddError("優先度の値が不正です。");
            }

            return result;
        }
    }
}
