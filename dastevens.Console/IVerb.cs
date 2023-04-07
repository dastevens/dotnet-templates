namespace dastevens.Console
{
    using System.CommandLine;
    using System.CommandLine.Invocation;

    internal interface IVerb
    {
        string CommandName { get; }

        string CommandDescription { get; }

        Argument[] Arguments { get; }

        Task<int> InvokeAsync(InvocationContext context);
    }
}