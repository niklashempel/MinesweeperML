using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Commands;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.Enumerations;
using MinesweeperML.Models;

namespace MinesweeperML.ViewsModel
{
    /// <summary>
    /// Highscores view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class HighscoresViewModel : BaseViewModel
    {
        private readonly int entriesPerPage = 10;
        private readonly IMapper mapper;
        private readonly MinesweeperDbContextFactory minesweeperDbContextFactory;
        private int currentPage = 1;
        private RelayCommand goBackCommand;
        private int maxPage;
        private RelayCommand<DataGridSortingEventArgs> sortingCommand;

        /// <summary>
        /// The sorting direction.
        /// </summary>
        private ListSortDirection? SortingDirection = ListSortDirection.Ascending;

        private Expression<Func<Highscore, object>> SortingOrder = score => score.Time;

        /// <summary>
        /// Gets the difficulties.
        /// </summary>
        /// <value>The difficulties.</value>
        public static IEnumerable<Difficulty> Difficulties => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (value != currentPage)
                {
                    currentPage = value;
                    NotifyPropertyChanged(nameof(CurrentPage));
                    ApplyPaging();
                }
            }
        }

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        /// <value>The go back command.</value>
        public RelayCommand GoBackCommand => goBackCommand ??= new RelayCommand(param => this.GoBack());

        /// <summary>
        /// Gets the highscores view.
        /// </summary>
        /// <value>The highscores view.</value>
        public ICollectionView HighscoresView => HighscoresViewSource.View;

        /// <summary>
        /// Gets or sets the highscores view source.
        /// </summary>
        /// <value>The highscores view source.</value>
        public CollectionViewSource HighscoresViewSource { get; set; }

        /// <summary>
        /// Gets or sets the main window view model.
        /// </summary>
        /// <value>The main window view model.</value>
        public MainMenuViewModel MainWindowViewModel { get; set; }

        /// <summary>
        /// Gets or sets the maximum page.
        /// </summary>
        /// <value>The maximum page.</value>
        public int MaxPage
        {
            get
            {
                return maxPage;
            }
            set
            {
                if (value != maxPage)
                {
                    maxPage = value;
                    NotifyPropertyChanged(nameof(MaxPage));
                }
            }
        }

        /// <summary>
        /// Gets the skip to first page command.
        /// </summary>
        /// <value>The skip to first page command.</value>
        public RelayCommand SkipToFirstPageCommand { get; private set; }

        /// <summary>
        /// Gets the skip to last page command.
        /// </summary>
        /// <value>The skip to last page command.</value>
        public RelayCommand SkipToLastPageCommand { get; private set; }

        /// <summary>
        /// Gets the skip to next page command.
        /// </summary>
        /// <value>The skip to next page command.</value>
        public RelayCommand SkipToNextPageCommand { get; private set; }

        /// <summary>
        /// Gets the skip to previous page command.
        /// </summary>
        /// <value>The skip to previous page command.</value>
        public RelayCommand SkipToPreviousPageCommand { get; private set; }

        /// <summary>
        /// Gets the sorting command.
        /// </summary>
        /// <value>The sorting command.</value>
        public RelayCommand<DataGridSortingEventArgs> SortingCommand => sortingCommand ??= new RelayCommand<DataGridSortingEventArgs>(param => this.Sorting(param));

        /// <summary>
        /// Initializes a new instance of the <see cref="HighscoresViewModel" /> class.
        /// </summary>
        /// <param name="minesweeperDbContextFactory">
        /// The minesweeper database context factory.
        /// </param>
        /// <param name="mapper">The mapper.</param>
        public HighscoresViewModel(MinesweeperDbContextFactory minesweeperDbContextFactory, IMapper mapper)
        {
            SkipToFirstPageCommand = new RelayCommand(param => this.SkipToFirstPage());
            SkipToPreviousPageCommand = new RelayCommand(param => this.SkipToPreviousPage());
            SkipToNextPageCommand = new RelayCommand(param => this.SkipToNextPage());
            SkipToLastPageCommand = new RelayCommand(param => this.SkipToLastPage());

            this.minesweeperDbContextFactory = minesweeperDbContextFactory;
            this.mapper = mapper;
            HighscoresViewSource = new CollectionViewSource();
        }

        private void ApplyPaging()
        {
            using (var db = minesweeperDbContextFactory.CreateDbContext())
            {
                // Get entitites.
                IQueryable<Highscore> query = db.HighScores;

                // Sorting
                if (SortingOrder != null)
                {
                    if (SortingDirection == ListSortDirection.Ascending)
                    {
                        query = query.OrderBy(SortingOrder);
                    }
                    else if (SortingDirection == ListSortDirection.Descending)
                    {
                        query = query.OrderByDescending(SortingOrder);
                    }
                }

                // Paging
                MaxPage = (int)Math.Ceiling(Convert.ToDouble(query.Count()) / entriesPerPage);
                if (MaxPage < 1)
                {
                    MaxPage = 1;
                    ApplyPaging();
                }
                if (CurrentPage > MaxPage)
                {
                    CurrentPage = MaxPage;
                    ApplyPaging();
                }

                // Map results to view model.

                var highscores = query.Skip(entriesPerPage * (CurrentPage - 1)).Take(entriesPerPage)
                    .Select(s => mapper.Map<HighscoreViewModel>(s)).ToList();

                HighscoresViewSource.Source = highscores;
                HighscoresView.Refresh();
            }
            NotifyPropertyChanged(nameof(HighscoresView));
        }

        private void GoBack()
        {
            MainWindowViewModel.StartWindowViewModel.SelectedViewModel = MainWindowViewModel;
        }

        private void SkipToFirstPage()
        {
            CurrentPage = 1;
            ApplyPaging();
        }

        /// <summary>
        /// Skips to last page.
        /// </summary>
        private void SkipToLastPage()
        {
            var numberOfElements = 0;
            using (var db = minesweeperDbContextFactory.CreateDbContext())
            {
                numberOfElements = db.HighScores.Count();
            }
            var maxPage = Math.Ceiling(Convert.ToDouble(numberOfElements) / entriesPerPage);
            if (maxPage == 0)
            {
                maxPage = 1;
            }
            CurrentPage = (int)maxPage;
            ApplyPaging();
        }

        /// <summary>
        /// Skips to next page.
        /// </summary>
        private void SkipToNextPage()
        {
            var numberOfElements = 0;
            using (var db = minesweeperDbContextFactory.CreateDbContext())
            {
                numberOfElements = db.HighScores.Count();
            }
            if (CurrentPage != Math.Ceiling(Convert.ToDouble(numberOfElements) / entriesPerPage))
            {
                CurrentPage++;
                ApplyPaging();
            }
        }

        private void SkipToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                ApplyPaging();
            }
        }

        private void Sorting(DataGridSortingEventArgs e)
        {
            if (e.Column.SortDirection == null)
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
            }
            else if (e.Column.SortDirection == ListSortDirection.Ascending)
            {
                e.Column.SortDirection = ListSortDirection.Descending;
            }
            else
            {
                e.Column.SortDirection = null;
            }
            SortingDirection = e.Column.SortDirection;
            SortingOrder = e.Column.SortMemberPath switch
            {
                nameof(Highscore.Difficulty) => score => score.Difficulty,
                nameof(Highscore.Time) => score => score.Time,
                _ => throw new NotImplementedException($"{e.Column.SortMemberPath} is not implemented."),
            };
            ApplyPaging();
            e.Column.SortDirection = SortingDirection;
            e.Handled = true;
        }
    }
}