using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GyartoSoros
{
    public partial class MainWindow : Window
    {
        public class RectangleHelper
        {
            private static readonly Random random = new Random();

            public static string SetRandomColor(Rectangle rectangle)
            {
                string[] colors = { "piros", "zold", "kek" };
                int colorIndex = random.Next(colors.Length);
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
                return selectedColor;
            }
        }

        [Serializable]
        public class GameState
        {
            public List<string> ColorsListGrd1 { get; set; }
            public List<string> ColorsListGrd2 { get; set; }
        }

        private List<string> colorsListGrd1 = new List<string>();
        private List<Rectangle> rectanglesListGrd1 = new List<Rectangle>();
        private List<string> colorsListGrd2 = new List<string>();
        private List<Rectangle> rectanglesListGrd2 = new List<Rectangle>();
        private int meret = 8;

        public MainWindow()
        {
            InitializeComponent();
            UpdateButtonState();
        }

        private void KovetkezoKor_Click(object sender, RoutedEventArgs e)
        {
            KovetkezoKor.Content = "Következő kör";
            TablaMeret.IsEnabled = false;
            int szam1, szam2;
            if (Szam1.Text == "0" || Szam2.Text == "0")
            {
                MessageBox.Show("Nem lehet egyik játékos száma sem 0");
            }
            else
            {
                if (!int.TryParse(Szam1.Text, out szam1)) szam1 = 0;
                if (!int.TryParse(Szam2.Text, out szam2)) szam2 = 0;

                for (int i = 0; i < szam1; i++)
                {
                    AddRectangleToGrid(Grd_1, colorsListGrd1, rectanglesListGrd1);
                }

                for (int i = 0; i < szam2; i++)
                {
                    AddRectangleToGrid(Grd_2, colorsListGrd2, rectanglesListGrd2);
                }
                Pont1.Content = Convert.ToString(CalculatePoints(colorsListGrd1));
                Pont2.Content = Convert.ToString(CalculatePoints(colorsListGrd2));
                if (rectanglesListGrd1.Count == meret && rectanglesListGrd2.Count == meret)
                {
                    int points1 = CalculatePoints(colorsListGrd1);
                    int points2 = CalculatePoints(colorsListGrd2);
                    MessageBox.Show($"Játékos 1 pontok: {points1}\nJátékos 2 pontok: {points2}");
                }
                UpdateButtonState();
            }
        }

        private void Jatek(object sender, RoutedEventArgs e)
        {
            colorsListGrd1.Clear();
            rectanglesListGrd1.Clear();
            colorsListGrd2.Clear();
            rectanglesListGrd2.Clear();
            Grd_1.Children.Clear();
            Grd_2.Children.Clear();
            UpdateButtonState();
        }

        private void AddRectangleToGrid(Grid grid, List<string> colorsList, List<Rectangle> rectanglesList)
        {
            if (rectanglesList.Count >= meret)
            {
                Rectangle toRemove = rectanglesList[0];
                grid.Children.Remove(toRemove);
                rectanglesList.RemoveAt(0);
                colorsList.RemoveAt(0);
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
            MessageBoxResult result = MessageBox.Show("Új játékot kíván kezdeni ?", "",MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Rendben!", "");
                    break;
            }
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            GameState state = new GameState
            {
                ColorsListGrd1 = new List<string>(colorsListGrd1),
                ColorsListGrd2 = new List<string>(colorsListGrd2)
            };

            using (FileStream fs = new FileStream("gameState.dat", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, state);
            }
            MessageBox.Show("Játékállás elmentve.");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Biztosan be szeretnéd tölteni a mentett játékot? A jelenlegi játékmenet el fog veszni.", "Betöltés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (File.Exists("gameState.dat"))
                {
                    using (FileStream fs = new FileStream("gameState.dat", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        GameState state = (GameState)formatter.Deserialize(fs);

                        colorsListGrd1 = new List<string>(state.ColorsListGrd1);
                        colorsListGrd2 = new List<string>(state.ColorsListGrd2);

                        rectanglesListGrd1.Clear();
                        rectanglesListGrd2.Clear();
                        Grd_1.Children.Clear();
                        Grd_2.Children.Clear();

                        foreach (var color in colorsListGrd1)
                        {
                            AddRectangleFromColor(Grd_1, color, rectanglesListGrd1);
                        }
                        foreach (var color in colorsListGrd2)
                        {
                            AddRectangleFromColor(Grd_2, color, rectanglesListGrd2);
                        }
                    }
                    Pont1.Content = Convert.ToString(CalculatePoints(colorsListGrd1));
                    Pont2.Content = Convert.ToString(CalculatePoints(colorsListGrd2));
                    UpdateButtonState();
                    MessageBox.Show("Játékállás betöltve.");
                }
                else
                {
                    MessageBox.Show("Nincs elmentett játékállás.");
                }
            }
        }

        private void AddRectangleFromColor(Grid grid, string color, List<Rectangle> rectanglesList)
        {
            Rectangle doboz = new Rectangle
            {
                Height = 100,
                Width = 100,
                Margin = new Thickness(10, 10, 10, 10)
            };

            switch (color)
            {
                case "piros":
                    doboz.Fill = new SolidColorBrush(Colors.Red);
                    break;
                case "zold":
                    doboz.Fill = new SolidColorBrush(Colors.Green);
                    break;
                case "kek":
                    doboz.Fill = new SolidColorBrush(Colors.Blue);
                    break;
            }

            rectanglesList.Add(doboz);
            Grid.SetColumn(doboz, rectanglesList.Count - 1);
            Grid.SetRow(doboz, 0);
            grid.Children.Add(doboz);
        }
    }
}
