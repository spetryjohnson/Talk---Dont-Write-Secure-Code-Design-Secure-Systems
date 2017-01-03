using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace SecurityTests {

	[TestClass]
	public class EndpointAuditTests {

		[TestMethod]
		[UseReporter(typeof(DiffReporter))]
		public void Endpoint_analysis_file_matches_expectations() {
			var auditReport = SecurityAudit.Program.GenerateReport();
			
			Approvals.Verify(auditReport);
		}
	}
}
