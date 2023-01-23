using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Dosyalama işlemi için bu kütüphane kullanılır.
using System.Collections;


namespace WindowsFormsApp75
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int skor = 0;
        int count;
        static string Ad_Soyad;
        int flag1, flag2, x ;
        static int[,] layout = new int[20, 20];
        static PictureBox[,] pics = new PictureBox[20, 20];
        static ArrayList skorlar = new ArrayList();

        Point[] noktalar = new Point[80];

        int player_x=10;
        int player_y=19;


        private void bombalari_ac()
        {
            for(int i=0;i<noktalar.Length;i++)

            {
                pics[noktalar[i].X, noktalar[i].Y].Visible = true;         //bomba yerleştirilen yerleri görünür hale getirir.
            }
                    
        }

        private bool nokta_kullanildimi(Point [] noktalar,int x, int y)
        {
            int durum = 0;                                                  //random koordinatlar atıyorduk. Eğer bu random koordinatlardan daha önce
            for (int i=0;i<noktalar.Length;i++)
            {                                                               //kullanılmış olan varsa o noktayı bir daha kullanmamak için.
                if (noktalar[i].X==x && noktalar[i].Y==y)
                {
                    durum = 0;
                    break;
                }
                else
                {
                    durum = 1;
                    
                }
                
            }
            if (durum == 1)
            {
                return true;
            }
            else return false; 

        }

        private int mayinlara_yakinlik(int x, int y)
        {
            int sayac = 0;
            int a = 0;
            int b = 0;

            a = x + 1;                                            //oyuncunun sağ tarafında mayın olup olmadığını kontrol etmek için
            b = y;

            for (int i = 0; i < noktalar.Length; i++)
            {
                if (noktalar[i].X == a && noktalar[i].Y == b)
                {
                    sayac++;
                    break;
                }
            }

            a = x - 1;                                          //oyuncunun sol tarafında mayın olup olmadığını kontrol etmek için
            b = y;
            for (int i = 0; i < noktalar.Length; i++)
            {
                if (noktalar[i].X == a && noktalar[i].Y == b)
                {
                    sayac++;
                    break;
                }
            }
            a = x;
            b = y + 1;                                          //oyuncunun aşağısında mayın olup olmadığını kontrol etmek için
            for (int i = 0; i < noktalar.Length; i++)
            {
                if (noktalar[i].X == a && noktalar[i].Y == b)
                {
                    sayac++;
                    break;
                }
            }
            a = x;
            b = y - 1;                                          //oyuncunun yukarısında mayın olup olmadığını kontrol etmek için
            for (int i = 0; i < noktalar.Length; i++)
            {
                if (noktalar[i].X == a && noktalar[i].Y == b)
                {
                    sayac++;
                    break;
                }
            }
            return sayac;

        }

        private void game_start()
        {


            label3.Text = mayinlara_yakinlik(player_x,player_y).ToString();
            tableLayoutPanel1.Controls.Add(pictureBox3, player_x, player_y);//oyuncunun ilk yerini koyar



            

            string Ad_Soyad="";
            
            //static int[,] layout = new int[20, 20];
            pics = new PictureBox[20, 20];
            skorlar = new ArrayList();
            noktalar = new Point[40];
            player_x = 10;
            player_y = 19;
            //butun herseyi sifirlar

            radioButton1.Checked = true;
            string dosya_yolu = Application.StartupPath.ToString() + "\\skorlar.txt";
            //Okuma işlem yapacağımız dosyanın yolunu belirtiyoruz.
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            //Bir file stream nesnesi oluşturuyoruz. 1.parametre dosya yolunu,
            //2.parametre dosyanın açılacağını,
            //3.parametre dosyaya erişimin veri okumak için olacağını gösterir.
            StreamReader sw = new StreamReader(fs);
            //Okuma işlemi için bir StreamReader nesnesi oluşturduk.

            string tutucu = " ";

            while (tutucu != null)
            {
                tutucu = sw.ReadLine();
                skorlar.Add(tutucu);
            }
            //yazi = yazi.Replace("@", System.Environment.NewLine);
            //Satır satır okuma işlemini gerçekleştirdik ve ekrana yazdırdık
            //Son satır okunduktan sonra okuma işlemini bitirdik
            sw.Close();
            fs.Close();

            //İşimiz bitince kullandığımız nesneleri iade ettik.
       

            Random rnd = new Random();

            

            
            int bomb_number;
            if (radioButton1.Checked == true) 
                bomb_number = 40;
            else if (radioButton2.Checked == true)
                bomb_number = 50;
            else
                bomb_number = 80;

            noktalar = new Point[bomb_number]; //bomba sayisi kadar kordinat olusturur

            for (x = 0; x < bomb_number; x++)
            {

                flag1 = rnd.Next(0, 20);                            //flag1 ve flag2ye random sayı atar ve buna göre bazı koordinatlara mayın yerleştirir.
                flag2 = rnd.Next(0, 20);

                while (true)
                {
                    if (nokta_kullanildimi(noktalar, flag1, flag2)) //koordinatlar daha önce kullandildi mi
                                                                    //kullanilmadiysa bu kordinatlari kaydeder
                    {
                        noktalar[x].X = flag1;
                        noktalar[x].Y = flag2;
                        

                        pics[flag1, flag2] = new PictureBox(); //yeni resim objesi olusturur
                        pics[flag1, flag2].Name = "pic" + "_" + flag1 + "_" + flag2; //bunu isimlendirir
                        pics[flag1, flag2].Size = new Size(40, 40);//bunu boyutlandırır
                        pics[flag1, flag2].Visible = false;
                        pics[flag1, flag2].Image = Properties.Resources.Avatar__Basic_Doodle_C_85_256;//bir resim atar
                        pics[flag1, flag2].SizeMode = PictureBoxSizeMode.Zoom;//resmi hizalar
                        tableLayoutPanel1.Controls.Add(pics[flag1, flag2], flag1, flag2); //bombayi bir noktaya yerlestirir
                  

                        break;
                    }

                    else  //degilse tekrar yeni kordinat secer rastgele
                    {
                        
                        flag1 = rnd.Next(0, 20);
                        flag2 = rnd.Next(0, 20);

                    }

                }
            }
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;         // program başlatıldığında oyun butonları aktif değildir .


        }


        

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        

        private void button6_Click(object sender, EventArgs e)
        {

            
           

            MessageBox.Show("3 farklı seviyede oyun tasarlandı \n" +
                            "kolay seviyede  40 mayın ,\n" +
                            "orta seviyede 50 mayın ,\n " +
                            "zor seviyede 80 mayın pembe renkte görünen bölgeye\n" +
                            "rasgele olarak koyuldu . \n" +
                            "Oyunu oynarken 4 adet buton (sağ,sol,ön,arka) ile hareket sağlayın \n" +
                            "ve sınırları aşmamaya dikkat edin .\n" +
                            "Mayına yaklaşınca mayınlara yakın yazan kısımda \n" +
                            "kaç mayına yakın olduğunu görebilirsiniz .\n" +
                            "\nGeliştirici : Zehra Karakaya ");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(player_y - 1 < 0))
            {
                if (timer1.Enabled != true)
                {
                    timer1.Start();
                }

                if (!nokta_kullanildimi(noktalar, player_x, player_y - 1)) //eger hareket etmek istedigi konumda bomba varsa oyunu sifirlar
                {

                    timer1.Stop();

                    bombalari_ac();
                    MessageBox.Show("Yeni Oyun");
                    Application.Restart();                              //Yeni oyun çağırmak için .

                }
                else
                {
                    player_y--;
                    label3.Text = mayinlara_yakinlik(player_x, player_y).ToString();
                    
                    tableLayoutPanel1.Controls.Add(pictureBox3, player_x, player_y);

                    if ((player_x == 0 && player_y == 0) || (player_x == 19 && player_y == 0)) //eğer 
                    {
                        MessageBox.Show("Tebrikler Kazandınız Yeni Oyun ?");
                        timer1.Stop();
                        dosyayaYaz();
                        Application.Restart();

                    }


                }
            }
            else
            {
                MessageBox.Show("Yukarı Çok gittiniz !!");
            }
            
            


            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!(player_x - 1 < 0))
            {
                if (timer1.Enabled != true)
                {
                    timer1.Start();
                }
                if (!nokta_kullanildimi(noktalar, player_x - 1, player_y))
                {
                    timer1.Stop();                                           //eğer bir mayınla karşılaşırsak oyun sona erecek ve mayınlar ortaya çıkacak
                    bombalari_ac();
                    MessageBox.Show("Yeni Oyun");
                    Application.Restart();
                }
                else
                {
                    player_x--;
                    tableLayoutPanel1.Controls.Add(pictureBox3, player_x, player_y);
                    label3.Text = mayinlara_yakinlik(player_x, player_y).ToString();
                    
                    if ((player_x == 0 && player_y == 0) || (player_x == 19 && player_y == 0))
                    {
                        MessageBox.Show("Tebrikler Kazandınız Yeni Oyun ?");
                        timer1.Stop();
                        dosyayaYaz();
                        Application.Restart();

                    }
                }
            }
            else
            {
                MessageBox.Show("Sola çok gittiniz !!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(player_y + 1 > 19)) {
                if (timer1.Enabled != true)
                {
                    timer1.Start();
                }
                if (!nokta_kullanildimi(noktalar, player_x, player_y + 1))
                {
                    timer1.Stop();
                    bombalari_ac();
                    MessageBox.Show("Yeni Oyun");
                    Application.Restart();
                }
                else
                {
                    player_y++;
                    tableLayoutPanel1.Controls.Add(pictureBox3, player_x, player_y);
                    label3.Text = mayinlara_yakinlik(player_x, player_y).ToString();
                    
                    if ((player_x == 0 && player_y == 0) || (player_x == 19 && player_y == 0))
                    {
                        MessageBox.Show("Tebrikler Kazandınız Yeni Oyun ?");
                        timer1.Stop();
                        dosyayaYaz();
                        Application.Restart();

                    }
                }
                
           }
            else
            {
                MessageBox.Show("aşağıya çok gittiniz!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(player_x > 19))
            {
                if (timer1.Enabled != true) // sağ , sol , yukarı ve aşağı butonlarından herhangi birine bastığında eğer timer aktif değilse aktif olmaya başlayacak
                {
                    timer1.Start();
                }

                if (!nokta_kullanildimi(noktalar, player_x + 1, player_y))
                {
                    timer1.Stop();
                    bombalari_ac();
                    MessageBox.Show("Yeni Oyun");
                    Application.Restart();
                    if ((player_x == 0 && player_y == 0) || (player_x == 19 && player_y == 0))   //sol üst veya sağ üst köşeye ulaşılırsa oyun kazanılacak.
                    {
                        MessageBox.Show("Tebrikler Kazandınız Yeni Oyun ?");
                        timer1.Stop();
                        dosyayaYaz();
                        Application.Restart();

                    }


                }
                else
                {
                    player_x++;                                                        //değilse oyuncu bir ilerletilecek
                    tableLayoutPanel1.Controls.Add(pictureBox3, player_x, player_y);
                    label3.Text = mayinlara_yakinlik(player_x, player_y).ToString();
                    
                }
            }
            else
            {
                MessageBox.Show("Sağa çok gittiniz!!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)                         //Bu fonksiyonla oyuna başladıktan sonra oyun bitene kadar sayar . En az sürede oyunu tamamlayan 
        {
            skor++;

            int saniye, dakika;

            saniye = skor;
            dakika = saniye / 60;
            saniye = saniye % 60;
            if (saniye < 10 && dakika < 10)
            {
                label4.Text = "0" + dakika.ToString() + ".0" + saniye;
            }

            else
            {
                label4.Text = "0" + dakika.ToString() + "." + saniye;

                if (dakika > 9)
                {
                    label4.Text = dakika.ToString() + "." + saniye;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            game_start();//oyunu baslatan programi yazar

            Ad_Soyad = textBox1.Text;
            
            MessageBox.Show(Ad_Soyad + " kaydedildi .");

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;  // oyun seviyesinden sonra isim de girilir ve daha sonra oyun aktif hale gelir .

        }

        private void button7_Click(object sender, EventArgs e)
        {

            string dosya_yolu = Application.StartupPath.ToString() + "\\skorlar.txt";
            //Okuma işlem yapacağımız dosyanın yolunu belirtiyoruz.
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            //Bir file stream nesnesi oluşturuyoruz. 1.parametre dosya yolunu,
            //2.parametre dosyanın açılacağını,
            //3.parametre dosyaya erişimin veri okumak için olacağını gösterir.
            StreamReader sw = new StreamReader(fs);
            //Okuma işlemi için bir StreamReader nesnesi oluşturduk.

            string tutucu = " ";
            string yazi = "";
            while (tutucu != null)
            {
               tutucu = sw.ReadLine();
                yazi = yazi + '\n' + tutucu;
            }
            //yazi = yazi.Replace("@", System.Environment.NewLine);
            //Satır satır okuma işlemini gerçekleştirdik ve ekrana yazdırdık
            //Son satır okunduktan sonra okuma işlemini bitirdik
            sw.Close();
            fs.Close();
            MessageBox.Show(yazi);
            //İşimiz bitince kullandığımız nesneleri iade ettik.
            
        }

        private  void dosyayaYaz()
        {
            string dosya_yolu = Application.StartupPath.ToString() + "\\skorlar.txt";
            //İşlem yapacağımız dosyanın yolunu belirtiyoruz.
            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            //Bir file stream nesnesi oluşturuyoruz. 1.parametre dosya yolunu,
            //2.parametre dosya varsa açılacağını yoksa oluşturulacağını belirtir,
            //3.parametre dosyaya erişimin veri yazmak için olacağını gösterir.
            StreamWriter sw = new StreamWriter(fs);
            //Yazma işlemi için bir StreamWriter nesnesi oluşturduk.
           
            for(int i=0;i<skorlar.Count;i++)//mevcut skorlar dosyaya yolladik
            {
                sw.WriteLine(skorlar[i]);
            }
            
            sw.WriteLine(Ad_Soyad+" :"+label4.Text); //yeni skoru ekledik
            //Dosyaya ekleyeceğimiz iki satırlık yazıyı WriteLine() metodu ile yazacağız.
            sw.Flush();
            //Veriyi tampon bölgeden dosyaya aktardık.
            sw.Close();
            fs.Close();
            //İşimiz bitince kullandığımız nesneleri iade ettik.
        }


    }

   
}
