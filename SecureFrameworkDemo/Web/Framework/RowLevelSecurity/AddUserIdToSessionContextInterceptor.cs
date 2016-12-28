using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Web;

namespace SecureFrameworkDemo.Framework.RowLevelSecurity {

	/// <summary>
	/// See https://docs.microsoft.com/en-us/azure/app-service-web/web-sites-dotnet-entity-framework-row-level-security
	/// 
	/// This requires SQL Server 2016 to work. You can turn this off in web.config if you want to use SQL Express
	/// </summary>
	public class AddUserIdToSessionContextInterceptor : IDbConnectionInterceptor {

		public void Opened(DbConnection conn, DbConnectionInterceptionContext ctx) {
			// If no user is logged in, leave SESSION_CONTEXT null & return everything
			var isLoggedIn = (HttpContext.Current != null) && HttpContext.Current.User.Identity.IsAuthenticated;

			if (!isLoggedIn) {
				return;
			}

			// HACK FOR DEMO APP: Need to disable this for the "insecure" and "secure feature" areas of the site.
			// This demonstrates that secure frameworks are all about making it SECURE BY DEFAULT, and then requiring
			// extra effort to be INSECURE, which should be the non-common case. 
			var isSecureFrameworkArea = HttpContext.Current.Request.RawUrl.ContainsIgnoringCase("SecureFramework");
			if (!isSecureFrameworkArea) {
				return;
			}

			// Set SESSION_CONTEXT to current UserId whenever EF opens a connection
			// There are table-level predicates defined during the EF Migrations that look for
			// this value and, if present, implement whatever logic is necessary
			var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

			if (userId != null) {
				DbCommand cmd = conn.CreateCommand();
				cmd.CommandText = "EXEC sp_set_session_context @key=N'UserId', @value=@UserId";
				DbParameter param = cmd.CreateParameter();
				param.ParameterName = "@UserId";
				param.Value = userId;
				cmd.Parameters.Add(param);
				cmd.ExecuteNonQuery();
			}
		}

		public void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext) {
		}

		public void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext) {
		}

		public void BeginningTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext) {
		}

		public void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext) {
		}

		public void Closing(DbConnection connection, DbConnectionInterceptionContext interceptionContext) {
		}

		public void ConnectionStringGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void ConnectionStringGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void ConnectionStringSet(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext) {
		}

		public void ConnectionStringSetting(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext) {
		}

		public void ConnectionTimeoutGetting(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext) {
		}

		public void ConnectionTimeoutGot(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext) {
		}

		public void DataSourceGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void DataSourceGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void DatabaseGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void DatabaseGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void Disposed(DbConnection connection, DbConnectionInterceptionContext interceptionContext) {
		}

		public void Disposing(DbConnection connection, DbConnectionInterceptionContext interceptionContext) {
		}

		public void EnlistedTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext) {
		}

		public void EnlistingTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext) {
		}

		public void ServerVersionGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void ServerVersionGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) {
		}

		public void StateGetting(DbConnection connection, DbConnectionInterceptionContext<System.Data.ConnectionState> interceptionContext) {
		}

		public void StateGot(DbConnection connection, DbConnectionInterceptionContext<System.Data.ConnectionState> interceptionContext) {
		}
	}

	public class SessionContextConfiguration : DbConfiguration {
		public SessionContextConfiguration() {
			AddInterceptor(new AddUserIdToSessionContextInterceptor());
		}
	}
}