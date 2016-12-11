namespace BuildSecureSystems.Migrations {
	using System;
	using System.Data.Entity.Migrations;

	public partial class _02_AddedRLS : DbMigration {
		public override void Up() {
			Sql(@"
				CREATE SCHEMA Security
				go

				-- INPUT:  The user ID that owns an order
				-- OUTPUT: Returns TRUE if the ID being passed in equals the 'current user' -OR- the current 
				--         user can access all orders
				CREATE FUNCTION Security.currentUserCanViewAllOrdersOrIdEquals(@UserId nvarchar(128))
					RETURNS TABLE
					WITH SCHEMABINDING
				AS
					RETURN SELECT 1 AS accessResult
					WHERE (
						-- If the context isn't populated, don't restrict. Otherwise admins can't manage via SSMS
						(SESSION_CONTEXT(N'UserId') IS NULL)
						
						-- Allow the current user to access their own stuff
						OR (@UserId = CAST(SESSION_CONTEXT(N'UserId') AS nvarchar(128)))
						
						-- Permission check, to see if current user can access orders they don't own
						OR EXISTS (
							SELECT	1
							FROM	dbo.PermissionApplicationUsers
							WHERE	Permission_Id = 200	-- ViewOrdersForOthers
									AND ApplicationUser_Id = CAST(SESSION_CONTEXT(N'UserId') AS nvarchar(128))
						)
					)
				go

				CREATE SECURITY POLICY Security.orderAccessControlPolicy
					ADD FILTER PREDICATE Security.currentUserCanViewAllOrdersOrIdEquals(Applicationuser_Id) ON dbo.Orders
				go
			");
		}

		public override void Down() {
			Sql(@"
				DROP SECURITY POLICY IF EXISTS Security.orderAccessControlPolicy;
				DROP FUNCTION IF EXISTS Security.currentUserCanViewAllOrdersOrIdEquals;
				DROP SCHEMA IF EXISTS Security;
			");
		}
	}
}
