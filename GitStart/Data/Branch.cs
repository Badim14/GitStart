namespace GitStart.Data
{
    public class Branch
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RepositoryID { get; set; } // Связь с репозиторием
        public Repository Repository { get; set; } // Навигационное свойство

        public List<Commit> Commits { get; set; } = new List<Commit>();
    }
}