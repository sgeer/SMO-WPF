namespace SMO_PROG_WPF
{
    class Line
    {

        public int Id { get; set; }
        public string Time { get; set; }
        public string Clients { get; set; }
        public string Сashbox { get; set; }

        public Line()
        {
        }

        public Line(int id, string time, string clients, string cashbox)
        {
            Id = id;
            Сashbox = cashbox;
            Time = time;
            Clients = clients;
        }
    }
}
