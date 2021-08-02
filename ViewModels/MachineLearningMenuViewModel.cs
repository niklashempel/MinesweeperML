using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MinesweeperML.Business.Commands;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// The view model representing the machine learning menu.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class MachineLearningMenuViewModel : BaseViewModel
    {
        private int generatedEntries;
        private AsyncCommand generateTrainingDataAsyncCommand;
        private RelayCommand goBackCommand;
        private bool isDataGenerating;
        private MainMenuViewModel mainMenuViewModel;
        private int numberOfTargetEntries;
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
                return generateTrainingDataAsyncCommand ??= new AsyncCommand(this.GenerateTrainingDataAsync);
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
                }
            }
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
        public void SetStartWindowViewModel(StartWindowViewModel value)
        {
            this.startWindowViewModel = value;
        }

        private async Task GenerateTrainingDataAsync()
        {
            if (!string.IsNullOrEmpty(TrainingDataFilePath))
            {
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
    }
}