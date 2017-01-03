using System;
using System.IO;
using System.Text;
using SecureFrameworkDemo.Controllers;
using SecureFrameworkDemo.Framework.SecurityAudit;
using SecureFrameworkDemo.Framework;

namespace SecurityAudit {

	/// <summary>
	/// Performs an endpoint analysis for MVC controllers and writes the report to disk, so that it can be 
	/// examined in tests or by other automated processes.
	/// </summary>
	public class Program {

		static void Main(string[] args) {
			var outputPath = GetOutputPathFromArgs(args);

			WriteReportToDisk(outputPath);
		}

		public static string GenerateReport() {
			var report = new ControllerEndpointAudit(
				typeof(SecureFrameworkController).Assembly
			);

			var sb = new StringBuilder();
			foreach (var endpoint in report.Endpoints) {
				sb.AppendLine(
					endpoint.RelativePath + "\n"
					+ (endpoint.RequiresAuthentication ? "REQUIRES AUTH" : "PUBLIC") + " / "
					+ endpoint.RequiresPermission.ToStringNullSafe("[no perm required]") + "\n"
				);
			}

			return sb.ToString();
		}

		public static void WriteReportToDisk(string outputPath) {
			var fileContents = GenerateReport();
			File.WriteAllText(outputPath, fileContents);
		}

		private static string GetOutputPathFromArgs(string[] args) {
			var defaultDir = Environment.CurrentDirectory;
			var defaultFilename = $"EndpointAnalysis-{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";

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
