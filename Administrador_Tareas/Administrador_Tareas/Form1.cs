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
using System.Diagnostics; //Librería para usar la variable Process



namespace Administrador_Tareas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            //Se inicializa la función Procesos
            procesos();
        }
        public void procesos() //Se añaden las colúmnas necesarias
        {
            dgv.Columns.Add("primera", "ID");
            dgv.Columns.Add("Segunda", "Proceso");
            dgv.Columns.Add("Tercera", "Programa");
            dgv.Columns.Add("Cuarta", "Memoria RAM");
            dgv.Columns.Add("Quinta", "Memoria Virtual");
            dgv.Columns.Add("Sexta", "CPU");
            //dgv.Columns.Add("Novena", "Estado");
            try
            {
                Process[] procesos; //Se crea la matriz de tipo Process con los procesos utilizados
                procesos = Process.GetProcesses();  //Se llena la matriz de procesos
                foreach (Process pro in procesos)   //Se declara foreach que es una variable que puede ser entero, string etc
                {
                    //Se crean las filas con todos los datos
                     //Id del proceso, Nombre del proceso, Ventana principal del proceso, RAM en megas, Memoria virtual en Megas, y el número del procesador que realiza el proceso
                    dgv.Rows.Add(pro.Id, pro.ProcessName, pro.MainWindowTitle ,(pro.PeakWorkingSet64/1024)/1024+" Mb",pro.VirtualMemorySize64/1024/1024+" Mb",pro.SessionId +1);
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
