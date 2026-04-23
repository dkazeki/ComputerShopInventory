using MySqlConnector;
using GenshinInventory.Models;

namespace GenshinInventory.Services
{
    public class DatabaseService
    {
        private readonly string connectionString = "server=localhost;database=genshin_db;uid=root;pwd=pda31128_;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public bool ValidateAdmin(string username, string password)
        {
            using var conn = GetConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM admins WHERE username = @username AND password = @password";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
        public void AddProduct(Product product)
        {
            using var conn = GetConnection();
            conn.Open();

            string query = @"INSERT INTO products
                            (product_name, category, brand, quantity, price)
                            VALUES
                            (@product_name, @category, @brand, @quantity, @price)";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@product_name", product.ProductName);
            cmd.Parameters.AddWithValue("@category", product.Category);
            cmd.Parameters.AddWithValue("@brand", product.Brand);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@price", product.Price);

            cmd.ExecuteNonQuery();
        }
        public AdminProfile GetAdminProfile()
        {
            using var conn = GetConnection();
            conn.Open();

            string query = "SELECT id, full_name, username, email FROM admin_profile LIMIT 1";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new AdminProfile
                {
                    Id = reader.GetInt32("id"),
                    FullName = reader.GetString("full_name"),
                    Username = reader.GetString("username"),
                    Email = reader.GetString("email")
                };
            }

            return new AdminProfile();
        }

        public void UpdateAdminProfile(AdminProfile profile)
        {
            using var conn = GetConnection();
            conn.Open();

            string query = @"UPDATE admin_profile
                     SET full_name = @full_name,
                         username = @username,
                         email = @email
                     WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@full_name", profile.FullName);
            cmd.Parameters.AddWithValue("@username", profile.Username);
            cmd.Parameters.AddWithValue("@email", profile.Email);
            cmd.Parameters.AddWithValue("@id", profile.Id);

            cmd.ExecuteNonQuery();
        }
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using var conn = GetConnection();
            conn.Open();

            string query = "SELECT id, product_name, category, brand, quantity, price FROM products";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32("id"),
                    ProductName = reader.GetString("product_name"),
                    Category = reader.GetString("category"),
                    Brand = reader.GetString("brand"),
                    Quantity = reader.GetInt32("quantity"),
                    Price = reader.GetDecimal("price")
                });
            }

            return products;
        }
        public void UpdateProduct(Product product)
        {
            using var conn = GetConnection();
            conn.Open();

            string query = @"UPDATE products
                     SET product_name = @product_name,
                         category = @category,
                         brand = @brand,
                         quantity = @quantity,
                         price = @price
                     WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@product_name", product.ProductName);
            cmd.Parameters.AddWithValue("@category", product.Category);
            cmd.Parameters.AddWithValue("@brand", product.Brand);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@id", product.Id);

            cmd.ExecuteNonQuery();
        }

        public void DeleteProduct(int id)
        {
            using var conn = GetConnection();
            conn.Open();

            string query = "DELETE FROM products WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}