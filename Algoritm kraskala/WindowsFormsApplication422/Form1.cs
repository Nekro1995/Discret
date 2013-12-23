using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        int n = 1;
        float[,] a, a1;
        ArrayList koord = new ArrayList(); 

        public Form1()
        {
            InitializeComponent();
        }

        private void control() 
        {
            a = new float[n, n];
            a1 = new float[n, n];
            bool flag = true;
            for (short i = 0; i < n; i++)
                for (short j = 0; j < n; j++)
                    try
                    {
                        a[i, j] = Convert.ToSingle(dataGridView1[j, i].Value);
                    }
                    catch
                    {
                        a[i, j] = 0;
                        flag = false;
                    }
            if (flag == false)
            {
                MessageBox.Show("Введенное вами значение имеет некорректный формат", "Ошибкa");
                return;
            }
            for (short i = 0; i < n; i++)
                for (short j = 0; j < n; j++)
                    a1[i, j] = Convert.ToSingle(dataGridView2[j, i].Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            n = (byte)numericUpDown1.Value;
            dataGridView1.ColumnCount = n;
            dataGridView1.RowCount = n;
            dataGridView2.ColumnCount = n;
            dataGridView2.RowCount = n;
            for (short i = 0; i < n; i++)
            {
                dataGridView1[i, i].Style.BackColor = Color.Gray;
                dataGridView1[i, i].Value = 0;
                dataGridView1[i, i].ReadOnly = true;
                dataGridView2[i, i].Style.BackColor = Color.Gray;
                dataGridView2[i, i].Value = 0;
                dataGridView2[i, i].ReadOnly = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown1_ValueChanged(sender, e);
            button1.Enabled = false;
            построитьToolStripMenuItem.Enabled = false;
            button2.Enabled = false;
            progressBar1.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Maximum = n * n;
            progressBar1.Value = 0;
            Random x = new Random();
            for (short i = 0; i < n; i++)
                for (short j = 0; j < n; j++)
                {
                    progressBar1.Value++;
                    if (i != j)
                    {
                        dataGridView1[i, j].Value = x.Next(0, 20);
                    }
               }
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите сбросить старые результаты?", "Подтверждение выбора", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                progressBar1.Show();
                progressBar1.Value = 0;
                progressBar1.Maximum = 2 * n * n;
                for (short i = 0; i < n; i++)
                {
                    for (short j = 0; j < n; j++)
                    {
                        progressBar1.Value++;
                        if (i != j)
                        {
                            dataGridView1[i, j].Value = null;
                            dataGridView2[i, j].Value = null;
                            a[i, j] = 0;
                            a1[i, j] = 0;
                        }

                    }
                }
                koord.Clear();
                n = 1;
                numericUpDown1.Value = n;
                pictureBox1.Invalidate();
                pictureBox2.Invalidate();
                progressBar1.Value = 0;
                button1.Enabled = false;
                progressBar1.Value = progressBar1.Maximum;
                progressBar1.Hide();
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == e.RowIndex)
                e.Cancel = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool flag1 = true;
            button1.Enabled = true;
            try
            {
                Convert.ToDouble(dataGridView1[e.ColumnIndex, e.RowIndex].Value);
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
            }
            catch
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "Ошибка в красной ячейке (" + (e.RowIndex + 1) + "," + (e.ColumnIndex + 1) + ")";
                flag1 = false;
            }
            if (flag1)
                dataGridView1[e.RowIndex, e.ColumnIndex].Value = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            построитьToolStripMenuItem.Enabled = true;
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            progressBar1.Show();
            progressBar1.Value = 0;
            if (n <= 2)
                progressBar1.Maximum = 16;
            else
                progressBar1.Maximum = n*n*n*n/2;
            control();
            a1 = new float[n, n]; //Матрица весов окончательной матрицы
            short[,] b = new short[n, n]; //Массив компонент (двумерный)
            float[] c; //Массив ребер (вещественный)
            progressBar1.Value++;
            for (short i = 0; i < n; i++)
                for (short j = 0; j < n; j++)
                    b[i, j] = -1;
            progressBar1.Value++;
            //Заполнение массива компонент (первая строка)
            for(short i = 0; i < n; i++)
	            b[0, i] = i;
            progressBar1.Value++;
            short re = 0;
            //Проверка числа ребер
            for(short i = 0; i < n; i++)
            	for(short j = 0; j < i; j++)
	                if(a[i, j] != 0)
	            		re++;
            c = new float[re];
            re = 0;
            progressBar1.Value++;
            //Добавление ребер в массив ребер
            for(short i = 0; i < n; i++)
                for(short j = 0; j < i; j++)
             		if(a[i, j] != 0)
	               		{
	            			c[re] = a[i, j];
	            			re++;
	            		}
            progressBar1.Value++;
            //Упорядочение массива ребер по возрастанию
            float g; //Обменник (вещественная переменная)
            short l;
            while(true)
    	        {
	            l = 0;
	            for(short i = 1; i < re; i++)
		            {
			            if(c[i] < c[i - 1])
				            {
					            g = c[i - 1];
					            c[i - 1] = c[i];
    					        c[i] = g;
	    				        l++;
		    		        }
		            }
    	        if(l == 0)
	    	        break;
	            }
            progressBar1.Value++;
            //Выполнение алгоритма
            short com1 = 0, com2 = 0, n3;
            bool flag = false;
            for(short k = 0; k < re; k++)
            {	//Поиск ребра в матрице весов
	            for (short i = 0; i < n; i++)
		        for (short j = 0; j < i; j++)
                {
                    progressBar1.Value++;
                    if (c[k] == a[i, j] && c[k] != a1[i, j])
                    {
                            //Проверка вершин на принадлежность разным компонентам
                        flag = false;
                        for (short n1 = 0; n1 < n; n1++)
                        {
                            for (short n2 = 0; n2 < n; n2++)
                                if (i == b[n1, n2])
                                {
                                    com1 = n2;
                                    flag = true;
                                }
                            if (flag)
                                break;
                        }
                        flag = false;
                        for (short n1 = 0; n1 < n; n1++)
                        {
                            for (short n2 = 0; n2 < n; n2++)
                                if (j == b[n1, n2])
                                {
                                    com2 = n2;
                                    flag = true;
                                }
                            if (flag)
                                break;
                        }
                        if (com1 != com2)
                        {	//Добавление ребра в остовый лес
                            a1[i, j] = c[k];
                            a1[j, i] = c[k];
                            //Обьединение двух соединенных компонент в одну
                            n3 = 0;
                            for (short t = 0; t < n; t++)
                                if (b[t, com1] == -1)
                                {
                                    while (b[n3, com2] != -1)
                                    {
                                        b[n3 + t, com1] = b[n3, com2];
                                        b[n3, com2] = -1;
                                        n3++;
                                    }
                                    break;
                                }
                        }
                    }
                }     
			//Изьятие использованного ребра из массива ребер        
            c[k] = 0;
        }
        progressBar1.Value++;
        //На выходе получаем матрицу остовного леса
        for (short i = 0; i < n; i++)
            for (short j = 0; j < n; j++)
            {
                dataGridView2[i, j].Value = a1[i, j];
                dataGridView2[j, i].Value = a1[i, j];
            }
        progressBar1.Value++;
        progressBar1.Value++;
        progressBar1.Value = progressBar1.Maximum;
        progressBar1.Value = progressBar1.Minimum;
        progressBar1.Hide();
        pictureBox2.Invalidate();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void построитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void слЧислаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }

        private void сохранитьГрафToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point[] pnt = (Point[])koord.ToArray(typeof(Point));
            int[] x = new int[pnt.Length];
            int[] y = new int[pnt.Length];
            for (int i = 0; i < pnt.Length; i++)
            {
                x[i] = pnt[i].X;
                y[i] = pnt[i].Y;
            }
            progressBar1.Maximum = 2 * n * n;
            progressBar1.Value = 0;
            control();
            SaveFileDialog s = new SaveFileDialog();
            s.DefaultExt = ".xml";
            s.Filter = "Граф в формате *.xml|*.xml";
            if (s.ShowDialog() != DialogResult.OK) return;
            XmlTextWriter w = new XmlTextWriter(s.FileName, null);
            w.Formatting = Formatting.Indented;
            w.WriteStartDocument();
            w.WriteStartElement ("Структура");
            w.WriteStartElement ("Неориентированный_граф");
            w.WriteAttributeString( "Вершин", XmlConvert.ToString(n));
            for (short i = 0; i < n; i++)
            {
                w.WriteStartElement("Строка_" + (i + 1));
                for (short j = 0; j < n; j++)
                {
                    progressBar1.Value++;
                    w.WriteAttributeString("Яч_" + (j + 1), XmlConvert.ToString(a[i, j]));
                }
                w.WriteEndElement();
            }
            w.WriteEndElement();
            w.WriteStartElement ("Остовый_лес");
            w.WriteAttributeString ("Вершин", XmlConvert.ToString(n));
            for (short i = 0; i < n; i++)
            {
                w.WriteStartElement ("Строка_" + (i + 1));
                for (short j = 0; j < n; j++)
                {
                    progressBar1.Value++;
                    w.WriteAttributeString("Яч_" + (j + 1), XmlConvert.ToString(a1[i, j]));
                }
                w.WriteEndElement();
            }
            w.WriteEndElement();
            w.WriteStartElement ("Массив_координат");
            w.WriteAttributeString ("Вершин", XmlConvert.ToString(n));
            w.WriteStartElement ("Координаты_x");
            for (short i = 0; i < n; i++)
                w.WriteAttributeString ("В_" + (i + 1), x[i].ToString());
            w.WriteEndElement();
            w.WriteStartElement("Координаты_y");
            for (short i = 0; i < n; i++)
                w.WriteAttributeString("В_" + (i + 1), y[i].ToString());
            w.WriteEndElement();
            w.WriteEndElement();
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Close();
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Hide();
            progressBar1.Hide();
        }

        private void загрузитьГрафToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            int[] x, y;
            Point pnt = new Point();
            OpenFileDialog o = new OpenFileDialog();
            o.DefaultExt = ".xml";
            o.Filter = "Граф в формате *.xml|*.xml";
            if (o.ShowDialog() != DialogResult.OK) return;
            XmlTextReader r = new XmlTextReader(o.FileName);
            while (r.Read() && (r.Name != "Структура"));
            while (r.Read() && (r.Name != "Неориентированный_граф"));
            numericUpDown1.Value = XmlConvert.ToInt32(r.GetAttribute("Вершин"));
            n = (int)numericUpDown1.Value;
            x = new int[n];
            y = new int[n];
            for (short i = 0; i < n; i++)
            {
                while (r.Read() && (r.Name != "Строка_" + (i + 1))) ;
                for (short j = 0; j < n; j++)
                {
                    dataGridView1[j, i].Value = XmlConvert.ToDouble(r.GetAttribute("Яч_" + (j + 1)));
                }
            }
            while (r.Read() && (r.Name != "Остовый_лес")) ;
            for (short i = 0; i < n; i++)
            {
                while (r.Read() && (r.Name != "Строка_" + (i + 1))) ;
                for (short j = 0; j < n; j++)
                {
                    dataGridView2[j, i].Value = XmlConvert.ToDouble(r.GetAttribute("Яч_" + (j + 1)));
                }
            }
            while (r.Read() && (r.Name != "Массив_координат")) ;
            while (r.Read() && (r.Name != "Координаты_x")) ;
            for (short i = 0; i < n; i++)
            {
                x[i] = XmlConvert.ToInt32(r.GetAttribute("В_" + (i + 1)));
            }
            while (r.Read() && (r.Name != "Координаты_y")) ;
            for (short i = 0; i < n; i++)
            {
                y[i] = XmlConvert.ToInt32(r.GetAttribute("В_" + (i + 1)));
            }
            for (int i = 0; i < n; i++)
            {
                pnt.X = x[i];
                pnt.Y = y[i];
                koord.Add(pnt);
            }
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            koord.Clear();
            Random z1 = new Random();
            Point x = new Point();
            for (short i = 0; i < n; i++)
            {
                x.X = z1.Next(20, pictureBox1.ClientSize.Width - 20);
                x.Y = z1.Next(20, pictureBox1.ClientSize.Height - 20);
                koord.Add(x);
            }
            pictureBox1.Invalidate();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (koord.Count != n)
                return;
            Point[] pnt = (Point[])koord.ToArray(typeof(Point));
            Font ft = new Font("Times New Roman", 16);
            Size sz = new Size(8, 8);
            int k = 0;
            control();
            e.Graphics.Clear(pictureBox1.BackColor);
            Graphics gr = e.Graphics;
            foreach (Point pt in koord)
            {
                e.Graphics.FillEllipse(Brushes.Gray, new Rectangle(pt, sz));
                e.Graphics.DrawString((k + 1).ToString(), ft, Brushes.Gray, pt.X - 20, pt.Y - 20);
                k++;
            }
            for (short i = 0; i < n; i++)
                for (short j = 0; j < n; j++)
                    if (a[i, j] != 0)
                    {
                        e.Graphics.DrawLine(Pens.Gray, pnt[i].X + 4, pnt[i].Y + 4, pnt[j].X + 4, pnt[j].Y + 4);
                    }
            if (k >= n)
                button2.Enabled = true;
            else
                button2.Enabled = false;
            ft.Dispose();
        }

        public void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        { 
            MouseButtons mouse = e.Button;
            if (mouse == MouseButtons.Left)
            {
                koord.Add(e.Location);
            }
            n = koord.Count;
            numericUpDown1.Value = n;
            pictureBox1.Invalidate();
        }

        private void pictureBox2_Paint_1(object sender, PaintEventArgs e)
        {
            if (koord.Count != n)
                return;
            Point[] pnt = (Point[])koord.ToArray(typeof(Point));
            Font ft = new Font("Times New Roman", 16);
            Size sz = new Size(8, 8);
            int k = 0;
            control();
            e.Graphics.Clear(pictureBox2.BackColor);
            Graphics gr = e.Graphics;
            foreach (Point pt in koord)
            {
                e.Graphics.FillEllipse(Brushes.Gray, new Rectangle(pt, sz));
                e.Graphics.DrawString((k + 1).ToString(), ft, Brushes.Gray, pt.X - 20, pt.Y - 20);
                k++;
            }
            for (short i = 0; i < n; i++)
                for (short j = 0; j < n; j++)
                    if (a1[i, j] != 0)
                    {
                        e.Graphics.DrawLine(Pens.Gray, pnt[i].X + 4, pnt[i].Y + 4, pnt[j].X + 4, pnt[j].Y + 4);
                    }
            ft.Dispose();
        }

        private void button2_EnabledChanged(object sender, EventArgs e)
        {

        }

        
    }
}
