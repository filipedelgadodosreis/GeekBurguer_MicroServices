using Dapper;
using GeekBurger.UI.Contract;
using System;
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
                string sqlInsertOrder = "INSERT INTO Orders (OrderId, StoreId,OrderDate,OrderStatusId) Values (@OrderId, @StoreId,@OrderDate,@OrderStatusId)";
                string sqlInsertProduct = " INSERT INTO Product (ProductId, OrderId) Values (@ProductId, @OrderId)";

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute(sqlInsertOrder, new { request.OrderId, request.StoreId, @OrderDate = DateTime.Now, @OrderStatusId = 1 });

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
