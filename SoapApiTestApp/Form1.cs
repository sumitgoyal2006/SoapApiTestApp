using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SoapApiTestApp.Sfdc;
using System.Net;

namespace SoapApiTestApp
{
    public partial class Form1 : Form
    {

        public String uname = "sumit05082018@bisp.com";
        public String pwd = "Admin123456";
        public String securityToken = "0CJFDfxO5gdo9kbEoNK36Jak";
        private String sessionId;
        private LoginResult _loginResult;
        private DateTime _nextLoginTime;
        private SforceService _sforceRef = new SforceService();
        
        public Form1()
        {
            InitializeComponent();
        }


        private bool IsConnected()
        {
            bool binResult = false;
            if (!string.IsNullOrEmpty(sessionId) & sessionId != null)
            {
                if (DateTime.Now > _nextLoginTime)
                    binResult = false;
                else
                    binResult = true;
            }

            else
                binResult = false;

            return binResult;
        }


        private void getSessionInfo()
        {
            _loginResult = new LoginResult();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _loginResult = _sforceRef.login(uname, pwd + securityToken);
            sessionId = _loginResult.sessionId;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsConnected())
            {
                getSessionInfo();
                _sforceRef.Url = _loginResult.serverUrl;
               // MessageBox.Show(_sforceRef.Url.ToString());
                _sforceRef.SessionHeaderValue = new SessionHeader();
                _sforceRef.SessionHeaderValue.sessionId = _loginResult.sessionId;
                Lead l1 = new Lead();              
                   
               
                l1.LastName = textBox1.Text;
                l1.Company = textBox2.Text;
                l1.Status = textBox3.Text;

                SaveResult[] createResult = _sforceRef.create(new sObject[] { l1 });
                if (createResult[0].success)
                {
                    MessageBox.Show("Lead Inserted Succesfully ");
                }

                    
            }
        }
    }
}
