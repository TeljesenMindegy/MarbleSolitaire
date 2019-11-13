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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<CheckBox> pegs = new List<CheckBox>();
        static List<string> possibleMove = new List<string>();
        static bool canMove = true;

        static int secondStep = 0;

        CheckBox GetCheckBox(int column, int row)
        {
            return FindName("Hole_" + column + "_" + row) as CheckBox;
        }

        bool isPossibleMove()
        {
            foreach (CheckBox peg in pegs)
            {
                int row = Grid.GetRow(peg);
                int column = Grid.GetColumn(peg);

                if ((GetCheckBox(column + 1, row) != null) && GetCheckBox(column + 2, row) != null)
                {
                    if ((bool)GetCheckBox(column + 1, row).IsChecked)
                    {
                        if ((bool)!(GetCheckBox(column + 2, row).IsChecked))
                        {
                            return false;
                        }
                    }
                }
                if ((GetCheckBox(column - 1, row) != null) && GetCheckBox(column - 2, row) != null)
                {
                    if ((bool)GetCheckBox(column - 1, row).IsChecked)
                    {
                        if ((bool)!(GetCheckBox(column - 2, row).IsChecked))
                        {
                            return false;
                        }
                    }
                }
                if ((GetCheckBox(column, row + 1) != null) && GetCheckBox(column, row + 2) != null)
                {
                    if ((bool)GetCheckBox(column, row + 1).IsChecked)
                    {
                        if ((bool)!(GetCheckBox(column, row + 2).IsChecked))
                        {
                            return false;
                        }
                    }
                }
                if ((GetCheckBox(column, row - 1) != null) && GetCheckBox(column, row - 2) != null)
                {
                    if ((bool)GetCheckBox(column, row - 1).IsChecked)
                    {
                        if ((bool)!(GetCheckBox(column, row - 2).IsChecked))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        void GetPegPositions()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (GetCheckBox(i, j) != null)
                    {
                        if ((bool)GetCheckBox(i, j).IsChecked)
                        {
                            pegs.Add(GetCheckBox(i, j));
                            
                        }
                    }
                }
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            CheckBox newHole;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (!(((i == 0 && j == 0) || (i == 0 && j == 1) || (i == 1 && j == 0) || (i == 1 && j == 1))
                    || ((i == 0 && j == 5) || (i == 0 && j == 6) || (i == 1 && j == 5) || (i == 1 && j == 6))
                    || ((i == 5 && j == 0) || (i == 5 && j == 1) || (i == 6 && j == 0) || (i == 6 && j == 1))
                    || ((i == 5 && j == 5) || (i == 5 && j == 6) || (i == 6 && j == 5) || (i == 6 && j == 6))))
                    {
                        newHole = new CheckBox();
                        newHole.IsChecked = true;
                        RegisterName("Hole_" + i.ToString() + "_" + j.ToString(), newHole);
                        Grid.SetRow(newHole, j);
                        Grid.SetColumn(newHole, i);
                        newHole.Click += NewHole_Click;

                        gridMain.Children.Add(newHole);
                    }
                }
            }
            GetCheckBox(3, 3).IsChecked = false;
        }

        private void NewHole_Click(object sender, RoutedEventArgs e)
        {

            int row = (Grid.GetRow(sender as CheckBox));
            int column = (Grid.GetColumn(sender as CheckBox));

            if ((bool)GetCheckBox(column, row).IsChecked)
            {
                GetCheckBox(column, row).IsChecked = false;
            }
            else
            {
                (sender as CheckBox).IsChecked = true;
            }

            
            foreach (string move in possibleMove)
            {
                string[] moveParams = move.Split('|');
                if (moveParams[0].ToString() == "Hole_" + column + "_" + row)
                {
                    if (moveParams[1].ToString() == "C+1")
                    {
                        GetCheckBox(column, row).Background = Brushes.White;
                        GetCheckBox(column, row).IsChecked = true;
                        GetCheckBox(column - 2, row).IsChecked = false;
                        GetCheckBox(column - 1, row).IsChecked = false;
                        possibleMove.Clear();
                        secondStep = 0;
                        return;
                    }
                    else if (moveParams[1].ToString() == "C-1")
                    {
                        (FindName("Hole_" + column + "_" + row) as CheckBox).Background = Brushes.White;
                        (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        (FindName("Hole_" + (column + 1) + "_" + row) as CheckBox).IsChecked = false;
                        (FindName("Hole_" + (column + 2) + "_" + row) as CheckBox).IsChecked = false;
                        possibleMove.Clear();
                        secondStep = 0;
                        return;
                    }
                    else if (moveParams[1].ToString() == "R+1")
                    {
                        (FindName("Hole_" + column + "_" + row) as CheckBox).Background = Brushes.White;
                        (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        (FindName("Hole_" + column + "_" + (row - 1)) as CheckBox).IsChecked = false;
                        (FindName("Hole_" + column + "_" + (row - 2)) as CheckBox).IsChecked = false;
                        possibleMove.Clear();
                        secondStep = 0;
                        return;
                    }
                    else
                    {
                        (FindName("Hole_" + column + "_" + row) as CheckBox).Background = Brushes.White;
                        (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        (FindName("Hole_" + column + "_" + (row + 1)) as CheckBox).IsChecked = false;
                        (FindName("Hole_" + column + "_" + (row + 2)) as CheckBox).IsChecked = false;
                        possibleMove.Clear();
                        secondStep = 0;
                        return;
                    }
                }
            }

            if (((FindName("Hole_" + (column + 1) + "_" + row) as CheckBox) != null) && (FindName("Hole_" + (column + 2) + "_" + row) as CheckBox) != null)
            {
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    if ((bool)(FindName("Hole_" + (column + 1) + "_" + row) as CheckBox).IsChecked)
                    {
                        if ((bool)!((FindName("Hole_" + (column + 2) + "_" + row) as CheckBox).IsChecked))
                        {
                            possibleMove.Add("Hole_" + (column + 2) + "_" + row + "|C+1");
                        }
                        else
                        {
                            (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        }
                    }
                }
            }

            if (((FindName("Hole_" + (column - 1) + "_" + row) as CheckBox) != null) && (FindName("Hole_" + (column - 2) + "_" + row) as CheckBox) != null)
            {
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    if ((bool)(FindName("Hole_" + (column - 1) + "_" + row) as CheckBox).IsChecked)
                    {
                        if ((bool)!((FindName("Hole_" + (column - 2) + "_" + row) as CheckBox).IsChecked))
                        {
                            possibleMove.Add("Hole_" + (column - 2) + "_" + row + "|C-1");
                        }
                        else
                        {
                            (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        }
                    }
                }
            }

            if (((FindName("Hole_" + column + "_" + (row + 1)) as CheckBox) != null) && (FindName("Hole_" + column + "_" + (row + 2)) as CheckBox) != null)
            {
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    if ((bool)(FindName("Hole_" + column + "_" + (row + 1)) as CheckBox).IsChecked)
                    {
                        if (!(bool)((FindName("Hole_" + column + "_" + (row + 2)) as CheckBox).IsChecked))
                        {
                            possibleMove.Add("Hole_" + column + "_" + (row + 2) + "|R+1");
                        }
                        else
                        {
                            (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        }
                    }
                }

            }

            if (((FindName("Hole_" + column + "_" + (row - 1)) as CheckBox) != null) && (FindName("Hole_" + column + "_" + (row - 2)) as CheckBox) != null)
            {
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    if ((bool)(FindName("Hole_" + column + "_" + (row - 1)) as CheckBox).IsChecked)
                    {
                        if ((bool)!((FindName("Hole_" + column + "_" + (row - 2)) as CheckBox).IsChecked))
                        {
                            possibleMove.Add("Hole_" + column + "_" + (row - 2) + "|R-1");
                        }
                        else
                        {
                            (FindName("Hole_" + column + "_" + row) as CheckBox).IsChecked = true;
                        }
                    }
                }
            }
            secondStep++;

            foreach (string move in possibleMove)
            {
                string[] moveParams = move.Split('|');
                (FindName(moveParams[0]) as CheckBox).Background = Brushes.Red;
            }

            if (secondStep == 2)
            {
                foreach (string move in possibleMove)
                {
                    string[] moveParams = move.Split('|');
                    (FindName(moveParams[0]) as CheckBox).Background = Brushes.White;
                }
                possibleMove.Clear();
                secondStep = 0;
            }

            pegs.Clear();
            GetPegPositions();
            canMove = !isPossibleMove();

            if (!canMove)
            {
                pegs.Remove(GetCheckBox(3, 3));
                if (pegs.Count == 0)
                {
                    MessageBox.Show("Nyertél!");
                }
                else
                {
                    MessageBox.Show("Vesztettél!");
                }
            }
        }
    }
}
