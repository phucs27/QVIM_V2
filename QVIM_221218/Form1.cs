using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QVIM_221218
{
    public partial class FormMain : Form
    {
        //fields
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;

        //constructor
        public FormMain()
        {
            InitializeComponent();
            random= new Random();
            //this.Text=string.Empty;//loai bo close, mini,.. mac dinh cua win
            this.ControlBox = false;
            this.MaximizedBounds=Screen.FromHandle(this.Handle).WorkingArea;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //Methods
        private Color SelectThemeColor()
        {
            int index = random.Next(Themecolor.ColorList.Count);
            while (tempIndex == index)
            {
               index = random.Next(Themecolor.ColorList.Count);
            }
            tempIndex = index;
            string color = Themecolor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSender)
        {
            if(btnSender != null) 
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton= (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font= new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panel1.BackColor=color;
                    //panelLogo.BackColor=color;
                }
            }
        }
        private void DisableButton()
        {
            foreach (Control preBtn in panelMenu.Controls)
            {
                if (preBtn.GetType() == typeof(Button))
                {
                    preBtn.BackColor = Color.FromArgb(51, 51, 70);
                    preBtn.ForeColor = Color.Gainsboro;
                    preBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }   
            }
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            OpenChildform(new FormChilds.FormView(), sender);
        }

        private void buttonView2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            OpenChildform(new FormChilds.FormInformation(), sender);
        }
        
        //add form
        private void OpenChildform(Form childForm, object btnSender)
        {
            if(activeForm!=null)
            {
                activeForm.Close();
            }
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel= false;
            childForm.FormBorderStyle= FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDisplay.Controls.Add(childForm);
            this.panelDisplay.Tag= childForm;
            childForm.BringToFront();
            childForm.Show();
            //labelTitle.Text = childForm.Text;
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                this.WindowState=FormWindowState.Maximized;
            }
            else
            {
                this.WindowState=FormWindowState.Normal;
            }
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
