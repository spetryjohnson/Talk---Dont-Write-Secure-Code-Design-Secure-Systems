namespace Talk_BuildSecureSystems_RowLevelSecurity.Migrations {
	using System;
	using System.Data.Entity.Migrations;

	public partial class _02_AddedRLS : DbMigration {
		public override void Up() {
			Sql(@"
				CREATE SCHEMA Security
				go

				CREATE FUNCTION Security.userAccessPredicate(@UserId nvarchar(128))
					RETURNS TABLE
					WITH SCHEMABINDING
				AS
					RETURN SELECT 1 AS accessResult
					WHERE (
						(1 = CAST(SESSION_CONTEXT(N'CanViewAllOrders') AS bit))
						OR
						(@UserId = CAST(SESSION_CONTEXT(N'UserId') AS nvarchar(128)))
					)
				go

				CREATE SECURITY POLICY Security.userSecurityPolicy
					ADD FILTER PREDICATE Security.userAccessPredicate(UserId) ON dbo.Orders,
					ADD BLOCK PREDICATE Security.userAccessPredicate(UserId) ON dbo.Contacts
				go
			");
		}

		public override void Down() {
			Sql("DROP SCHEMA Security");
		}
	}
}
