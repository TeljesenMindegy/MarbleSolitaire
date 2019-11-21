using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PegSolitaire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private RestApi rest = new RestApi();
        private static List<CheckBox> pegs = new List<CheckBox>();
        private static List<Move> possibleMove = new List<Move>();
        private static bool canMove = true;
        private string playerName;
        private Scoring scoring;
        private Stopwatch stopWatch = new Stopwatch();
        #endregion

        #region Functions and methods
        private void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
            }
        }

        private CheckBox GetCheckBox(int column, int row)
        {
            CheckBox checkBox = FindName("Hole_" + column + "_" + row) as CheckBox;
            if (checkBox != null)
            {
                checkBox.Name = "Hole_" + column + "_" + row;
            }
            return checkBox;
        }

        private bool IsPossibleMove()
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

        private void GetPossibleMove(CheckBox checkBox)
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
                move.Peg.Opacity = 1;
            }
        }

        private void GetPegPositions()
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

        private void MakeMove(Move move)
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
            ClearPossible();
        }

        private void ClearPossible()
        {
            foreach (CheckBox checkBox in gridMain.Children)
            {
                if (checkBox.IsChecked == false)
                checkBox.Opacity = 0.25;
            }
        }

        private void Start()
        {
            gridMain.Children.Clear();
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
                        if (GetCheckBox(i, j) != null)
                        {
                            UnregisterName("Hole_" + i.ToString() + "_" + j.ToString());
                        }
                        RegisterName("Hole_" + i.ToString() + "_" + j.ToString(), newHole);
                        Grid.SetRow(newHole, j);
                        Grid.SetColumn(newHole, i);
                        newHole.Click += NewHole_Click;
                        newHole.VerticalAlignment = VerticalAlignment.Center;
                        newHole.HorizontalAlignment = HorizontalAlignment.Center;

                        gridMain.Children.Add(newHole);

                    }
                }
            }
            GetCheckBox(3, 3).IsChecked = false;
            GetCheckBox(3, 3).Opacity = 0.25;
        }

        private void NewHole_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Start();

            TimeSpan ts = stopWatch.Elapsed;

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
                ClearPossible();
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
                            Win.Visibility = Visibility.Visible;
                            if (stopWatch.IsRunning)
                            {
                                stopWatch.Stop();
                            }
                            scoring = new Scoring(playerName, ts.TotalSeconds);
                            rest.SaveData(scoring);
                            WinTime.Content = string.Format("Time: {0:00}:{1:00}", ts.Minutes, ts.Seconds);
                        }
                        else
                        {
                            GameOver.Visibility = Visibility.Visible;
                        }
                        stopWatch.Reset();
                    }
                    return;
                }
            }
            GetPossibleMove(sender as CheckBox);
        }

        private void SetName()
        {
            if (tbPlayerName.Text.Length >= 3 && tbPlayerName.Text != string.Empty)
            {
                playerName = tbPlayerName.Text;
                HideLogin();
                HideMenu(false);
            }
        }

        private void LoadScores()
        {
            List<Scoreboard> Scores = rest.LoadData();
            dgScores.ItemsSource = Scores;
        }
        #endregion

        #region Buttons
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start();
            HideMenu();
            HideGame(false);
        }

        private void BtnHighscore_Click(object sender, RoutedEventArgs e)
        {
            HideMenu();
            HideHighScore(false);
            LoadScores();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            HideGame();
            HideGameOver();
            HideWin();
            HideMenu(false);
            HideHighScore();
        }

        private void SetName_Click(object sender, RoutedEventArgs e)
        {
            SetName();
        }

        private void tbPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetName();
        }
        #endregion

        #region Views
        private void HideGame(bool hide = true)
        {
            Game.Visibility = hide ? Visibility.Hidden : Visibility.Visible;
        }

        private void HideMenu(bool hide = true)
        {
            Menu.Visibility = hide ? Visibility.Hidden : Visibility.Visible;
        }

        private void HideHighScore(bool hide = true)
        {
            Highscore.Visibility = hide ? Visibility.Hidden : Visibility.Visible;
        }

        private void HideGameOver(bool hide = true)
        {
            GameOver.Visibility = hide ? Visibility.Hidden : Visibility.Visible;
        }

        private void HideLogin(bool hide = true)
        {
            Login.Visibility = hide ? Visibility.Hidden : Visibility.Visible;
        }

        private void HideWin(bool hide = true)
        {
            Win.Visibility = hide ? Visibility.Hidden : Visibility.Visible;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = scoring;
        }
    }
}
