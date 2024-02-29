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
using System.Security.Policy;

namespace Navegador_web
{
    public partial class Form1 : Form
    {
        List<url> urls = new List<url>();
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

        private void Grabar(string Filename)
        {
            //utilizar a veces append o open.or.create
            FileStream stream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            foreach (var url in urls)
            {
                writer.WriteLine(url.Pagina);
                writer.WriteLine(url.Veces);
                writer.WriteLine(url.Fecha);
            }
            writer.Close();
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

            url urlExiste = urls.Find(u => u.Pagina == url);
            if (urlExiste == null)
            {
                url urlNueva = new url();
                urlNueva.Pagina = url;
                urlNueva.Veces = 1;
                urlNueva.Fecha = DateTime.Now;
                urls.Add(urlNueva);
                Grabar(@"C:\\Users\\ASUS\\Downloads\\Navegador-web-master\\Navegador-web-master\\Navegador web\\bin\\Debug\\historial.txt");
                webView21.CoreWebView2.Navigate(url);
            }
            else
            {
                urlExiste.Veces++;
                urlExiste.Fecha = DateTime.Now;
                Grabar(@"C:\\Users\\ASUS\\Downloads\\Navegador-web-master\\Navegador-web-master\\Navegador web\\bin\\Debug\\historial.txt");
                webView21.CoreWebView2.Navigate(urlExiste.Pagina);
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

        private void leer()
        {
            string fileName = @"C:\\Users\\ASUS\\Downloads\\Navegador-web-master\\Navegador-web-master\\Navegador web\\bin\\Debug\\historial.txt";
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            while (reader.Peek() > -1)
            {
                url url = new url();
                url.Pagina = reader.ReadLine();
                url.Veces = Convert.ToInt32(reader.ReadLine());
                url.Fecha = Convert.ToDateTime(reader.ReadLine());
                urls.Add(url);
            }

            reader.Close();
            comboBox1.DisplayMember = "pagina";
            comboBox1.DataSource = null;
            comboBox1.DataSource = urls;
            comboBox1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string nombreArchivo = @"C:\Users\ASUS\Downloads\Navegador-web-master\Navegador-web-master\Navegador web\bin\Debug\historial.txt";

            //FileStream flujo = new FileStream(nombreArchivo, FileMode.Open, FileAccess.Read);
            //StreamReader lector = new StreamReader(flujo);

            //while (lector.Peek() > -1)
            //{
            //    string textoleido = lector.ReadLine();
            //    comboBox1.Items.Add(textoleido);
            //}
            //lector.Close();
            leer();
        }
    }
}
