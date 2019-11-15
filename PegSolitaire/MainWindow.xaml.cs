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

namespace PegSolitaire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<CheckBox> pegs = new List<CheckBox>();
        static List<Move> possibleMove = new List<Move>();
        static bool canMove = true;

        CheckBox GetCheckBox(int column, int row)
        {
            CheckBox checkBox = FindName("Hole_" + column + "_" + row) as CheckBox;
            if (checkBox != null)
            {
                checkBox.Name = "Hole_" + column + "_" + row;
            }
            return checkBox;
        }

        bool IsPossibleMove()
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

        void GetPossibleMove(CheckBox checkBox)
        {
            int column = Grid.GetColumn(checkBox);
            int row = Grid.GetRow(checkBox);

            if (GetCheckBox(column + 1, row) != null && GetCheckBox(column + 2, row) != null)
            {
                if ((bool)checkBox.IsChecked)
                {
                    if ((bool)GetCheckBox(column + 1, row).IsChecked)
                    {
                        if ((bool)!GetCheckBox(column + 2, row).IsChecked)
                        {
                            Move move = new Move(GetCheckBox(column + 2, row), Direction.ColumnPlus);
                            possibleMove.Add(move);
                        }
                        else
                        {
                            checkBox.IsChecked = true;
                        }
                    }
                }
            }

            if (GetCheckBox(column - 1, row) != null && GetCheckBox(column - 2, row) != null)
            {
                if ((bool)checkBox.IsChecked)
                {
                    if ((bool)GetCheckBox(column - 1, row).IsChecked)
                    {
                        if ((bool)!GetCheckBox(column - 2, row).IsChecked)
                        {
                            Move move = new Move(GetCheckBox(column - 2, row), Direction.ColumnMinus);
                            possibleMove.Add(move);
                        }
                        else
                        {
                            checkBox.IsChecked = true;
                        }
                    }
                }
            }

            if (GetCheckBox(column, row + 1) != null && GetCheckBox(column, row + 2) != null)
            {
                if ((bool)checkBox.IsChecked)
                {
                    if ((bool)GetCheckBox(column, row + 1).IsChecked)
                    {
                        if (!(bool)GetCheckBox(column, row + 2).IsChecked)
                        {
                            Move move = new Move(GetCheckBox(column, row + 2), Direction.RowPlus);
                            possibleMove.Add(move);
                        }
                        else
                        {
                            checkBox.IsChecked = true;
                        }
                    }
                }
            }

            if (GetCheckBox(column, row - 1) != null && GetCheckBox(column, row - 2) != null)
            {
                if ((bool)checkBox.IsChecked)
                {
                    if ((bool)GetCheckBox(column, row - 1).IsChecked)
                    {
                        if ((bool)!GetCheckBox(column, row - 2).IsChecked)
                        {
                            Move move = new Move(GetCheckBox(column, row - 2), Direction.RowMinus);
                            possibleMove.Add(move);
                        }
                        else
                        {
                            checkBox.IsChecked = true;
                        }
                    }
                }
            }
            foreach (Move move in possibleMove)
            {
                move.Peg.Background = Brushes.Red;
            }
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

        void MakeMove(Move move)
        {
            int column = Grid.GetColumn(move.Peg);
            int row = Grid.GetRow(move.Peg);
            move.Peg.IsChecked = true;

            if (move.Direction == Direction.ColumnPlus)
            {
                GetCheckBox(column - 2, row).IsChecked = false;
                GetCheckBox(column - 1, row).IsChecked = false;
            }
            else if (move.Direction == Direction.ColumnMinus)
            {
                GetCheckBox(column + 1, row).IsChecked = false;
                GetCheckBox(column + 2, row).IsChecked = false;
            }
            else if (move.Direction == Direction.RowPlus)
            {
                GetCheckBox(column, row - 1).IsChecked = false;
                GetCheckBox(column, row - 2).IsChecked = false;
            }
            else
            {
                GetCheckBox(column, row + 1).IsChecked = false;
                GetCheckBox(column, row + 2).IsChecked = false;
            }
            possibleMove.Clear();
            clearPossible();
        }

        void clearPossible()
        {
            foreach (CheckBox checkBox in gridMain.Children)
            {
                checkBox.Background = Brushes.White;
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

            bool reset = true;
            foreach (Move move in possibleMove)
            {
                if (move.Peg == (sender as CheckBox))
                {
                    reset = false;
                }
            }
            if (reset)
            {
                possibleMove.Clear();
                clearPossible();
            }
            
            foreach (Move move in possibleMove)
            {
                if (move.Peg.Name == "Hole_" + column + "_" + row)
                {
                    MakeMove(move);
                    pegs.Clear();
                    GetPegPositions();
                    canMove = !IsPossibleMove();

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
                    return;
                }
            }
            GetPossibleMove(sender as CheckBox);
        }
    }
}
