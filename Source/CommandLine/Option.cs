namespace UnityDissector.CommandLine
{
    public class Option
    {
        public delegate void OptionHandler(CommandLineHandler handler, string file);

        public Option(string name, OptionHandler handler)
        {
            Name = name;
            Alias = new string[0];
            Handler = handler;
        }

        public Option(string name, OptionHandler handler, params string[] alias)
        {
            Name = name;
            Alias = alias;
            Handler = handler;
        }

        public Option(string name, string description, OptionHandler handler, params string[] alias)
        {
            Name = name;
            Description = description;
            Alias = alias;
            Handler = handler;
        }

        public string Name { get; set; }
        public string[] Alias { get; set; }
        public string Description { get; set; }

        private OptionHandler Handler { get; set; }

        public void Handle(CommandLineHandler handler, string file)
        {
            Handler(handler, file);
        }
    }
}
