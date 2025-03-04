namespace GitStart.Data
{
    public class Commit
    {
        public int ID { get; set; }
        public string Hash { get; set; } // Уникальный SHA-идентификатор коммита
        public string Message { get; set; } // Сообщение коммита
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public int BranchID { get; set; } // Связь с веткой
        public Branch Branch { get; set; } // Навигационное свойство
    }
}
