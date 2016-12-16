using Microsoft.AspNet.Identity;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace SecureFrameworkDemo.Framework.RowLevelSecurity {

	/// <summary>
	/// See https://docs.microsoft.com/en-us/azure/app-service-web/web-sites-dotnet-entity-framework-row-level-security
	/// 
	/// This requires SQL Server 2016 to work. 
	/// 
	/// TODO: Put a flag in web.config that disables the RLS stuff, so that the demo can be run locally without it.
	/// </summary>
	public class AddUserIdToSessionContextInterceptor : IDbConnectionInterceptor {

		public void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext) {
			// Set SESSION_CONTEXT to current UserId whenever EF opens a connection
			try {
				var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

				if (userId != null) {
					// temporarily removed while working on insecure setuff
					//DbCommand cmd = connection.CreateCommand();
					//cmd.CommandText = "EXEC sp_set_session_context @key=N'UserId', @value=@UserId";
					//DbParameter param = cmd.CreateParameter();
					//param.ParameterName = "@UserId";
					//param.Value = userId;
					//cmd.Parameters.Add(param);
					//cmd.ExecuteNonQuery();
				}
			}
			catch (System.NullReferenceException) {
				// If no user is logged in, leave SESSION_CONTEXT null (all rows will be filtered)
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