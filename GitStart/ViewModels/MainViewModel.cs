using System.Windows.Input;
using GitStart.Data;
using GitStart.Services;
using System.Windows;
using System.Windows.Input;
using GitStart.Views;
using Windows.UI.Core;

namespace GitStart.ViewModels
{
    public class MainViewModel : Utils.ViewModelBase
    {

        private bool _isWindowMaximized;
        public bool IsWindowMaximized
        {
            get => _isWindowMaximized;
            set
            {
                if (_isWindowMaximized != value)
                {
                    _isWindowMaximized = value;
                    OnPropertyChanged();
                    MaximizeRestoreIcon = _isWindowMaximized ? "Minimize2" : "Maximize2";
                }
            }
        }

        // Иконка для кнопки максимизации/восстановления окна
        private string _maximizeRestoreIcon;
        public string MaximizeRestoreIcon
        {
            get => _maximizeRestoreIcon;
            set
            {
                if (_maximizeRestoreIcon != value)
                {
                    _maximizeRestoreIcon = value;
                    OnPropertyChanged();
                }
            }
        }



        private readonly Window _mainWindow;
        private GitDbContext _context;
        private GitService _gitService;
        private Repository _selectedRepository;
        private Branch _selectedBranch;
        private Commit _selectedCommit;


        public List<Repository> Repositories { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Commit> Commits { get; set; }


        // Команды для управления окном
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeRestoreCommand { get; }
        public ICommand CloseCommand { get; }

        public Repository SelectedRepository
        {
            get => _selectedRepository;
            set
            {
                if (_selectedRepository != value)
                {
                    _selectedRepository = value;
                    OnPropertyChanged();
                    LoadBranchesAndCommits(); // Загружаем ветки и коммиты при смене репозитория
                }
            }
        }

        public Branch SelectedBranch
        {
            get => _selectedBranch;
            set
            {
                if (_selectedBranch != value)
                {
                    _selectedBranch = value;
                    OnPropertyChanged();
                    LoadCommits(); // Загружаем коммиты при смене ветки
                }
            }
        }

        public Commit SelectedCommit
        {
            get => _selectedCommit;
            set
            {
                if (_selectedCommit != value)
                {
                    _selectedCommit = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CloneRepositoryCommand { get; }
        public ICommand CreateBranchCommand { get; }
        public ICommand CommitCommand { get; }
        public ICommand CheckoutBranchCommand { get; }
        public ICommand PushCommand { get; }
        public ICommand PullCommand { get; }

        public MainViewModel(Window mainWindow)
        {
            _context = new GitDbContext();
            _gitService = new GitService();
            _mainWindow = mainWindow;

            // Загрузка репозиториев из базы данных
            Repositories = _context.Repositories.ToList();

            // Инициализация команд
            CloneRepositoryCommand = new RelayCommand(CloneRepository);
            CreateBranchCommand = new RelayCommand(CreateBranch);
            CommitCommand = new RelayCommand(CommitChanges);
            CheckoutBranchCommand = new RelayCommand(CheckoutBranch);
            PushCommand = new RelayCommand(PushChanges);
            PullCommand = new RelayCommand(PullChanges);
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            MaximizeRestoreCommand = new RelayCommand(ToggleMaximizeRestoreWindow);
            CloseCommand = new RelayCommand(CloseWindow);

            // Изначальное состояние окна
            IsWindowMaximized = _mainWindow.WindowState == WindowState.Maximized;
            MaximizeRestoreIcon = IsWindowMaximized ? "Minimize2" : "Maximize2";
        }
        // Метод для минимизации окна
        private void MinimizeWindow()
        {
            _mainWindow.WindowState = WindowState.Minimized;
        }

        // Метод для максимизации/восстановления окна
        private void ToggleMaximizeRestoreWindow()
        {
            if (_mainWindow.WindowState == WindowState.Maximized)
            {
                _mainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                _mainWindow.WindowState = WindowState.Maximized;
            }

            IsWindowMaximized = _mainWindow.WindowState == WindowState.Maximized;
        }


        // Метод для закрытия окна
        private void CloseWindow()
        {
            _mainWindow.Close();
        }
        private void LoadBranchesAndCommits()
        {
            if (SelectedRepository != null)
            {
                Branches = _context.Branches.Where(b => b.RepositoryID == SelectedRepository.ID).ToList();
                Commits = _context.Commits.Where(c => c.BranchID == SelectedBranch.ID).ToList();
            }
        }

        private void LoadCommits()
        {
            if (SelectedBranch != null)
            {
                Commits = _context.Commits.Where(c => c.BranchID == SelectedBranch.ID).ToList();
            }
        }

        private void CloneRepository()
        {
            // Логика клонирования репозитория
            _gitService.CloneRepository("url", "localPath");

            // Пример добавления репозитория в базу данных
            var newRepo = new Repository { Name = "New Repo", Path = "path_to_repo" };
            _context.Repositories.Add(newRepo);
            _context.SaveChanges();

            Repositories = _context.Repositories.ToList(); // Обновление списка репозиториев
            OnPropertyChanged(nameof(Repositories)); // Уведомление UI об изменении списка репозиториев
        }

        private void CreateBranch()
        {
            // Логика создания ветки
            if (SelectedRepository != null)
            {
                _gitService.CreateBranch(SelectedRepository.Path, "new-branch");

                var newBranch = new Branch { Name = "new-branch", RepositoryID = SelectedRepository.ID };
                _context.Branches.Add(newBranch);
                _context.SaveChanges();

                LoadBranchesAndCommits();
                OnPropertyChanged(nameof(Branches)); // Уведомление UI об изменении списка веток
            }
        }

        private void CommitChanges()
        {
            // Логика коммита
            if (SelectedRepository != null && SelectedBranch != null)
            {
                _gitService.CommitChanges(SelectedRepository.Path, "Commit message");

                var newCommit = new Commit
                {
                    Hash = "SHA256_HASH",
                    Message = "Commit message",
                    BranchID = SelectedBranch.ID,
                    Timestamp = DateTime.Now
                };

                _context.Commits.Add(newCommit);
                _context.SaveChanges();

                LoadCommits();
                OnPropertyChanged(nameof(Commits)); // Уведомление UI об изменении списка коммитов
            }
        }

        private void CheckoutBranch()
        {
            // Логика переключения ветки
            if (SelectedRepository != null && SelectedBranch != null)
            {
                _gitService.CheckoutBranch(SelectedRepository.Path, SelectedBranch.Name);
            }
        }

        private void PushChanges()
        {
            // Логика пуша изменений
            if (SelectedRepository != null)
            {
                _gitService.PushChanges(SelectedRepository.Path);
            }
        }

        private void PullChanges()
        {
            // Логика пулла изменений
            if (SelectedRepository != null)
            {
                _gitService.PullChanges(SelectedRepository.Path);
            }
        }
    }
}
