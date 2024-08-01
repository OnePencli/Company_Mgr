using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Company_Mgr.Model;

namespace Company_Mgr._Repositories
{
    //这里相当于在和数据库进行交互  获取数据
    public class PetRepository : BaseRepository, IPetRepository
    {
        public PetRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(PetModel petModel)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into tb_Pet values (@id,@name,@type,@color)";
                command.Parameters.Add("@id", SqlDbType.Int).Value = petModel.Id;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = petModel.Name;
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = petModel.Type;
                command.Parameters.Add("@color", SqlDbType.NVarChar).Value = petModel.Color;
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "delete from tb_Pet where Pet_Id=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.ExecuteNonQuery();
            }
        }

        public void Edit(PetModel petModel)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "update tb_Pet set Pet_Name=@name,Pet_Type=@type,Pet_Color=@color where Pet_Id=@id";
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = petModel.Name;
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = petModel.Type;
                command.Parameters.Add("@color", SqlDbType.NVarChar).Value = petModel.Color;
                command.Parameters.Add("@id", SqlDbType.Int).Value = petModel.Id;
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<PetModel> GetAll()
        {
            var perList = new List<PetModel>();
            using (var connection = new SqlConnection(connectionString)) 
            using(var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from tb_Pet order by Pet_Id desc";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PetModel petModel = new PetModel();
                        petModel.Id = (int)reader[0];
                        petModel.Name = reader[1].ToString();
                        petModel.Type = reader[2].ToString();
                        petModel.Color = reader[3].ToString();
                        perList.Add(petModel);
                    }
                }
            }
            return perList;
        }

        public IEnumerable<PetModel> GetByValue(string value)
        {
            var perList = new List<PetModel>();
            int petId = int.TryParse(value, out _) ? Convert.ToInt32(value) : 0;
            string petName = value;
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"Select * from tb_Pet 
                                        where Pet_Id=@id or Pet_Name like @name+'%'
                                        order by Pet_Id desc";
                command.Parameters.Add("@id", SqlDbType.Int).Value = petId;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = petName;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PetModel petModel = new PetModel();
                        petModel.Id = (int)reader[0];
                        petModel.Name = reader[1].ToString();
                        petModel.Type = reader[2].ToString();
                        petModel.Color = reader[3].ToString();
                        perList.Add(petModel);
                    }
                }
            }
            return perList;
        }
    }
}
