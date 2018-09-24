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
using System.Threading;
using System.Timers;
using System.Management;




namespace Administrador_Tareas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Se crean las columnas en el Data Gried View
            dgv.Columns.Add("primera", "ID");
            dgv.Columns.Add("Segunda", "Proceso");
            dgv.Columns.Add("Tercera", "Programa");
            dgv.Columns.Add("Cuarta", "Memoria RAM");
            dgv.Columns.Add("Quinta", "Memoria Virtual");
            dgv.Columns.Add("Sexta", "CPU");
            timer1.Enabled = true; //Se habilita el Timer
            timer1.Start(); //Comienza el conteo del tiempo establecido
            procesos();//Se inicializa la función Procesos
          

        }

        //MUESTRA INFORMACION
         private void MuestraInformacion(string clave)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + clave);
            listView1.Items.Clear();
            ListViewGroup lstvg;
            try
            {
                foreach (ManagementObject objeto in searcher.Get())
                {
                    try
                    {
                        lstvg = listView1.Groups.Add(objeto["Name"].ToString(), objeto["Name"].ToString());
                    }
                    catch
                    {

                        lstvg = listView1.Groups.Add(objeto["Name"].ToString(), objeto["Name"].ToString());

                    }
                    if (objeto.Properties.Count <= 0)
                    {
                        MessageBox.Show("La Información No Está Disponible", "No Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    foreach (PropertyData PropiedadObjeto in objeto.Properties)
                    {
                        ListViewItem listViewItem1 = new ListViewItem(lstvg);
                        listViewItem1.Text = PropiedadObjeto.Name;

                        if (PropiedadObjeto.Value != null && PropiedadObjeto.Value.ToString() != "")
                        {
                            listViewItem1.SubItems.Add(PropiedadObjeto.Value.ToString());
                            listView1.Items.Add(listViewItem1);
                        }
                        else
                        {
                            // Informacion nula
                        }

                    }
                }
            }

            catch (Exception exp)
            {
                MessageBox.Show("No se pueden obtener datos \n" + exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //MUESTRA INFORMACION FIN

        public void procesos() //Se añaden las colúmnas necesarias
        {
            try
            {
                
                Process[] procesos; //Se crea la matriz de tipo Process con los procesos utilizados
                procesos = Process.GetProcesses();  //Se llena la matriz de procesos
                foreach (Process pro in procesos)   //Se declara foreach que es una variable que puede ser entero, string etc
                {
                    //Se crean las filas con todos los datos
                     //Id del proceso, Nombre del proceso, Ventana principal del proceso, RAM en megas, Memoria virtual en Megas, y el número del procesador que realiza el proceso
                    dgv.Rows.Add(pro.Id, pro.ProcessName, pro.MainWindowTitle ,(pro.PeakWorkingSet64/1024)/1024+" Mb",pro.VirtualMemorySize64/1024/1024+" Mb",pro.SessionId +1);
                    dgv.RefreshEdit();
                }
            } catch (Exception e) //se caputa errores
            {
                MessageBox.Show("Error al intentar abrir el programa", "Error al Inicio", MessageBoxButtons.OK);

            }
          
        }
        private void cerrar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
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

        private void timer1_Tick(object sender, EventArgs e) //Funcion de lo que hará el Timer cada tiempo establecido previamente
        {
             dgv.Rows.Clear();   //Limpia los registros
             dgv.Refresh();  //Actualiza la data gried view
            
            procesos(); //Llama la función Procesos
        }

        private void btn_Kill_Click(object sender, EventArgs e)
        {
            try {

                string id = dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString(); //Obtiene el valor de la celda seleccionada
                Process persona = Process.GetProcessById(int.Parse(id));  //Crea una variable tipo process y le da el valor del id seleccionado previamente
                persona.Kill(); //Se mata el proceso
                dgv.Rows.Remove(dgv.CurrentRow); //Removemos la celda de la data
            
            }
            catch (Exception ek){

                MessageBox.Show("Error al intentar matar el proceso", "Error", MessageBoxButtons.OK);
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgv.Visible = true;
            graficas.Visible = false;
        }

        private void dgv_Click(object sender, EventArgs e)
        {
       

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dgv.Visible = false;
            graficas.Visible = true;

        }

        private void panel_vertical_Paint(object sender, PaintEventArgs e)
        {

        }

        private void graficas_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Processor");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_BaseBoard");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_BIOS");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_VideoController");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_SoundDevice");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_DiskDrive");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_DiskPartition");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_CacheMemory");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_PhysicalMemory");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Process");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_NetworkAdapter");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Account");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_USBController");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Keyboard");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Bus");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Group");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MuestraInformacion("Win32_Thread");
        }
    }
}
