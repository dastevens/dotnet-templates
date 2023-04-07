namespace dastevens.Console.Verbs
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using dastevens.Console;

    internal class HelloWorld : IVerb
    {
        private readonly Argument<string> greetingArgument = new Argument<string>(
            name: "greeting",
            getDefaultValue: () => "Hello",
            description: "The greeting.");

        private readonly Argument<string> nameArgument = new Argument<string>(
            name: "name",
            getDefaultValue: () => "World",
            description: "The name to be greeted.");

        public string CommandName { get; } = nameof(HelloWorld);

        public string CommandDescription { get; } = "Print Hello World";

        public Argument[] Arguments => new[]
        {
            this.greetingArgument,
            this.nameArgument,
        };

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var cancellationToken = context.GetCancellationToken();

            var greeting = context.ParseResult.GetValueForArgument(this.greetingArgument);
            var name = context.ParseResult.GetValueForArgument(this.nameArgument);

            context.Console.WriteJson($"{greeting} {name}");

            await Task.Delay(1, cancellationToken);

            return 0;
        }
    }
}