using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Commands;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.Enumerations;
using MinesweeperML.Models;
using MinesweeperML.Views;

namespace MinesweeperML.ViewsModel
{
    /// <summary>
    /// Minesweeper viewmodel.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class MinesweeperViewModel : BaseViewModel
    {
        private readonly IMapper mapper;
        private readonly MinesweeperDbContextFactory minesweeperDbContextFactory;
        private RelayCommand changeModeCommand;
        private int columns;
        private ClickMode currentMode = ClickMode.Reveal;
        private Difficulty difficulty = Difficulty.Custom;
        private DispatcherTimer dispatcherTimer;
        private bool? gameWon;
        private RelayCommand goBackCommand;
        private bool isFirstClick = true;
        private int numberOfBombs;
        private int remaningBombs;
        private int rows;
        private RelayCommand startNewGameCommand;
        private RelayCommand<TileViewModel> tileClickedCommand;
        private RelayCommand<TileViewModel> tileRightClickedCommand;
        private ObservableCollection<ObservableCollection<TileViewModel>> tiles = new ObservableCollection<ObservableCollection<TileViewModel>>();

        private Stopwatch time;

        /// <summary>
        /// Gets the change mode command.
        /// </summary>
        /// <value>The change mode command.</value>
        public RelayCommand ChangeModeCommand => changeModeCommand ??= new RelayCommand(param => this.ChangeMode());

        /// <summary>
        /// Gets or sets the current mode.
        /// </summary>
        /// <value>The current mode.</value>
        public ClickMode CurrentMode
        {
            get
            {
                return currentMode;
            }
            set
            {
                if (value != currentMode)
                {
                    currentMode = value;
                    NotifyPropertyChanged(nameof(CurrentMode));
                }
            }
        }

        /// <summary>
        /// Gets or sets the game won.
        /// </summary>
        /// <value>The game won.</value>
        public bool? GameWon
        {
            get
            {
                return gameWon;
            }
            set
            {
                if (value != gameWon)
                {
                    gameWon = value;
                    NotifyPropertyChanged(nameof(GameWon));
                }
            }
        }

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        /// <value>The go back command.</value>
        public RelayCommand GoBackCommand => goBackCommand ??= new RelayCommand(param => this.GoBack());

        /// <summary>
        /// Gets or sets the main menu view model.
        /// </summary>
        /// <value>The main menu view model.</value>
        public MainMenuViewModel MainMenuViewModel { get; set; }

        /// <summary>
        /// Gets or sets the remaining bombs.
        /// </summary>
        /// <value>The remaining bombs.</value>
        public int RemainingBombs
        {
            get
            {
                return remaningBombs;
            }
            set
            {
                if (value != remaningBombs)
                {
                    remaningBombs = value;
                    NotifyPropertyChanged(nameof(RemainingBombs));
                }
            }
        }

        /// <summary>
        /// Gets the start new game command.
        /// </summary>
        /// <value>The start new game command.</value>
        public RelayCommand StartNewGameCommand => startNewGameCommand ??= new RelayCommand(param => this.GenerateList());

        /// <summary>
        /// Gets the tile clicked command.
        /// </summary>
        /// <value>The tile clicked command.</value>
        public RelayCommand<TileViewModel> TileClickedCommand => tileClickedCommand ??= new RelayCommand<TileViewModel>(param => this.TileClicked(param));

        /// <summary>
        /// Gets the tile right clicked command.
        /// </summary>
        /// <value>The tile right clicked command.</value>
        public RelayCommand<TileViewModel> TileRightClickedCommand => tileRightClickedCommand ??= new RelayCommand<TileViewModel>(param => this.TileRightClicked(param));

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public ObservableCollection<ObservableCollection<TileViewModel>> Tiles
        {
            get
            {
                return tiles;
            }
            set
            {
                if (value != tiles)
                {
                    tiles = value;
                    NotifyPropertyChanged(nameof(Tiles));
                }
            }
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public Stopwatch Time
        {
            get
            {
                return time;
            }
            set
            {
                if (value != time)
                {
                    time = value;
                    NotifyPropertyChanged(nameof(Time));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinesweeperViewModel" /> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="minesweeperDbContextFactory">
        /// The minesweeper database context factory.
        /// </param>
        public MinesweeperViewModel(IMapper mapper, MinesweeperDbContextFactory minesweeperDbContextFactory)
        {
            this.mapper = mapper;
            this.minesweeperDbContextFactory = minesweeperDbContextFactory;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="numberOfBombs">The number of bombs.</param>
        /// <param name="difficulty">The difficulty.</param>
        public void StartGame(int columns, int rows, int numberOfBombs, Difficulty difficulty)
        {
            this.numberOfBombs = numberOfBombs;
            this.columns = columns;
            this.rows = rows;
            this.difficulty = difficulty;
            RemainingBombs = this.numberOfBombs;
            GenerateList();
        }

        private void ChangeMode()
        {
            CurrentMode = CurrentMode == ClickMode.Mark ? ClickMode.Reveal : ClickMode.Mark;
        }

        private void CheckGameWon()
        {
            var tiles = Tiles.SelectMany(s => s);
            var bombs = tiles.Where(s => s.IsBomb);
            var unveiledTiles = tiles.Where(s => !s.IsClickable);
            var markedTiles = tiles.Where(s => s.IsMarked);
            var veiledTiles = tiles.Where(s => s.IsClickable);

            var won = markedTiles.Union(veiledTiles).All(s => bombs.Contains(s)) && bombs.All(s => markedTiles.Union(veiledTiles).Contains(s));
            if (won)
            {
                GameWon = true;
                Time.Stop();
                var dialog = new GameWonDialog
                {
                    DataContext = new GameWonDialogViewModel()
                    {
                        WinningTime = $"{Time.Elapsed.Hours:D2}:{Time.Elapsed.Minutes:D2}:{Time.Elapsed.Seconds:D2}.{Time.Elapsed.Milliseconds / 10:D2}",
                    },
                    Owner = Application.Current.MainWindow,
                };
                dialog.ShowDialog();
                if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                {
                    var newHighscore = mapper.Map<Highscore>(new HighscoreViewModel
                    {
                        Difficulty = difficulty,
                        Time = Time.Elapsed.Duration(),
                    });
                    using var db = minesweeperDbContextFactory.CreateDbContext();
                    db.HighScores.Add(newHighscore);
                    db.SaveChanges();
                }
            }
        }

        private void ClickNotBombs()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var surroundingTiles = new List<TileViewModel>();
                    // Get all tiles in 3x3 around the tile.
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            if (i + m >= 0 && i + m < columns && j + n >= 0 && j + n < rows && !(m == 0 && n == 0) && Tiles[i + m][j + n].IsClickable)
                            {
                                surroundingTiles.Add(Tiles[i + m][j + n]);
                            }
                        }
                    }

                    // Check if number of tiles this one is connected to is equal to the
                    // number of surrounding bombs.
                    if (!Tiles[i][j].IsClickable && surroundingTiles.Count > 0 && Tiles[i][j].SurroundingBombs == surroundingTiles.Where(s => s.IsMarked).Count())
                    {
                        foreach (var tile in surroundingTiles.Where(s => !s.IsMarked))
                        {
                            TileClicked(tile);
                        }
                    }
                }
            }
        }

        private void CountSurroundingBombs()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (Tiles[i][j].IsBomb)
                    {
                        for (int m = -1; m < 2; m++)
                        {
                            for (int n = -1; n < 2; n++)
                            {
                                if (i + m >= 0 && i + m < columns && j + n >= 0 && j + n < rows)
                                {
                                    Tiles[i + m][j + n].SurroundingBombs++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FirstClick()
        {
            Time = new Stopwatch();
            Time.Start();
            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 5), DispatcherPriority.Background, new EventHandler(UpdateTime),
            Application.Current.Dispatcher);
            isFirstClick = !isFirstClick;
        }

        private void GenerateList()
        {
            var random = new Random();
            var tilesList = new List<TileViewModel>();
            for (int i = 0; i < rows * columns; i++)
            {
                tilesList.Add(new TileViewModel
                {
                    IsBomb = false,
                    IsMarked = false,
                    SurroundingBombs = 0,
                    IsClickable = true,
                });
            }

            var bombTiles = tilesList.OrderBy(s => Guid.NewGuid()).Take(numberOfBombs);
            foreach (var tile in bombTiles)
            {
                tile.IsBomb = true;
            }
            for (int i = 0; i < tilesList.Count; i++)
            {
                tilesList[i].ID = i;
            }

            Tiles.Clear();
            var index = 0;
            for (int i = 0; i < columns; i++)
            {
                Tiles.Add(new ObservableCollection<TileViewModel>());
                for (int j = 0; j < rows; j++)
                {
                    Tiles[i].Add(tilesList[index]);
                    index++;
                }
            }
            CountSurroundingBombs();
            PrepareGame();
        }

        private void GoBack()
        {
            MainMenuViewModel.StartWindowViewModel.SelectedViewModel = MainMenuViewModel;
        }

        private void MarkBombs()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var surroundingTiles = new List<TileViewModel>();
                    // Get all tiles in 3x3 around the tile that are clickable.
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            if (i + m >= 0 && i + m < columns && j + n >= 0 && j + n < rows && !(m == 0 && n == 0) && Tiles[i + m][j + n].IsClickable)
                            {
                                surroundingTiles.Add(Tiles[i + m][j + n]);
                            }
                        }
                    }

                    // Check if number of tiles this one is connected to is equal to the
                    // number of surrounding bombs.
                    if (!Tiles[i][j].IsClickable && surroundingTiles.Count > 0 && Tiles[i][j].SurroundingBombs == surroundingTiles.Count)
                    {
                        foreach (var tile in surroundingTiles)
                        {
                            if (!tile.IsMarked)
                            {
                                tile.IsMarked = true;
                                RemainingBombs--;
                            }
                        }
                    }
                }
            }
        }

        private void Pathfinding(TileViewModel tile)
        {
            if (tile.SurroundingBombs != 0)
            {
                return;
            }
            for (int m = -1; m < 2; m++)
            {
                for (int n = -1; n < 2; n++)
                {
                    var row = tile.ID / rows;
                    var column = tile.ID % rows;
                    if (m + row >= 0 && m + row < columns && n + column >= 0 && n + column < rows)
                    {
                        var newTile = Tiles[m + row][n + column];
                        if (!newTile.IsBomb && newTile.IsClickable && !newTile.IsMarked)
                        {
                            newTile.IsClickable = false;
                            Pathfinding(newTile);
                        }
                    }
                }
            }
        }

        private void PrepareGame()
        {
            RemainingBombs = numberOfBombs;
            GameWon = null;
            CurrentMode = ClickMode.Reveal;
            Time = null;
            isFirstClick = true;
            if (time != null && time.IsRunning)
            {
                dispatcherTimer.Stop();
                time.Stop();
            }
        }

        private void TileClicked(TileViewModel clickedTile)
        {
            if (isFirstClick && CurrentMode == ClickMode.Reveal)
            {
                if (CurrentMode == ClickMode.Reveal)
                {
                    if (clickedTile.IsBomb)
                    {
                        // Swap bomb tile with non-bomb tile.
                        var nonBomb = Tiles.SelectMany(s => s).First(s => !s.IsBomb);
                        clickedTile.IsBomb = false;
                        nonBomb.IsBomb = true;

                        foreach (var item in Tiles.SelectMany(s => s))
                        {
                            item.SurroundingBombs = 0;
                        }
                        CountSurroundingBombs();
                    }
                }
                FirstClick();
                CheckGameWon();
            }
            if (GameWon == null)
            {
                if (CurrentMode == ClickMode.Reveal)
                {
                    if (!clickedTile.IsMarked)
                    {
                        if (clickedTile.IsBomb)
                        {
                            foreach (var tile in Tiles.SelectMany(s => s))
                            {
                                if (tile.IsBomb || (!tile.IsBomb && tile.IsMarked))
                                {
                                    tile.IsClickable = false;
                                    Time.Stop();
                                    GameWon = false;
                                }
                            }
                        }
                        else
                        {
                            clickedTile.IsClickable = false;
                            if (clickedTile.SurroundingBombs == 0)
                            {
                                Pathfinding(clickedTile);
                            }
                        }
                        CheckGameWon();
                    }
                }
                else if (CurrentMode == ClickMode.Mark)
                {
                    RemainingBombs = clickedTile.IsMarked ? RemainingBombs + 1 : RemainingBombs - 1;
                    clickedTile.IsMarked = !clickedTile.IsMarked;
                }
            }
            NotifyPropertyChanged(nameof(Tiles));
        }

        private void TileRightClicked(TileViewModel tile)
        {
            if (isFirstClick)
            {
                FirstClick();
            }
            if (GameWon != true)
            {
                RemainingBombs = tile.IsMarked ? RemainingBombs + 1 : RemainingBombs - 1;
                tile.IsMarked = !tile.IsMarked;
                NotifyPropertyChanged(nameof(Tiles));
            }
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(Time));
        }
    }
}