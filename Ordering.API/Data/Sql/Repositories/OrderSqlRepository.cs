using Dapper;
using GeekBurger.UI.Contract;
using System.Data.SqlClient;

namespace Ordering.API.Sql.Repositories
{
    public class OrderSqlRepository
    {
        private const string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FIAP15;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void Add(NewOrderMessage request)
        {
            try
            {
                string sqlInsertOrder = "INSERT INTO Orders (OrderId, StoreId) Values (@OrderId, @StoreId)";
                string sqlInsertProduct = " INSERT INTO Product (ProductId, OrderId) Values (@ProductId, @OrderId)";

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute(sqlInsertOrder, new { @OrderId = request.OrderId, @StoreId = request.StoreId });

                    if (request.Products == null || request.Products.Count == 0) return;
                    foreach (var product in request.Products)
                    {
                        connection.Execute(sqlInsertProduct, new { @ProductId = product.ProductId, @OrderId = request.OrderId });
                    }
                }

            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
