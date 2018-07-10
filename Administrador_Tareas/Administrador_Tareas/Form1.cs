using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;



namespace Administrador_Tareas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
            procesos();
        }
        public void procesos()
        {
            dgv.Columns.Add("primera", "ID");
            dgv.Columns.Add("Segunda", "Proceso");
            dgv.Columns.Add("Tercera", "Nombre");
            dgv.Columns.Add("Septima", "Ventana");
            dgv.Columns.Add("Cuarta", "Memoria RAM");
            dgv.Columns.Add("Quinta", "Memoria Virtual");
            dgv.Columns.Add("Sexta", "CPU");
            dgv.Columns.Add("Novena", "Estado");
            try
            {


                Process[] procesos;
                String estado;
                procesos = Process.GetProcesses();
                foreach (Process pro in procesos)
                {
                    dgv.Rows.Add(pro.Id, pro.ProcessName, pro.MainWindowTitle ,(pro.PeakWorkingSet64/1024)/1024,pro.VirtualMemorySize64/1024,pro.SessionId +1);
                }

            } catch (Exception e)
            {
                MessageBox.Show("Error al intentar abrir el programa", "Error al Inicio", MessageBoxButtons.OK);

            }
        }
        private void cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void maximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            maximizar.Visible = false;
            minimizar.Visible = true;
        }

        private void minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            minimizar.Visible = false;
            maximizar.Visible = true;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel_titulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel_titulo_Paint(object sender, PaintEventArgs e)
        {
                
        }
    }
}
