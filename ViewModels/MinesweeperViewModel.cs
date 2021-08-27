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

namespace MinesweeperML.ViewModels
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
        private Difficulty difficulty;
        private GameViewModel gameViewModel;
        private RelayCommand goBackCommand;
        private int numberOfBombs;
        private int rows;
        private RelayCommand startNewGameCommand;
        private RelayCommand<TileViewModel> tileClickedCommand;
        private RelayCommand<TileViewModel> tileRightClickedCommand;

        /// <summary>
        /// Gets the change mode command.
        /// </summary>
        /// <value>The change mode command.</value>
        public RelayCommand ChangeModeCommand => changeModeCommand ??= new RelayCommand(param => this.ChangeMode());

        /// <summary>
        /// Gets or sets the game view model.
        /// </summary>
        /// <value>The game view model.</value>
        public GameViewModel GameViewModel
        {
            get
            {
                return gameViewModel;
            }
            set
            {
                if (value != gameViewModel)
                {
                    gameViewModel = value;
                    NotifyPropertyChanged(nameof(GameViewModel));
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
        /// Gets the start new game command.
        /// </summary>
        /// <value>The start new game command.</value>
        public RelayCommand StartNewGameCommand => startNewGameCommand ??= new RelayCommand(param => this.StartGame());

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
        /// Initializes a new instance of the <see cref="MinesweeperViewModel" /> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="minesweeperDbContextFactory">
        /// The minesweeper database context factory.
        /// </param>
        /// <param name="gameViewModel">The game view model.</param>
        public MinesweeperViewModel(IMapper mapper, MinesweeperDbContextFactory minesweeperDbContextFactory, GameViewModel gameViewModel)
        {
            this.mapper = mapper;
            this.minesweeperDbContextFactory = minesweeperDbContextFactory;
            GameViewModel = gameViewModel;
        }

        /// <summary>
        /// Sets the game settings.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="numberOfBombs">The number of bombs.</param>
        /// <param name="difficulty">The difficulty.</param>
        public void SetGameSettings(int columns, int rows, int numberOfBombs, Difficulty difficulty)
        {
            this.numberOfBombs = numberOfBombs;
            this.columns = columns;
            this.rows = rows;
            this.difficulty = difficulty;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            gameViewModel.StartGame(this.columns, this.rows, this.numberOfBombs, this.difficulty);
        }

        private void ChangeMode()
        {
            gameViewModel.SwitchClickMode();
        }

        private void GoBack()
        {
            MainMenuViewModel.StartWindowViewModel.SelectedViewModel = MainMenuViewModel;
        }

        private void TileClicked(TileViewModel param)
        {
            gameViewModel.ClickTile(param);
            if (gameViewModel.GameWon is true)
            {
                var dialog = new GameWonDialog
                {
                    DataContext = new GameWonDialogViewModel()
                    {
                        WinningTime = $"{gameViewModel.Time.Elapsed:g}",
                    },
                    Owner = Application.Current.MainWindow,
                };
                dialog.ShowDialog();
                if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                {
                    var newHighscore = mapper.Map<Highscore>(new HighscoreViewModel
                    {
                        Difficulty = difficulty,
                        Time = gameViewModel.Time.Elapsed.Duration(),
                    });
                    using var db = minesweeperDbContextFactory.CreateDbContext();
                    db.HighScores.Add(newHighscore);
                    db.SaveChanges();
                }
            }
        }

        private void TileRightClicked(TileViewModel param)
        {
            gameViewModel.RightClickTile(param);
        }
    }
}