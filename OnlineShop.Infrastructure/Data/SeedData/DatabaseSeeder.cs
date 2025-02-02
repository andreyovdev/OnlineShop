namespace OnlineShop.Infrastructure.Data.SeedData
{
	using Microsoft.AspNetCore.Identity;

	using Newtonsoft.Json;

	using Domain.Entities;
	using Identity;

	public class DatabaseSeeder : IDatabaseSeeder
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;

		private const string jsonsPath = "D:\\OnlineShop\\OnlineShop.Infrastructure\\Data\\SeedData";

		public DatabaseSeeder(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task SeedAsync()
		{
			await SeedProductsAsync();
			await SeedUsersAsync();
			await SeedRolesAsync();
			await SeedUserRolesAsync();
			await SeedUserProfiles();
			await SeedAddressesAsync();
		}

		public async Task SeedProductsAsync()
		{
			if (!_context.Products.Any())
			{
				var jsonPath = Path.Combine(jsonsPath, "products.json");
				var productsJson = File.ReadAllText(jsonPath);
				var products = JsonConvert.DeserializeObject<List<Product>>(productsJson);

				if (products != null)
				{
					await _context.Products.AddRangeAsync(products);
					await _context.SaveChangesAsync();
				}
			}
		}

		public async Task SeedAddressesAsync()
		{
			if (!_context.Addresses.Any())
			{
				var jsonPath = Path.Combine(jsonsPath, "addresses.json");
				var addressesJson = File.ReadAllText(jsonPath);
				var addresses = JsonConvert.DeserializeObject<List<Address>>(addressesJson);

				if (addresses == null) return;

				foreach (var address in addresses)
				{
					var existingAddress = await _context.Addresses.FindAsync(address.Id);
					if (existingAddress == null)
					{
						var ad = new Address
						{
							Id = address.Id,
							UserProfileId = address.UserProfileId,
							Country = address.Country,
							City = address.City,
							Street = address.Street,
							PhoneNumber = address.PhoneNumber,
						};

						await _context.Addresses.AddAsync(ad);
					}
				}
				await _context.SaveChangesAsync();

			}
		}

		public async Task SeedUsersAsync()
		{
			if (!_context.Users.Any())
			{
				var jsonPath = Path.Combine(jsonsPath, "users.json");
				var appUsersJson = File.ReadAllText(jsonPath);
				var appUsers = JsonConvert.DeserializeObject<List<AppUser>>(appUsersJson);

				if (appUsers == null) return;

				foreach (var user in appUsers)
				{
					var existingUser = await _userManager.FindByEmailAsync(user.Email);
					if (existingUser == null)
					{
						var newUser = new AppUser
						{
							Id = user.Id,
							UserName = user.UserName,
							FullName = user.FullName,
							Email = user.Email,
						};

						var result = await _userManager.CreateAsync(newUser, "password123");
					}
				}
			}
		}

		public async Task SeedRolesAsync()
		{
			if (!_context.Roles.Any())
			{
				var jsonPath = Path.Combine(jsonsPath, "roles.json");
				var rolesJson = File.ReadAllText(jsonPath);
				var roles = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(rolesJson);

				foreach (var role in roles)
				{
					string roleName = role["Name"];

					if (!await _roleManager.RoleExistsAsync(roleName))
					{
						await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
					}
				}
			}
		}

		public async Task SeedUserRolesAsync()
		{
			if (!_context.UserRoles.Any())
			{
				var jsonPath = Path.Combine(jsonsPath, "user_roles.json");
				var userRolesJson = File.ReadAllText(jsonPath);
				var userRoles = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(userRolesJson);

				foreach (var userRole in userRoles)
				{
					string email = userRole["Email"];
					string role = userRole["Role"];

					var user = await _userManager.FindByEmailAsync(email);
					if (user == null)
					{
						continue;
					}

					if (!await _userManager.IsInRoleAsync(user, role))
					{
						await _userManager.AddToRoleAsync(user, role);
					}
				}
			}
		}

		public async Task SeedUserProfiles()
		{
			if (!_context.UserProfiles.Any())
			{
				var jsonPath = Path.Combine(jsonsPath, "user_profiles.json");
				var userProfilesJson = await File.ReadAllTextAsync(jsonPath);
				var userProfiles = JsonConvert.DeserializeObject<List<UserProfile>>(userProfilesJson);

				if (userProfiles == null) return;

				foreach (var userProfile in userProfiles)
				{
					var existingProfile = await _context.UserProfiles.FindAsync(userProfile.Id);
					if (existingProfile == null)
					{
						var newUserProfile = new UserProfile
						{
							Id = userProfile.Id,
							FullName = userProfile.FullName ?? "Unknown User",
							Email = userProfile.Email ?? "Unknown Email",
							AppUserId = userProfile.AppUserId,
							AddressId = userProfile.AddressId,
							Wishlist = userProfile.Wishlist ?? new List<Wishlist>(),
							Cart = userProfile.Cart ?? new List<Cart>(),
							Purchases = userProfile.Purchases ?? new List<Purchase>()
						};

						_context.UserProfiles.Add(newUserProfile);
					}
				}

				await _context.SaveChangesAsync();
			}
		}
	}
}
