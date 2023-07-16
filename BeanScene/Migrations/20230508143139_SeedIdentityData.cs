using BeanScene.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanScene.Migrations
{
	public partial class SeedIdentityData : Migration
	{
		private MigrationBuilder _migrationBuilder;

		// Role IDs 
		private Dictionary<string, string> _roleIds = new Dictionary<string, string>
		{
			{ "User", "82f0fa55-2f23-459a-8b8c-1b0d85ab0270"},
			{ "Staff", "f805be06-9c6b-4161-a3c3-6e413c923c77"},
			{ "Manager", "bed003c6-e8d3-4141-8106-eb79fcb83bf8"},
		};

		private Dictionary<string, string> _userIds = new Dictionary<string, string>
		{
			{ "seed1", "d0a19aa3-903b-48d8-a592-fba588d549c9"},
			{ "seed2", "5de6a3ad-3e89-4fae-99df-f7b7110e36cf"},
			{ "seed3", "03520368-7621-4d20-9d1b-b1fb73b134fb"},
			{ "seed4", "e9db31a2-3fcd-4ee2-8dd8-1dfcbef2ffbd"}
		};

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Make sure the migration builder is available to other methods
			_migrationBuilder = migrationBuilder;

			// Add roles to the AspNetRoles table
			CreateRole(_roleIds["User"], "User");
			CreateRole(_roleIds["Staff"], "Staff");
			CreateRole(_roleIds["Manager"], "Manager");

			// Add users to the AspNetUsers table
			CreateUser(_userIds["seed1"], "seed1@test.com", "ultimatepassword", "seed1@test.com", "0404 040 404", "Seed1", "User",
				new string[] { _roleIds["User"] });
			CreateUser(_userIds["seed2"], "seed2@test.com", "ultimatepassword", "seed2@test.com", "0404 040 404", "Seed2", "Staff",
			   new string[] { _roleIds["Staff"] });
			CreateUser(_userIds["seed3"], "seed3@test.com", "ultimatepassword", "seed3@test.com", "0404 040 404", "Seed3", "Manager",
			   new string[] { _roleIds["Staff"], _roleIds["Manager"] });
			CreateUser(_userIds["seed4"], "seed4@test.com", "ultimatepassword", "seed4@test.com", "0404 040 404", "Seed4", "No Role");

			// Assign roles to users



		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

			// Make sure the migration builder is available to other methods
			_migrationBuilder = migrationBuilder;

			// Role IDs
			// Delete roles from the AspNetRoles table
			foreach (string roleId in _roleIds.Values)
			{
				DeleteRole(roleId);
			}

			// Delete users from the AspNetUsers table
			foreach (string userId in _userIds.Values)
			{
				DeleteUser(userId);
			}


		}

		/// <summary>
		/// Create an identity role
		/// </summary>
		private void CreateRole(string id, string name)
		{
			// TODO : Validation

			// Create an IdentityRole object to hold all the data
			IdentityRole role = new IdentityRole()
			{
				Id = id,
				Name = name
			};

			// Generate normalised name
			role.NormalizedName = name.ToUpperInvariant();

			//Generate stamp (random value that must change whenewer a role is persisted to the store)
			role.ConcurrencyStamp = Guid.NewGuid().ToString();

			// Build query
			string[] fields = { "Id", "Name", "NormalizedName", "ConcurrencyStamp" };
			string[] data = { role.Id, role.Name, role.NormalizedName, role.ConcurrencyStamp };

			// Insert record into the database
			_migrationBuilder.InsertData("AspNetRoles", fields, data);
		}


		/// <summary>
		/// Create an identity user
		/// </summary>
		private void CreateUser(string id, string userName, string password, string email, string? phone, string firstName, string lastName, string[]? rolesIds = null)
		{
			// TODO : Validation

			// Create an IdentityRole object to hold all the data
			BeanSceneApplicationUser user = new BeanSceneApplicationUser()
			{
				Id = id,
				UserName = userName,
				Email = email,
				PhoneNumber = phone,
				FirstName = firstName,
				LastName = lastName
			};

			// Generate normalised name and email (to avoid duplicate users)
			user.NormalizedUserName = userName.ToUpperInvariant();
			user.NormalizedEmail = email.ToUpperInvariant();

			//Generate the concurrency stamp (random value that must change whenewer a user is persisted to the store)
			user.ConcurrencyStamp = Guid.NewGuid().ToString();

			//Generate the security stamp (whenever the )
			user.SecurityStamp = Guid.NewGuid().ToString();

			// Generate password hash from the plaintext password
			PasswordHasher<BeanSceneApplicationUser> passwordHasher = new PasswordHasher<BeanSceneApplicationUser>();
			user.PasswordHash = passwordHasher.HashPassword(user, password);

			// Other data
			user.EmailConfirmed = true;
			user.PhoneNumberConfirmed = false;
			user.TwoFactorEnabled = false;
			user.LockoutEnabled = true;
			user.LockoutEnd = null;
			user.AccessFailedCount = 0;


			// Build query
			string[] fields =
				{
					"Id"
					,"UserName"
					,"NormalizedUserName"
					,"Email"
					,"NormalizedEmail"
					,"EmailConfirmed"
					,"PasswordHash"
					,"SecurityStamp"
					,"ConcurrencyStamp"
					,"PhoneNumber"
					,"PhoneNumberConfirmed"
					,"TwoFactorEnabled"
					,"LockoutEnd"
					,"LockoutEnabled"
					,"AccessFailedCount"
					,"FirstName"
					,"LastName"
				};

			object[] data =
				{
					user.Id,
					user.UserName,
					user.NormalizedUserName,
					user.Email,
					user.NormalizedEmail,
					user.EmailConfirmed,
					user.PasswordHash,
					user.SecurityStamp,
					user.ConcurrencyStamp,
					user.PhoneNumber,
					user.PhoneNumberConfirmed,
					user.TwoFactorEnabled,
					user.LockoutEnd,
					user.LockoutEnabled,
					user.AccessFailedCount,
					user.FirstName,
					user.LastName,
				};

			// Insert record into the database
			_migrationBuilder.InsertData("AspNetUsers", fields, data);

			// Assign role(s) to user
			if (rolesIds != null)
			{
				foreach (string roleId in rolesIds)
				{
					AssignRoleToUser(user.Id, roleId);
				}
			}
		}

		/// <summary>
		/// Assign a role to a user
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="roleId"></param>
		private void AssignRoleToUser(string userId, string roleId)
		{
			// TODO: validation (e.g. check if both the role and user exist)?

			// Build query
			string[] fields = { "UserId", "RoleId" };
			object[] data = { userId, roleId };

			_migrationBuilder.InsertData("AspNetUserRoles", fields, data);
		}

		private void DeleteRole(string id)
		{
			_migrationBuilder.DeleteData("AspNetRoles", "Id", id);
		}

		private void DeleteUser(string id)
		{
			_migrationBuilder.DeleteData("AspNetUsers", "Id", id);
		}


	}
	/*public partial class SeedIdentityData : Migration
	{
		private MigrationBuilder _migrationBuilder;
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Making sure that migration builder is available to other methods
			_migrationBuilder = migrationBuilder;
			// Add roles to the AspNetRoles table

			// Role IDs
			string role1Id = "252ab99f-ad30-45fb-94de-e9d616f74f9d";
			string role2Id = "0ca00f04-f85c-4660-aa30-71a16784c08d";
			// Add users to the AspNetUsers table
			CreateRole(role1Id, "User");
			CreateRole(role2Id, "Staff");

			// Assign roles to users

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Making sure that migration builder is available to other methods
			_migrationBuilder = migrationBuilder;
			// Delete roles to the AspNetRoles table

			// Delete users to the AspNetUsers table 

		}

		private void CreateRole(string id, string name)
		{
			IdentityRole identityRole = new IdentityRole()
			{
				Id = id,
				Name = name
			};
			// Generate normalizeed name
			identityRole.NormalizedName = name.ToUpperInvariant();

			// Generate the concurrency stamp (random value that must change whenever a user is persisted to the store)
			identityRole.ConcurrencyStamp = Guid.NewGuid().ToString();

			// Build query (based on the object)

			string[] fields = { "Id", "Name", "NormalizedName", "ConcurrencyStamp" };
			string[] data = { identityRole.Id, identityRole.Name, identityRole.NormalizedName, identityRole.ConcurrencyStamp };


			// Insert record into the database
			_migrationBuilder.InsertData("AspNetRoles", fields, data);

		}
	}*/
}
