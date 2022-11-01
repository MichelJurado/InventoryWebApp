using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace InventoryWebApp.Data
{
    public class FruitDapperService
    {
        private string connectionString = "";

        public FruitDapperService()
        {
            connectionString = @"Data Source=localhost; Integrated Security=true; initial catalog=InventoryWebApp; user id=LibCommResourcesUser; password=Government1;";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public async Task<IEnumerable<FruitsAvailable>> GetFruits()
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return await dbConnection.QueryAsync<FruitsAvailable>("SELECT * FROM dbo.FruitsAvailable");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FruitsAvailable GetFruitsById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM dbo.FruitsAvailable WHERE ID = @Id";
                dbConnection.Open();
                return dbConnection.Query<FruitsAvailable>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void UpdateFruits(FruitsAvailable fruit)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"UPDATE dbo.FruitsAvailable SET Quantity = Quantity - 1 WHERE ID = @ID

                                  UPDATE 
                                  FS
                                  SET
                                  FS.Price = FS.Price + FA.Price
                                  FROM
                                  dbo.FruitsSold FS
                                  INNER JOIN FruitsAvailable FA
                                  ON FS.ID = @ID

                                  UPDATE dbo.FruitsSold
                                  SET Quantity = Quantity + 1
                                  WHERE ID = @ID;";

                dbConnection.Open();
                dbConnection.Query(sQuery, fruit);
            }
        }


    }
}



