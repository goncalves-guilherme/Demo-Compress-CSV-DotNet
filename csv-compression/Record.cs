namespace csv_compression
{
    public class Record
    {
        public string Property1 { get; }
        public string Property2 { get; }
        public string Property3 { get; }
        public string Property4 { get; }
        public string Property5 { get; }

        public Record() 
        {
            this.Property1 = Guid.NewGuid().ToString();
            this.Property2 = Guid.NewGuid().ToString();
            this.Property3 = Guid.NewGuid().ToString();
            this.Property4 = Guid.NewGuid().ToString();
            this.Property5 = Guid.NewGuid().ToString();
        }
    }
}
