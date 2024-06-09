using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GyartoSoros
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class RectangleHelper
        {
            private static readonly Random random = new Random();

            public static string SetRandomColor(Rectangle rectangle)
            {
                // Színek tömbje
                string[] colors = { "piros", "zold", "kek" };

                // Generálj egy random számot 0 és 2 között
                int colorIndex = random.Next(colors.Length);

                // Állítsd be a színt a random szám alapján
                string selectedColor = colors[colorIndex];
                switch (selectedColor)
                {
                    case "piros":
                        rectangle.Fill = new SolidColorBrush(Colors.Red);
                        break;
                    case "zold":
                        rectangle.Fill = new SolidColorBrush(Colors.Green);
                        break;
                    case "kek":
                        rectangle.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                }

                // Visszatér a kiválasztott színnel
                return selectedColor;
            }
        }

        // Listák a színek és a rectangle objektumok tárolására
        private List<string> colorsListGrd1 = new List<string>();
        private List<Rectangle> rectanglesListGrd1 = new List<Rectangle>();
        private List<string> colorsListGrd2 = new List<string>();
        private List<Rectangle> rectanglesListGrd2 = new List<Rectangle>();

        // Grid méret
        private int meret = 8;
        
        public MainWindow()
        {
            InitializeComponent();
            UpdateButtonState();
        }

        private void KovetkezoKor_Click(object sender, RoutedEventArgs e)
        {
            // Beolvasni a játékosok választásait
            int szam1, szam2;
            if (!int.TryParse(Szam1.Text, out szam1)) szam1 = 0;
            if (!int.TryParse(Szam2.Text, out szam2)) szam2 = 0;

            // Hozzáadni a rectangle-ket az első játékoshoz
            for (int i = 0; i < szam1; i++)
            {
                AddRectangleToGrid(Grd_1, colorsListGrd1, rectanglesListGrd1);
            }

            // Hozzáadni a rectangle-ket a második játékoshoz
            for (int i = 0; i < szam2; i++)
            {
                AddRectangleToGrid(Grd_2, colorsListGrd2, rectanglesListGrd2);
            }

            // Pontok számítása és megjelenítése, ha mindkét játékos elérte a "meret"-et
            if (rectanglesListGrd1.Count == meret && rectanglesListGrd2.Count == meret)
            {
                int points1 = CalculatePoints(colorsListGrd1);
                int points2 = CalculatePoints(colorsListGrd2);
                MessageBox.Show($"Játékos 1 pontok: {points1}\nJátékos 2 pontok: {points2}");
            }

            // Frissíteni a gomb állapotát
            UpdateButtonState();
        }

        private void Jatek(object sender, RoutedEventArgs e)
        {
            // Tisztítsuk meg a listákat és a Grid-eket
            colorsListGrd1.Clear();
            rectanglesListGrd1.Clear();
            colorsListGrd2.Clear();
            rectanglesListGrd2.Clear();

            Grd_1.Children.Clear();
            Grd_2.Children.Clear();

            // Frissíteni a gomb állapotát
            UpdateButtonState();
        }

        private void AddRectangleToGrid(Grid grid, List<string> colorsList, List<Rectangle> rectanglesList)
        {
            if (rectanglesList.Count >= meret)
            {
                // Ha a lista mérete eléri a "meret"-et, távolítsuk el az első elemet
                Rectangle toRemove = rectanglesList[0];
                grid.Children.Remove(toRemove);
                rectanglesList.RemoveAt(0);
                colorsList.RemoveAt(0);

                // Frissítsük a grid oszlopait
                for (int i = 0; i < rectanglesList.Count; i++)
                {
                    Grid.SetColumn(rectanglesList[i], i);
                }
            }

            Rectangle doboz = new Rectangle
            {
                Height = 100,
                Width = 100,
                Margin = new Thickness(10, 10, 10, 10)
            };

            // Véletlenszerű szín beállítása és rögzítése
            string color = RectangleHelper.SetRandomColor(doboz);
            colorsList.Add(color);
            rectanglesList.Add(doboz);

            Grid.SetColumn(doboz, rectanglesList.Count - 1);
            Grid.SetRow(doboz, 0);
            grid.Children.Add(doboz);
        }

        private int CalculatePoints(List<string> colorsList)
        {
            int points = 0;
            foreach (string color in colorsList)
            {
                switch (color)
                {
                    case "piros":
                        points += 3;
                        break;
                    case "kek":
                        points += 2;
                        break;
                    case "zold":
                        points += 1;
                        break;
                }
            }
            return points;
        }

        private void UpdateButtonState()
        {
            KovetkezoKor.IsEnabled = rectanglesListGrd1.Count < meret || rectanglesListGrd2.Count < meret;
        }

        private void UjJatek_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void TablaMeret_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablaMeret.SelectedIndex == 0)
            {
                meret = 8;
            }
            else if (TablaMeret.SelectedIndex == 1)
            {
                meret = 12;
            }
            else if (TablaMeret.SelectedIndex == 2)
            {
                meret = 16;
            }
        }
    }
}