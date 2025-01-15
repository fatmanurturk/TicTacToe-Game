using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tictactoee
{
    public partial class Form1 : Form
    {
        private bool OyuncuX = true; // Oyuncu değişimi için sınıf seviyesinde tanımlandı
        private int ucSayisiX = 0; // X oyuncusu için üçlü sayacı
        private int ucSayisiO = 0; // O oyuncusu için üçlü sayacı
        private bool oyunBitti = false;

        public Form1()
        {
            InitializeComponent();
            btnStart.Click += btnStart_Click;
            btnStart.Paint += btnStart_Paint;
        }
        private List<Tuple<Point, Point>> cizimler = new List<Tuple<Point, Point>>();

        private void UzeriniCiz(Button btn1, Button btn2, Button btn3)
        {
            // Çizgi için bir kalem tanımlayın
            Pen pen = new Pen(Color.Red, 4);

            // İlk ve son butonun merkezlerini hesaplayın
            Point startPoint = new Point(btn1.Left + btn1.Width / 2, btn1.Top + btn1.Height / 2);
            Point endPoint = new Point(btn3.Left + btn3.Width / 2, btn3.Top + btn3.Height / 2);

            // Panelin Graphics nesnesini kullanarak çizgi çizin
            using (Graphics g = panel1.CreateGraphics())
            {
                g.DrawLine(pen, startPoint, endPoint);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (oyunBitti) return;
            Button btn = sender as Button;
            if (btn != null && btn.Text == "")
            {
                if (OyuncuX)
                {
                    btn.Text = "X";
                }
                else
                {
                    btn.Text = "O";
                }
                OyuncuX = !OyuncuX; // Oyuncu değişimi sağlanıyor

                int boyut = Convert.ToInt32(comboBox1.SelectedItem.ToString().Substring(0, 1));
                UcluKontrolu(boyut); // Her hamlede üçlü kontrolü yapılır

                // Oyun bitimi kontrolü
                TümKutularDolu(boyut); // Eğer tüm kutular dolarsa, kazananı belirle
            }
        }
        // Daha önce sayılmış üçlülerin koordinatlarını depolamak için
        private HashSet<string> sayilmisUcluler = new HashSet<string>();

        private void UcluKontrolu(int boyut)
        {
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut - 2; j++)
                {
                    // Yatay üçlü kontrolü
                    if (((Button)panel1.Controls[$"btn_{i}_{j}"]).Text != "" &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i}_{j + 1}"]).Text &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i}_{j + 2}"]).Text)
                    {
                        string ucluKoordinatlar = $"Yatay-{i}-{j}";
                        if (!sayilmisUcluler.Contains(ucluKoordinatlar))
                        {
                            UcluSayisiniArttir(((Button)panel1.Controls[$"btn_{i}_{j}"]).Text);
                            sayilmisUcluler.Add(ucluKoordinatlar);
                            UzeriniCiz(
                                (Button)panel1.Controls[$"btn_{i}_{j}"],
                                (Button)panel1.Controls[$"btn_{i}_{j + 1}"],
                                (Button)panel1.Controls[$"btn_{i}_{j + 2}"]
                            );
                        }
                    }

                    // Dikey üçlü kontrolü
                    if (i < boyut - 2 &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text != "" &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i + 1}_{j}"]).Text &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i + 2}_{j}"]).Text)
                    {
                        string ucluKoordinatlar = $"Dikey-{i}-{j}";
                        if (!sayilmisUcluler.Contains(ucluKoordinatlar))
                        {
                            UcluSayisiniArttir(((Button)panel1.Controls[$"btn_{i}_{j}"]).Text);
                            sayilmisUcluler.Add(ucluKoordinatlar);
                            UzeriniCiz(
                                (Button)panel1.Controls[$"btn_{i}_{j}"],
                                (Button)panel1.Controls[$"btn_{i + 1}_{j}"],
                                (Button)panel1.Controls[$"btn_{i + 2}_{j}"]
                            );
                        }
                    }

                    // Çapraz üçlü kontrolü (sol üstten sağ alta)
                    if (i < boyut - 2 && j < boyut - 2 &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text != "" &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i + 1}_{j + 1}"]).Text &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i + 2}_{j + 2}"]).Text)
                    {
                        string ucluKoordinatlar = $"Capraz1-{i}-{j}";
                        if (!sayilmisUcluler.Contains(ucluKoordinatlar))
                        {
                            UcluSayisiniArttir(((Button)panel1.Controls[$"btn_{i}_{j}"]).Text);
                            sayilmisUcluler.Add(ucluKoordinatlar);
                            UzeriniCiz(
                                (Button)panel1.Controls[$"btn_{i}_{j}"],
                                (Button)panel1.Controls[$"btn_{i + 1}_{j + 1}"],
                                (Button)panel1.Controls[$"btn_{i + 2}_{j + 2}"]
                            );
                        }
                    }

                    // Çapraz üçlü kontrolü (sağ üstten sol alta)
                    if (i < boyut - 2 && j >= 2 &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text != "" &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i + 1}_{j - 1}"]).Text &&
                        ((Button)panel1.Controls[$"btn_{i}_{j}"]).Text == ((Button)panel1.Controls[$"btn_{i + 2}_{j - 2}"]).Text)
                    {
                        string ucluKoordinatlar = $"Capraz2-{i}-{j}";
                        if (!sayilmisUcluler.Contains(ucluKoordinatlar))
                        {
                            UcluSayisiniArttir(((Button)panel1.Controls[$"btn_{i}_{j}"]).Text);
                            sayilmisUcluler.Add(ucluKoordinatlar);
                            UzeriniCiz(
                                (Button)panel1.Controls[$"btn_{i}_{j}"],
                                (Button)panel1.Controls[$"btn_{i + 1}_{j - 1}"],
                                (Button)panel1.Controls[$"btn_{i + 2}_{j - 2}"]
                            );
                        }
                    }
                }
            }
        }


        private void UcluSayisiniArttir(string oyuncu)
        {
            if (oyuncu == "X")
            {
                ucSayisiX++;
            }
            else if (oyuncu == "O")
            {
                ucSayisiO++;
            }
        }


        // Oyun sonunda en çok üçlü yapan oyuncuyu belirleme
        private void KazananBelirle()
        {
            if (ucSayisiX > ucSayisiO)
            {
                MessageBox.Show("1. Oyuncu kazandı! ");
            }
            else if (ucSayisiO > ucSayisiX)
            {
                MessageBox.Show("2. Oyuncu kazandı! ");
            }
            else
            {
                MessageBox.Show("Berabere! ");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçim değişikliği olduğunda yapılacak işlemleri buraya ekleyin
        }

        // Tüm kutular dolduğunda oyunun bitip bitmediğini kontrol et
        private void TümKutularDolu(int boyut)
        {
            bool tumKutularDolu = true;

            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    Button btn = (Button)panel1.Controls[$"btn_{i}_{j}"];
                    if (btn.Text == "") // Eğer bir kutu boşsa
                    {
                        tumKutularDolu = false;
                        break;
                    }
                }
                if (!tumKutularDolu)
                    break;
            }

            if (tumKutularDolu) // Eğer tüm kutular dolduysa kazananı belirle
            {
                KazananBelirle();
                oyunBitti = true;
            }
        }

        private void StartGame()
        {
            if (comboBox1.SelectedItem != null)
            {
                string secilenBoyut = comboBox1.SelectedItem.ToString();
                int boyut;

                if (secilenBoyut == "3X3")
                {
                    boyut = 3;
                    MessageBox.Show("3x3 boyutunda oyun başlıyor ");
                }
                else if (secilenBoyut == "5X5")
                {
                    boyut = 5;
                    MessageBox.Show("5x5 boyutunda oyun başlıyor ");
                }
                else if (secilenBoyut == "7X7")
                {
                    boyut = 7;
                    MessageBox.Show("7x7 boyutunda oyun başlıyor ");
                }
                else
                {
                    MessageBox.Show("Geçersiz boyut!");
                    return;
                }

                // Üçlü sayılarını sıfırla
                ucSayisiX = 0;
                ucSayisiO = 0;
                oyunBitti = false;

                // Paneli temizleyip butonları yerleştiriyoruz
                panel1.Controls.Clear();
                int butonBoyutu = 70;

                for (int i = 0; i < boyut; i++)
                {
                    for (int j = 0; j < boyut; j++)
                    {
                        Button btn = new Button();
                        btn.Width = butonBoyutu;
                        btn.Height = butonBoyutu;

                        btn.Left = j * butonBoyutu;
                        btn.Top = i * butonBoyutu;

                        btn.Name = $"btn_{i}_{j}";
                        btn.Click += button1_Click;

                        panel1.Controls.Add(btn);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen oyun boyutunu seçin!");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame(); // Başlat butonuna basıldığında oyunu başlatır
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Red, 4);  // Çizgi rengini ve kalınlığını belirle
            foreach (var cizim in cizimler)
            {
                e.Graphics.DrawLine(pen, cizim.Item1, cizim.Item2);
            }
        }

        private void btnStart_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Pen pen = new Pen(Color.Pink, 12);
                e.Graphics.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }
        }
    }
}
