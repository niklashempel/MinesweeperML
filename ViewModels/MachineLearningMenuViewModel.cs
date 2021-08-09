using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using MinesweeperML.Business.Commands;
using MinesweeperML.Enumerations;
using MinesweeperML.Models;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// The view model representing the machine learning menu.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class MachineLearningMenuViewModel : BaseViewModel
    {
        private readonly CsvConfiguration csvAppendConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Do not wirte the header again.
            HasHeaderRecord = false,
        };

        private readonly IMapper mapper;
        private GameViewModel gameViewModel;
        private int generatedEntries;
        private AsyncCommand generateTrainingDataAsyncCommand;
        private RelayCommand goBackCommand;
        private bool isDataGenerating;
        private MainMenuViewModel mainMenuViewModel;
        private int numberOfTargetEntries = 100;
        private RelayCommand setTrainingDataFilePathCommand;
        private StartWindowViewModel startWindowViewModel;
        private string trainingDataFilePath = string.Empty;

        /// <summary>
        /// Gets or sets the generated entries.
        /// </summary>
        /// <value>The generated entries.</value>
        public int GeneratedEntries
        {
            get
            {
                return generatedEntries;
            }
            set
            {
                if (value != generatedEntries)
                {
                    generatedEntries = value;
                    NotifyPropertyChanged(nameof(GeneratedEntries));
                }
            }
        }

        /// <summary>
        /// Gets the generate training data asynchronous command.
        /// </summary>
        /// <value>The generate training data asynchronous command.</value>
        public AsyncCommand GenerateTrainingDataAsyncCommand
        {
            get
            {
                return generateTrainingDataAsyncCommand ??= new AsyncCommand(this.GenerateTrainingDataAsync, CanExecuteGenerateTrainingDataAsync);
            }
        }

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        /// <value>The go back command.</value>
        public RelayCommand GoBackCommand
        {
            get
            {
                return goBackCommand ??= new RelayCommand(param => this.GoBack());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data generating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is data generating; otherwise, <c>false</c>.
        /// </value>
        public bool IsDataGenerating
        {
            get
            {
                return isDataGenerating;
            }
            set
            {
                if (value != isDataGenerating)
                {
                    isDataGenerating = value;
                    NotifyPropertyChanged(nameof(IsDataGenerating));
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of target entries.
        /// </summary>
        /// <value>The number of target entries.</value>
        public int NumberOfTargetEntries
        {
            get
            {
                return numberOfTargetEntries;
            }
            set
            {
                if (value != numberOfTargetEntries)
                {
                    numberOfTargetEntries = value;
                    NotifyPropertyChanged(nameof(NumberOfTargetEntries));
                    GenerateTrainingDataAsyncCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Gets the set training data file path command.
        /// </summary>
        /// <value>The set training data file path command.</value>
        public RelayCommand SetTrainingDataFilePathCommand
        {
            get
            {
                return setTrainingDataFilePathCommand ??= new RelayCommand(param => this.SetTrainingDataFilePath());
            }
        }

        /// <summary>
        /// Gets or sets the training data file path.
        /// </summary>
        /// <value>The training data file path.</value>
        public string TrainingDataFilePath
        {
            get
            {
                return trainingDataFilePath;
            }
            set
            {
                if (value != trainingDataFilePath)
                {
                    trainingDataFilePath = value;
                    NotifyPropertyChanged(nameof(TrainingDataFilePath));
                    GenerateTrainingDataAsyncCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineLearningMenuViewModel" />
        /// class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public MachineLearningMenuViewModel(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Sets the main menu view model.
        /// </summary>
        /// <param name="mainMenuViewModel">The main menu view model.</param>
        public void SetMainMenuViewModel(MainMenuViewModel mainMenuViewModel)
        {
            this.mainMenuViewModel = mainMenuViewModel;
        }

        /// <summary>
        /// Sets the start window view model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="gameViewModel">The game view model.</param>
        public void SetStartWindowViewModel(StartWindowViewModel value, GameViewModel gameViewModel)
        {
            this.startWindowViewModel = value;
            this.gameViewModel = gameViewModel;
        }

        private async Task AppendTrainingDataToFileAsync(IEnumerable<TrainingDataViewModel> records)
        {
            using (var stream = File.Open(trainingDataFilePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, csvAppendConfig))
            {
                var trainingData = mapper.Map<IEnumerable<TrainingData>>(records);
                await csv.WriteRecordsAsync(trainingData);
            }
        }

        private bool CanExecuteGenerateTrainingDataAsync()
        {
            return NumberOfTargetEntries > 0 && !string.IsNullOrEmpty(TrainingDataFilePath);
        }

        /// <summary>
        /// Generates the training data asynchronous.
        /// </summary>
        private async Task GenerateTrainingDataAsync()
        {
            var counter = numberOfTargetEntries;
            var random = new Random();

            await WriteFileHeaderAsync();

            while (counter > 0)
            {
                gameViewModel.StartGame(15, 10, 30, Difficulty.Medium);

                // Skip the first two clicks to prevent too much data without much value.
                var clickableTiles = gameViewModel.Tiles.SelectMany(s => s);
                gameViewModel.ClickTile(clickableTiles.ToList()[random.Next(0, clickableTiles.Count())], false);

                // Only happens if there are n tiles and n-1 bombs.
                if (gameViewModel.GameWon is not null)
                {
                    continue;
                }
                else
                {
                    gameViewModel.ClickTile(clickableTiles.ToList()[random.Next(0, clickableTiles.Count())], false);
                }

                while (gameViewModel.GameWon is null)
                {
                    var trainingData = new List<TrainingDataViewModel>();
                    if (!gameViewModel.ClickNotBombs(ref counter, out trainingData))
                    {
                        trainingData.Add(gameViewModel.ClickTile(clickableTiles.ToList()[random.Next(0, clickableTiles.Count())], true));
                        counter--;
                    }
                    await AppendTrainingDataToFileAsync(trainingData);
                    if (counter <= 0)
                    {
                        break;
                    }
                }
            }
        }

        private void GoBack()
        {
            this.startWindowViewModel.SelectedViewModel = mainMenuViewModel;
        }

        private void SetTrainingDataFilePath()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text Documents|*.csv;*.txt",
            };
            if (dialog.ShowDialog() is bool result && result)
            {
                TrainingDataFilePath = dialog.FileName;
            }
        }

        private async Task WriteFileHeaderAsync()
        {
            // Write csv header.
            using var writer = new StreamWriter(trainingDataFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<TrainingData>();
            await csv.NextRecordAsync();
        }
    }
}