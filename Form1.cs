using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_12
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Image image;
        Exception FileNotFoundEx = new Exception("Файл не выбран");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = openFileDialog1;
            dialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.PNG)|*.bmp;*.jpg;*.gif;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(dialog.FileName);
                pictureBox1.Image = image;
                bmp = new Bitmap(image);
            }
            else
                MessageBox.Show("Картинка не выбрана");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) // если изображение в pictureBox1 имеется
            {
                // создаём Bitmap из изображения, находящегося в pictureBox1
                Bitmap input = new Bitmap(pictureBox1.Image);
                // создаём Bitmap для черно-белого изображения
                Bitmap output = new Bitmap(input.Width, input.Height);
                // перебираем в циклах все пиксели исходного изображения
                for (int j = 0; j < input.Height; j++)
                    for (int i = 0; i < input.Width; i++)
                    {
                        // получаем (i, j) пиксель
                        UInt32 pixel = (UInt32)(input.GetPixel(i, j).ToArgb());
                        // получаем компоненты цветов пикселя
                        float R = (float)((pixel & 0x00FF0000) >> 16); // красный
                        float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
                        float B = (float)(pixel & 0x000000FF); // синий
                                                               // делаем цвет черно-белым (оттенки серого) - находим среднее арифметическое
                        R = G = B = (R + G + B) / 3.0f;
                        // собираем новый пиксель по частям (по каналам)
                        UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                        // добавляем его в Bitmap нового изображения
                        output.SetPixel(i, j, Color.FromArgb((int)newPixel));
                    }
                // выводим черно-белый Bitmap в pictureBox2
                pictureBox2.Image = output;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (bmp == null) throw FileNotFoundEx;
                SaveFileDialog dialog2 = saveFileDialog1;
                dialog2.Filter = "Image files (*.PNG, *.JPG, *.GIF, *.BMP)|*.png;*.jpg;*.gif;*.bmp";
                dialog2.ShowDialog();
                bmp.Save(dialog2.FileName);
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
                if (ex.Message == "Файл не выбран")
                {
                    button1_Click(sender, e);
                }
                 
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
