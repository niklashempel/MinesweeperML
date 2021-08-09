using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MinesweeperML.Enumerations;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// The game view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class GameViewModel : BaseViewModel
    {
        private int columns;
        private ClickMode currentMode = ClickMode.Reveal;
        private Difficulty difficulty = Difficulty.Custom;
        private DispatcherTimer dispatcherTimer;
        private bool? gameWon;
        private bool isFirstClick = true;
        private int numberOfBombs;
        private int remaningBombs;
        private int rows;
        private ObservableCollection<ObservableCollection<TileViewModel>> tiles = new ObservableCollection<ObservableCollection<TileViewModel>>();
        private Stopwatch time;

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
        /// Clicks the not bombs.
        /// </summary>
        /// <param name="maxEntries">The maximum entries.</param>
        /// <param name="trainingDataViewModels">The training data view models.</param>
        /// <returns>True if a save tile was found. Else false.</returns>
        public bool ClickNotBombs(ref int maxEntries, out List<TrainingDataViewModel> trainingDataViewModels)
        {
            trainingDataViewModels = new List<TrainingDataViewModel>();
            var goodTileFound = false;
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
                            if (maxEntries > 1)
                            {
                                trainingDataViewModels.Add(ClickTile(tile, true));
                                maxEntries--;
                                goodTileFound = true;
                            }
                            else
                            {
                                // Last entry.
                                return goodTileFound;
                            }
                        }
                    }
                }
            }
            return goodTileFound;
        }

        /// <summary>
        /// Clicks the tile.
        /// </summary>
        /// <param name="clickedTile">The clicked tile.</param>
        /// <param name="doTraining">if set to <c>true</c> [do training].</param>
        public TrainingDataViewModel ClickTile(TileViewModel clickedTile, bool doTraining = false)
        {
            TrainingDataViewModel trainingDataViewModel = new TrainingDataViewModel();
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
                    if (doTraining)
                    {
                        trainingDataViewModel = GenerateTrainingData(clickedTile);
                    }
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
            return trainingDataViewModel;
        }

        /// <summary>
        /// Rights the click tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        public void RightClickTile(TileViewModel tile)
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

        /// <summary>
        /// Switches the click mode.
        /// </summary>
        public void SwitchClickMode()
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

        private TrainingDataViewModel GenerateTrainingData(TileViewModel tile)
        {
            // Always mark bombs to be more effective.
            MarkBombs();

            var surroundingTiles = new List<Feature>();

            for (int m = -2; m < 3; m++)
            {
                for (int n = -2; n < 3; n++)
                {
                    var row = tile.ID / rows;
                    var column = tile.ID % rows;
                    if (m + row < 0 || n + column < 0 || m + row >= columns || n + column >= rows)
                    {
                        surroundingTiles.Add(Feature.OutOfField);
                    }
                    else if (!(n == 0 && m == 0))
                    {
                        if (Tiles[m + row][n + column].IsClickable)
                        {
                            if (Tiles[m + row][n + column].IsMarked)
                            {
                                surroundingTiles.Add(Feature.Marked);
                            }
                            else
                            {
                                surroundingTiles.Add(Feature.Unknown);
                            }
                        }
                        else
                        {
                            if (Tiles[m + row][n + column].SurroundingBombs is int bombs)
                            {
                                surroundingTiles.Add((Feature)bombs);
                            }
                        }
                    }
                }
            }
            var features = surroundingTiles.ToArray();
            var newData = new TrainingDataViewModel
            {
                Label = tile.IsBomb,
                X00 = features[0],
                X01 = features[1],
                X02 = features[2],
                X03 = features[3],
                X04 = features[4],
                X10 = features[5],
                X11 = features[6],
                X12 = features[7],
                X13 = features[8],
                X14 = features[9],
                X20 = features[10],
                X21 = features[11],
                X23 = features[12],
                X24 = features[13],
                X30 = features[14],
                X31 = features[15],
                X32 = features[16],
                X33 = features[17],
                X34 = features[18],
                X40 = features[19],
                X41 = features[20],
                X42 = features[21],
                X43 = features[22],
                X44 = features[23],
            };
            return newData;
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

        private void UpdateTime(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(Time));
        }
    }
}