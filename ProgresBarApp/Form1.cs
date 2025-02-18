using System;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProgressBarApp
{
    public partial class Form1 : Form
    {
        private Thread progressThread;
        private bool isRunning;
        private int progressValue;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                progressValue = 0;
                label1.Text = $"{progressValue}%";
                progressBar1.Value = progressValue;

                progressThread = new Thread(UpdateProgress)
                {
                    IsBackground = true
                };
                progressThread.Start();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isRunning = false;
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                progressThread = new Thread(UpdateProgress)
                {
                    IsBackground = true
                };
                progressThread.Start();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            isRunning = false;
            progressValue = 0;
            label1.Text = $"{progressValue}%";
            progressBar1.Value = progressValue;
        }

        private void UpdateProgress()
        {
            while (isRunning && progressValue < 100)
            {
                Thread.Sleep(100);
                progressValue++;
                Invoke(new Action(() =>
                {
                    progressBar1.Value = progressValue;
                    label1.Text = $"{progressValue}%";
                }));
            }
            isRunning = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isRunning = false;
            progressThread?.Join();
            base.OnFormClosing(e);
        }
    }
}