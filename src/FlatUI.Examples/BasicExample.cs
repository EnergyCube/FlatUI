using System;
using System.Threading;
using System.Windows.Forms;

namespace FlatUI.Examples
{
    public partial class BasicExample : Form
    {
        private bool _progressOngoing;

        public BasicExample()
        {
            InitializeComponent();
        }

        private void FlatButton_Click(object sender, EventArgs e)
        {
            if (!_progressOngoing)
            {
                var sleepAmount = ProgressCheckBox.Checked ? 30 : 10;

                _progressOngoing = true;
                FlatProgressBar.Value = 0;

                new Thread(() =>
                {
                    while (FlatProgressBar.Value < FlatProgressBar.Maximum)
                    {
                        Thread.Sleep(sleepAmount);
                        Invoke((MethodInvoker) (() => FlatProgressBar.Increment(1)));
                    }

                    _progressOngoing = false;
                }).Start();
            }
        }

        private void SpawnAlertButton_Click(object sender, EventArgs e)
        {
            FlatAlertBox.Visible = false;
            var kind = GetAlertBoxKind();
            FlatAlertBox.Kind = kind;
            FlatAlertBox.Visible = true;
        }

        /// <summary>
        ///     Get the alert box type from the radio buttons.
        /// </summary>
        /// <returns>Alert box kind</returns>
        private FlatAlertBox.StyleKind GetAlertBoxKind()
        {
            if (SuccessRadioButton.Checked)
                return FlatAlertBox.StyleKind.Success;

            return ErrorRadioButton.Checked ?
                FlatAlertBox.StyleKind.Error :
                FlatAlertBox.StyleKind.Info;
        }
    }
}