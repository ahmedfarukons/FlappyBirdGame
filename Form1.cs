using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FlappyBirdWin
{
	public partial class Form1 : Form
	{
		private System.Windows.Forms.Timer oyunZamanlayici;
		private Image arkaplanResmi;
		private Image zeminResmi;
		private Image boruResmi;
		private Image[] kusKareleri;
		private int aktifKareIndeksi;
		private double kusY;
		private double kusHizi;
		private const double yercekimi = 0.6;
		private const double ziplamaGucu = -8.5;
		private int boruX;
		private int boslukY;
		private const int boslukYuksekligi = 110;
		private int skor;
		private bool oyunBasladi;
		private bool oyunBitti;
		private Random rastgele;

		public Form1()
		{
			InitializeComponent();
			DoubleBuffered = true;
			Width = 400;
			Height = 600;
			Text = "Flappy Bird";

			KeyPreview = true;
			KeyDown += Form1_TusaBasildi;

			VarliklariYukle();
			OyunuSifirla();

			oyunZamanlayici = new System.Windows.Forms.Timer();
			oyunZamanlayici.Interval = 16;
			oyunZamanlayici.Tick += OyunZamanlayici_Tick;
			oyunZamanlayici.Start();
		}

		private void VarliklariYukle()
		{
			arkaplanResmi = Image.FromFile(VarlikYolu("background-day.png"));
			zeminResmi = Image.FromFile(VarlikYolu("base.png"));
			boruResmi = Image.FromFile(VarlikYolu("pipe-green.png"));
			kusKareleri = new[]
			{
				Image.FromFile(VarlikYolu("yellowbird-downflap.png")),
				Image.FromFile(VarlikYolu("yellowbird-midflap.png")),
				Image.FromFile(VarlikYolu("yellowbird-upflap.png"))
			};
		}

		private string VarlikYolu(string dosyaAdi)
		{
			var adayYollar = new List<string>();
			var baseDir = AppContext.BaseDirectory;
			adayYollar.Add(Path.Combine(baseDir, "Assets", dosyaAdi));
			adayYollar.Add(Path.Combine(Application.StartupPath, "Assets", dosyaAdi));
			adayYollar.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", dosyaAdi));

			var current = new DirectoryInfo(baseDir);
			for (int i = 0; i < 5 && current != null; i++)
			{
				adayYollar.Add(Path.Combine(current.FullName, "Assets", dosyaAdi));
				current = current.Parent;
			}

			foreach (var yol in adayYollar)
			{
				if (File.Exists(yol)) return yol;
			}

			throw new FileNotFoundException($"Varlık bulunamadı: {dosyaAdi}\nDenenen yollar:\n{string.Join("\n", adayYollar)}");
		}

		private void OyunuSifirla()
		{
			kusY = 250;
			kusHizi = 0;
			aktifKareIndeksi = 0;
			boruX = Width + 100;
			rastgele = rastgele ?? new Random();
			boslukY = rastgele.Next(150, 350);
			skor = 0;
			oyunBitti = false;
			oyunBasladi = false;
		}

		private void OyunZamanlayici_Tick(object? sender, EventArgs e)
		{
			if (!oyunBitti && oyunBasladi)
			{
				kusHizi += yercekimi;
				kusY += kusHizi;

				boruX -= 3;
				if (boruX < -80)
				{
					boruX = Width + 60;
					boslukY = rastgele.Next(150, 350);
					skor++;
				}

				var kusDikdortgen = new Rectangle(80, (int)kusY, 34, 24);
				var ustBoruDikdortgen = new Rectangle(boruX, 0, 80, boslukY - boslukYuksekligi / 2);
				var altBoruDikdortgen = new Rectangle(boruX, boslukY + boslukYuksekligi / 2, 80, Height - (boslukY + boslukYuksekligi / 2) - 100);
				var zeminDikdortgen = new Rectangle(0, Height - 100, Width, 100);

				if (kusDikdortgen.IntersectsWith(ustBoruDikdortgen) || kusDikdortgen.IntersectsWith(altBoruDikdortgen) || kusDikdortgen.IntersectsWith(zeminDikdortgen) || kusY < 0)
				{
					oyunBitti = true;
				}

				aktifKareIndeksi = (aktifKareIndeksi + 1) % kusKareleri.Length;
			}

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;

			g.DrawImage(arkaplanResmi, new Rectangle(0, 0, Width, Height));
			BorulariCiz(g);
			g.DrawImage(zeminResmi, new Rectangle(0, Height - 100, Width, 100));

			var kus = kusKareleri[aktifKareIndeksi];
			g.DrawImage(kus, new Rectangle(80, (int)kusY, kus.Width, kus.Height));

			using var hudBrush = new SolidBrush(Color.White);
			using var outline = new SolidBrush(Color.Black);
			var skorMetni = $"Skor: {skor}";
			var font = new Font("Arial", 16, FontStyle.Bold);
			g.DrawString(skorMetni, font, outline, 21, 21);
			g.DrawString(skorMetni, font, hudBrush, 20, 20);

			if (!oyunBasladi && !oyunBitti)
			{
				var msg = "SPACE ile başla";
				g.DrawString(msg, font, outline, 101, 201);
				g.DrawString(msg, font, hudBrush, 100, 200);
			}

			if (oyunBitti)
			{
				var msg = "Oyun Bitti - R: Restart, ESC: Çıkış";
				g.DrawString(msg, font, outline, 51, 301);
				g.DrawString(msg, font, hudBrush, 50, 300);
			}
		}

		private void BorulariCiz(Graphics g)
		{
			int ustBoruYuksekligi = Math.Max(0, boslukY - boslukYuksekligi / 2);
			int altBoruYuksekligi = Math.Max(0, Height - (boslukY + boslukYuksekligi / 2) - 100);

			if (ustBoruYuksekligi > 0)
			{
				var state = g.Save();
				g.TranslateTransform(boruX + 40, ustBoruYuksekligi);
				g.ScaleTransform(1f, -1f);
				g.DrawImage(boruResmi, new Rectangle(-40, 0, 80, ustBoruYuksekligi));
				g.Restore(state);
			}

			if (altBoruYuksekligi > 0)
			{
				g.DrawImage(boruResmi, new Rectangle(boruX, boslukY + boslukYuksekligi / 2, 80, altBoruYuksekligi));
			}
		}

		private void Form1_TusaBasildi(object? sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				kusHizi = ziplamaGucu;
				if (!oyunBasladi)
				{
					oyunBasladi = true;
				}
			}
			else if (e.KeyCode == Keys.R && oyunBitti)
			{
				OyunuSifirla();
			}
			else if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
	}
}
