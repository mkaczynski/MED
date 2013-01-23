namespace DataGenerator
{
    public class CommandLineParser
    {
        private readonly string[] argv;

        public CommandLineParser(string[] argv)
        {
            this.argv = argv;
        }

        public string InFileName { get; private  set; }

        public int LinesCount { get; private set; }

        public string OutFileName { get; private set; }

        public bool ValidArguments { get; set; }

        private bool Validate()
        {
            int n;
            if(argv.Length < 3 || !int.TryParse(argv[1], out n))
            {
                return false;
            }
            ValidArguments = true;
            return true;
        }

        public void Parse()
        {
            if(!Validate())
                return;
            InFileName = argv[0];
            LinesCount = int.Parse(argv[1]);
            OutFileName = argv[2];
        }
    }
}