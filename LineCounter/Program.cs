using System;
using System.Collections.Generic;
using CommandLine;
using Linecounter.Logger.Specific;
using Linecounter.Params;

namespace Linecounter
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => new FileReader(new FileLinesLogger()).RunOptions(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            //Here handle input params errors if you wonna
        }
    }
}