using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Navegador_web
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView21.Size = this.ClientSize - new System.Drawing.Size(webView21.Location);
            button1.Left = this.ClientSize.Width - button1.Width;
           comboBox1.Width = button1.Left - comboBox1.Left;
        }

        private void Guardar(string nombreArchivo, string texto)
        {
            //utilizar a veces append o open.or.create
            FileStream flujo = new FileStream(nombreArchivo, FileMode.Append, FileAccess.Write);
            StreamWriter escritor = new StreamWriter(flujo);
            escritor.WriteLine(texto);
            escritor.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = comboBox1.Text.ToString();
            if (url.Contains(".") || url.Contains("/") || url.Contains(":"))
            {
                if (url.Contains("https"))
                    webView21.CoreWebView2.Navigate(url);
                else
                {
                    url = "https://" + url;
                    webView21.CoreWebView2.Navigate(url);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(url))
                {
                    url = "https://www.google.com/search?q=" + url;
                    webView21.CoreWebView2.Navigate(url);
                }
            }

            Guardar("historial.txt", comboBox1.Text);

            comboBox1.Items.Add(comboBox1.Text.ToString());
        }

        private void haciaAtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.GoBack();

        }

        private void haciaAdeanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.GoForward();
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.Navigate("https://microsoft.com");
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string nombreArchivo = @"C:\Users\ASUS\Downloads\Navegador-web-master\Navegador-web-master\Navegador web\bin\Debug\historial.txt";

            FileStream flujo = new FileStream(nombreArchivo, FileMode.Open, FileAccess.Read);
            StreamReader lector = new StreamReader(flujo);

            while (lector.Peek() > -1)
            {
                string textoleido = lector.ReadLine();
                comboBox1.Items.Add(textoleido);
            }
            lector.Close();
        }
    }
}
