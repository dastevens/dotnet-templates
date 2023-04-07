namespace dastevens.Console
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using dastevens.Console.Verbs;
    using Microsoft.Extensions.DependencyInjection;

    internal class Cli
    {
        private readonly IServiceProvider serviceProvider;

        public Cli(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task RunAsync(string[] args, CancellationToken cancellationToken)
        {
            var cmd = new RootCommand
            {
                new Command("sample", "Sample command")
                {
                    this.BuildCommand(this.serviceProvider.GetRequiredService<HelloWorld>()),
                },
            };

            await cmd.InvokeAsync(args);
        }

        private static string ToKebabCase(string str)
        {
            IEnumerable<char> ConvertChar(char c, int index)
            {
                if (char.IsUpper(c) && index != 0)
                {
                    yield return '-';
                }

                yield return char.ToLower(c);
            }

            return string.Concat(str.SelectMany(ConvertChar));
        }

        private Command BuildCommand<T>(T verb)
            where T : IVerb
        {
            var command = new Command(
                ToKebabCase(verb.CommandName),
                verb.CommandDescription);

            foreach (var argument in verb.Arguments)
            {
                command.AddArgument(argument);
            }

            command.Handler = new CommandHandler<T>(this.serviceProvider);
            return command;
        }

        private sealed class CommandHandler<T> : ICommandHandler
            where T : IVerb
        {
            private readonly IServiceProvider serviceProvider;

            public CommandHandler(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }

            public int Invoke(InvocationContext context) => this.InvokeAsync(context).Result;

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var verb = this.serviceProvider.GetService<T>();
                return await verb.InvokeAsync(context);
            }
        }
    }
}