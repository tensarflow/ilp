using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace ILP
{
    /// <summary>
    /// ToDo!!
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// ToDo!!
        /// </summary>
        public static TextBox textBox;
        /// <summary>
        /// ToDo!!
        /// </summary>
        public static GUIForm form1;

        /// <summary>
        /// ToDo!!
        /// </summary>
        public static void updateLogFile(string logMessage, bool isEvent = true)
        {
            string Text;
            if (!isEvent)
                Text = logMessage;
            else
                Text = "\r\nLog Entry : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + " | " + DateTime.Now.ToLongDateString() + " -------------------- " + logMessage;

            string Path = "C:/Users/teozk/OneDrive/Dokumente/Log.txt";

            string content = File.ReadAllText(Path);
            content = Text + "\n" + content;
            File.WriteAllText(Path, content);
            readLogFile(" ");
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        delegate void SetTextCallback(string text);

        /// <summary>
        /// ToDo!!
        /// </summary>
        private static void readLogFile(string a)
        {
            try
            {
                FileStream fileStream = new FileStream("C:/Users/teozk/OneDrive/Dokumente/Log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                StreamReader streamReader = new StreamReader(fileStream);

                if (textBox.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(readLogFile);
                    textBox.Invoke(d, new object[] { textBox.Text = streamReader.ReadToEnd() });
                    textBox.Update();
                }
                else
                {
                    textBox.Text = streamReader.ReadToEnd();
                    textBox.Update();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
