using CreateCompanyDivision;
using Microsoft.Win32;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SendBODToIMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bwIMS = new BackgroundWorker();
        BackgroundWorker bwUserInformation = new BackgroundWorker();

        private IONAPIFile credentials;

        IMS iMS = null;

        public MainWindow()
        {
            InitializeComponent();

            bwIMS.DoWork += bwIMS_DoWork;
            bwIMS.RunWorkerCompleted += bwIMS_RunWorkerCompleted;

        }

        private void setEnabled(bool aEnabled)
        {

            btnIONAPI.IsEnabled = aEnabled;
            btnSend.IsEnabled = aEnabled;

        }

        private void bwIMS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IMSResponse result = e.Result as IMSResponse;

            if(null != result)
            {
                if(false == string.IsNullOrEmpty(result.Result))
                {
                    MessageBox.Show("Done");
                }
                else
                {
                    MessageBox.Show("Error: " + result.StatusCode + " " + result.Error);
                }
            }
        }

        private void bwIMS_DoWork(object sender, DoWorkEventArgs e)
        {
            GetData gd = new GetData();

            string result = gd.PostIMS(credentials, iMS);

            IMSResponse response = new IMSResponse() { Error = gd.ErrorMessage, Result = result, StatusCode = gd.StatusCode };

            e.Result = response;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            iMS = new IMS();
            iMS.document = new doc();
            iMS.document.value = tbBODMessage.Text;
            iMS.documentName = tbBODName.Text;
            iMS.fromLogicalId = tbFromLogicalID.Text;
            iMS.toLogicalId = tbToLogicalID.Text;
            iMS.messageId = tbMessageID.Text;

            bwIMS.RunWorkerAsync();
        }

        private void btnIONAPI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofdDialog = new OpenFileDialog();
                ofdDialog.Filter = "ION API Files (*.ionapi)|*.ionapi";
                ofdDialog.CheckFileExists = true;

                if (true == ofdDialog.ShowDialog())
                {
                    string fileName = ofdDialog.FileName;

                    tbIONAPIPath.Text = fileName;

                    IONAPIFile ionAPI = IONAPIFile.LoadIONAPI(fileName);

                    if (null != ionAPI && true == string.IsNullOrEmpty(ionAPI.Error))
                    {
                        ionAPI.mainWindow = this;
                        tbTenant.Text = ionAPI.getTenant();
                        credentials = ionAPI;
                    }
                    else
                    {
                        if(null == ionAPI)
                        {
                            MessageBox.Show("Error failed to read file");
                        }
                        else
                        {
                            MessageBox.Show("Error: " + ionAPI.Error);
                        }
                        
                    }
                }
                ofdDialog = null;
            }
            catch (Exception ex)
            {

            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            credentials = null;
            Application.Current.Shutdown();
        }
    }
}
