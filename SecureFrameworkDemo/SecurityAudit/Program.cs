using System;
using SecureFrameworkDemo.Controllers;
using SecureFrameworkDemo.Framework.SecurityAudit;
using System.IO;
using System.Text;

namespace SecurityAudit {

	class Program {

		static void Main(string[] args) {

			var outputPath = GetOutputPathFromArgs(args);
			var report = new ControllerEndpointAudit(typeof(SecureFrameworkController).Assembly);

			WriteReportToDisk(report, outputPath);
		}

		private static void WriteReportToDisk(ControllerEndpointAudit report, string outputPath) {
			var sb = new StringBuilder();

			sb.AppendLine("Path,Requires Auth,Requires Perm");

			foreach (var endpoint in report.Endpoints) {
				sb.AppendLine($"{endpoint.RelativePath},{endpoint.RequiresAuthentication},{endpoint.RequiresPermission}");
			}

			File.WriteAllText(outputPath, sb.ToString());
		}

		private static string GetOutputPathFromArgs(string[] args) {
			var defaultDir = Environment.CurrentDirectory;
			var defaultFilename = $"output-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";

			// would have prefered to use CommandLineParser nuget package, but decided not to
			// take a dependency in the demo
			if (args.Length == 0) {
				return Path.Combine(defaultDir, defaultFilename);
			}
			else {
				// if a value was provided, but it was only a directory name, create a filename by default
				var outputFile = args[0];

				if (Directory.Exists(outputFile)) {
					outputFile = Path.Combine(outputFile, defaultFilename);
				}

				return outputFile;
			}
		}
	}
}
