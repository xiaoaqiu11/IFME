﻿using System;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;

namespace IFME
{
    public partial class frmSplashScreen : Form
    {
        private BackgroundWorker2 bgThread = new BackgroundWorker2();

        public frmSplashScreen()
        {
            frmSplashScreenStatus = this;
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Opacity = 0;

            bgThread.DoWork += BgThread_DoWork;
            bgThread.RunWorkerCompleted += BgThread_RunWorkerCompleted;
        }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {

        }

        private void frmSplashScreen_Shown(object sender, EventArgs e)
        {
            bgThread.RunWorkerAsync();
        }

        private void BgThread_DoWork(object sender, DoWorkEventArgs e)
        {
            lblLoadingUpdate(string.Empty);
            lblStatusUpdate(string.Empty);

            Thread.Sleep(500);

            frmFadeInOut(true);

            // Detect user machine
            // TODO: Detect user GPU
            lblLoadingUpdate("Initializing...");

            // Load everything
            new PluginsLoad();

            // Finished loading, clear status text
            lblLoadingUpdate(string.Empty);
            lblStatusUpdate(string.Empty);

            // Wait some CPU free
            Thread.Sleep(3000);

            frmFadeInOut(false);
        }
        private void BgThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void frmFadeInOut(bool fadeIn)
        {
            if (fadeIn)
            {
                while (Opacity < 1)
                {
                    BeginInvoke((Action)delegate ()
                    {
                        Opacity += 0.02;
                    });

                    Thread.Sleep(1);
                }
            }
            else
            {
                while (Opacity > 0)
                {
                    BeginInvoke((Action)delegate ()
                    {
                        Opacity -= 0.02;
                    });

                    Thread.Sleep(1);
                }
            }
        }

        private void lblLoadingUpdate(string value)
        {
            BeginInvoke((Action)delegate ()
            {
                lblLoading.Text = value;
            });
        }

        private void lblStatusUpdate(string value)
        {
            BeginInvoke((Action)delegate ()
            {
                lblStatus.Text = value;
            });
        }
    }
}
