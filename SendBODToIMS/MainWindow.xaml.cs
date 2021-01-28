using CreateCompanyDivision;
using Microsoft.Win32;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml.Linq;

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

        private string projectName = "project.json";
        private Project project = null;

        public MainWindow()
        {
            InitializeComponent();

            if(File.Exists(projectName))
            {
                project = new Project();
                string error = null;
                if(null != (error = project.Load(projectName)))
                {
                    project = null;
                }
                else
                {
                    tbFromLogicalID.Text = project.fromLogicalID;
                    tbToLogicalID.Text = project.toLogicalID;
                    tbMessageID.Text = project.messageID;
                    tbBODMessage.Text = project.BOD;

                    tbBODName.Text = getBODName(tbBODMessage.Text);
                }
            }

            bwIMS.DoWork += bwIMS_DoWork;
            bwIMS.RunWorkerCompleted += bwIMS_RunWorkerCompleted;

        }

        private string getBODName(string aBOD)
        {
            string result = null;

            string[] verbs = { "Sync", "Process", "Acknowledge", "Load" };

            try
            {
                if (false == string.IsNullOrEmpty(aBOD))
                {
                    XDocument doc = XDocument.Parse(aBOD);
                    string bodName = doc.Root.Name.LocalName;

                    if(false == string.IsNullOrEmpty(bodName))
                    {
                        foreach(string verb in verbs)
                        {
                            if (bodName.StartsWith(verb))
                            {
                                result = bodName.Insert(verb.Length, ".");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return (result);
        }

        private void setEnabled(bool aEnabled)
        {

            btnIONAPI.IsEnabled = aEnabled;
            btnSend.IsEnabled = aEnabled;
            //if(aEnabled)
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;
            //}
            //else
            //{
            //    Mouse.OverrideCursor = null;
            //}
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
            setEnabled(true);
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

            setEnabled(false);
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
                        tbCI.Text = ionAPI.getClientId();
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
            if(null == project)
            {
                project = new Project();
            }
            project.fromLogicalID = tbFromLogicalID.Text;
            project.toLogicalID = tbToLogicalID.Text;
            project.messageID = tbMessageID.Text;
            project.BOD = tbBODMessage.Text;

            project.Save(projectName);

            credentials = null;
            Application.Current.Shutdown();
        }

        private void btnDetermineBOD_Click(object sender, RoutedEventArgs e)
        {
            string bodName = getBODName(tbBODMessage.Text);

            if (null != bodName)
            {
                tbBODName.Text = bodName;
            }
            else
            {
                MessageBox.Show("Couldn't determine BOD name");
            }
        }

        private void tbBODMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            string bodName = getBODName(tbBODMessage.Text);

            if(null != bodName)
            {
                tbBODName.Text = bodName;
            }
        }
    }
}
