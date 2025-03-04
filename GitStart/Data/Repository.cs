namespace GitStart.Data
{
    public class Repository
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; } // Локальный путь к репозиторию
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public List<Branch> Branches { get; set; } = new List<Branch>();
    }

}
