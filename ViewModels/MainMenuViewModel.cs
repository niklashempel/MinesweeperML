﻿using System;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Commands;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.Enumerations;
using MinesweeperML.ViewModels;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// Main menu view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly CustomGameViewModel customGameViewModel;
        private readonly GameViewModel gameViewModel;
        private readonly HighscoresViewModel highscoresViewModel;
        private readonly MachineLearningMenuViewModel machineLearningMenuViewModel;
        private readonly MinesweeperViewModel minesweeperViewModel;
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private readonly SelectNewGameViewModel selectNewGameViewModel;
        private bool isDarkmodeEnabled;
        private RelayCommand showHighscoresCommand;
        private RelayCommand showMachineLearningMenuCommand;
        private RelayCommand startNewGameCommand;
        private StartWindowViewModel startWindowViewModel;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is darkmode enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is darkmode enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDarkmodeEnabled
        {
            get => isDarkmodeEnabled;
            set
            {
                if (value != isDarkmodeEnabled)
                {
                    isDarkmodeEnabled = value;
                    NotifyPropertyChanged(nameof(isDarkmodeEnabled));
                    ToggleDarkmode(isDarkmodeEnabled);
                }
            }
        }

        /// <summary>
        /// Gets the show highscores command.
        /// </summary>
        /// <value>The show highscores command.</value>
        public RelayCommand ShowHighscoresCommand
        {
            get
            {
                return showHighscoresCommand ??= new RelayCommand(param => this.ShowHighscores());
            }
        }

        /// <summary>
        /// Gets the show machine learning menu command.
        /// </summary>
        /// <value>The show machine learning menu command.</value>
        public RelayCommand ShowMachineLearningMenuCommand
        {
            get
            {
                return showMachineLearningMenuCommand ??= new RelayCommand(param => this.ShowMachineLearningMenu());
            }
        }

        /// <summary>
        /// Gets the start customer game command.
        /// </summary>
        /// <value>The start customer game command.</value>
        public RelayCommand StartCustomGameCommand { get; private set; }

        /// <summary>
        /// Gets the start easy game command.
        /// </summary>
        /// <value>The start easy game command.</value>
        public RelayCommand StartEasyGameCommand { get; private set; }

        /// <summary>
        /// Gets the start hard game command.
        /// </summary>
        /// <value>The start hard game command.</value>
        public RelayCommand StartHardGameCommand { get; private set; }

        /// <summary>
        /// Gets the start medium game command.
        /// </summary>
        /// <value>The start medium game command.</value>
        public RelayCommand StartMediumGameCommand { get; private set; }

        /// <summary>
        /// Gets the start new game command.
        /// </summary>
        /// <value>The start new game command.</value>
        public RelayCommand StartNewGameCommand
        {
            get
            {
                return startNewGameCommand ??= new RelayCommand(param => this.StartNewGame());
            }
        }

        /// <summary>
        /// Gets or sets the start window view model.
        /// </summary>
        /// <returns>The start window view model.</returns>
        public StartWindowViewModel StartWindowViewModel
        {
            get
            {
                return startWindowViewModel;
            }
            set
            {
                if (startWindowViewModel != value)
                {
                    startWindowViewModel = value;
                    this.machineLearningMenuViewModel.SetStartWindowViewModel(value, this.gameViewModel);
                    this.selectNewGameViewModel.SetStartWindowViewModel(StartWindowViewModel);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuViewModel" /> class.
        /// </summary>
        /// <param name="highscoresViewModel">The highscores view model.</param>
        /// <param name="selectNewGameViewModel">The select new game view model.</param>
        /// <param name="machineLearningMenuViewModel">
        /// The machine learning menu view model.
        /// </param>
        /// <param name="gameViewModel">The game view model.</param>
        public MainMenuViewModel(HighscoresViewModel highscoresViewModel, SelectNewGameViewModel selectNewGameViewModel, MachineLearningMenuViewModel machineLearningMenuViewModel, GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
            this.highscoresViewModel = highscoresViewModel;
            this.highscoresViewModel.SetMainMenuViewModel(this);
            this.selectNewGameViewModel = selectNewGameViewModel;
            this.selectNewGameViewModel.SetMainMenuViewModel(this);
            this.machineLearningMenuViewModel = machineLearningMenuViewModel;
            this.machineLearningMenuViewModel.SetMainMenuViewModel(this);
        }

        private void ShowHighscores()
        {
            startWindowViewModel.SelectedViewModel = this.highscoresViewModel;
            this.highscoresViewModel.SkipToFirstPageCommand.Execute(null);
        }

        private void ShowMachineLearningMenu()
        {
            startWindowViewModel.SelectedViewModel = this.machineLearningMenuViewModel;
        }

        private void StartNewGame()
        {
            startWindowViewModel.SelectedViewModel = this.selectNewGameViewModel;
        }

        private void ToggleDarkmode(bool isDarkmodeEnabled)
        {
            var theme = paletteHelper.GetTheme();
            var baseTheme = isDarkmodeEnabled ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            paletteHelper.SetTheme(theme);
        }
    }
}