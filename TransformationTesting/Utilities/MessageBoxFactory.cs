using System.Windows.Forms;

namespace TransformationTesting.Utilities
{
    public class MessageBoxFactory
    {
        /// <summary>
        /// Show a message box with <paramref name="caption"/>, <paramref name="message"/>, <paramref name="buttons"/> and <paramref name="icon"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }

        /// <summary>
        /// Show info message box with <paramref name="caption"/> and <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public DialogResult ShowInfoBox(string message, string caption)
        {
            return ShowMessageBox(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show question message box with <paramref name="caption"/> and <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public DialogResult ShowQuestionBox(string message, string caption)
        {
            return ShowMessageBox(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Show warning message box with <paramref name="caption"/> and <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public DialogResult ShowWarningBox(string message, string caption)
        {
            return ShowMessageBox(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Show error message box with <paramref name="caption"/> and <paramref name="message"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public DialogResult ShowErrorBox(string message, string caption)
        {
            return ShowMessageBox(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }
    }
}
