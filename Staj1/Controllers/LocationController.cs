using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Staj1;
using System.Data;

namespace Staj1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase

    {

        public List<Location> _Location = new List<Location>();

        static NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Database=Staj1;Username=postgres;Password=Selin1977");


        [HttpGet("connection")]
        public Object GetConnect()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();

            return "Bağlandı.";

        }

        [HttpGet("create")]
        public void Create()
        {
            GetConnect();
            var cmd = new NpgsqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Locations(Id SERIAL PRIMARY KEY, 
        Name VARCHAR(255), WKT Text)";
            cmd.ExecuteNonQuery();
            connection.Close();

        }


        [HttpPost]
        public Response Add(Location _location)
        {
            var _response = new Response();



            //Values of all variables must be entered.        
            if (_location.Name == "string" || _location.WKT == "string")
            {
                _response.Result = "Tüm değerleri giriniz.";
            }
            else
            {
                connection.Open();
                string query1 = $"SELECT* FROM Locations WHERE UPPER(Name)=UPPER(@Name)";
                NpgsqlCommand da = new NpgsqlCommand(query1, connection);
                da.Parameters.AddWithValue("Name", _location.Name);
                NpgsqlDataReader reader = da.ExecuteReader();
                Boolean a = reader.HasRows;
                connection.Close();
                //If there is not a row with the same name in the database before, it will be added.
                if (a == false)
                {
                    connection.Open();
                    string query = "INSERT INTO Locations (Name, WKT) VALUES (@name,@WKT)";
                    var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("id", _location.Id);
                    cmd.Parameters.AddWithValue("name", _location.Name);
                    cmd.Parameters.AddWithValue("WKT", _location.WKT);

                    cmd.ExecuteNonQuery();

                    _response.Result = "Başarılı.";

                }
                //If there is a row with the same name in the database before, it will not be added.
                else
                {
                    _response.Result = "basarisiz";
                    _response.Status = false;
                }

            }
            connection.Close();
            return _response;



            //cmd.Parameters.AddWithValue("Id", _location.Id);
            //_location.Id = new Random().Next();


        }
        //    return _Location;
        //  }

        /*_location.Id = new Random().Next();
        _Location.Add(_location);

    //    return "Ekleme başarılı.";*/
        //    return _Location;

        //}


        [HttpDelete]
        public Response Delete(int _Id)
        {
            GetConnect();
            var response = new Response();

            string cmdText = $"DELETE FROM Locations WHERE Id= (@p)";
            var cmd = new NpgsqlCommand(cmdText, connection);

            if (_Id > 0)
            {
                cmd.Parameters.AddWithValue("p", _Id);
                var res = cmd.ExecuteNonQuery();

                if (res == -1)
                {
                    response.Result = "Location not found.";
                }
                else
                {
                    response.Result = "Silindi";
                }
            }
            else
            {
                response.Result = "Location not found.";
            }


            connection.Close();
            return response;
        }

        //    /*var l = _Location.Find(x => x.Id == _Id);

        //    return "Silme başarılı.";*/


        //}


        [HttpPut]
        public Response Update(Location location)
        {
            var _response = new Response();

            GetConnect();
            Location tempLocation = new Location();
            string query = "SELECT * FROM Locations WHERE Id= " + location.Id;
            NpgsqlCommand da = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = da.ExecuteReader();
            reader.Read();

            tempLocation.Id = reader.GetInt32(0);
            tempLocation.Name = reader.GetString(1);
            tempLocation.WKT = reader.GetString(2);

            _Location.Add(tempLocation);


            connection.Close();
            connection.Open();
            if (location.Name == "string")
            {

                location.Name = tempLocation.Name;

            }
            if (location.WKT == "string")
            {

                location.WKT = tempLocation.WKT;

            }


            var commandText = "UPDATE locations SET Name=@name, WKT=@WKT WHERE Id=@id";
            var cmd = new NpgsqlCommand(commandText, connection);

            cmd.Parameters.AddWithValue("Id", location.Id);
            cmd.Parameters.AddWithValue("name", location.Name);
            cmd.Parameters.AddWithValue("WKT", location.WKT);



            cmd.ExecuteNonQuery();
            connection.Close();
            //_Location.Add(tempLocation);
            //_response.Value = _Location;
            //_response.Result = "Kayıt Bulundu.";
            _response.Status = true;
            return _response;


            /* NpgsqlCommand cmd = new NpgsqlCommand($"UPDATE Location SET" + location.Name + "," + location.X + "," + location.Y+ "WHERE Id= "+id);


                Location l = new Location();
                l = Get(id);
                    //_Location.Find(a => a.Id == id);
               if (l != null)
               {
                    l.Name = location.Name;
                    l.X = location.X;
                    l.Y = location.Y;
                    connection.Close();
                    return "Update oldu.";
                }  
                else connection.Close();  return "Aranan değer bulunmadı.";

                return "Update oldu.";*/

        }

        [HttpGet("id")]
        public Location Get(int id)
        {
            GetConnect();
            string query = $"SELECT * FROM Locations WHERE Id= {id}";
            NpgsqlCommand da = new NpgsqlCommand(query, connection);
            //da.Parameters.AddWithValue("id", id);

            NpgsqlDataReader reader = da.ExecuteReader();

            reader.Read();

            Location location = new Location();
            location.Id = reader.GetInt32(0);
            location.Name = reader.GetString(1);
            location.WKT = reader.GetString(2);



            return location;


            /* var location = _Location.Find(a => a.Id == id);
             if (location == null) return null;
             else return location;*/

        }


        [HttpGet]

        public List<Location> GetAll()
        {
            GetConnect();
            string query = "SELECT * FROM Locations WHERE Id= @id ORDER BY ID ASC";
            var cmd = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Location location = new Location();
                location.Id = reader.GetInt32(0);
                location.Name = reader.GetString(1);
                location.WKT = reader.GetString(2);
                _Location.Add(location);
            }
            connection.Close();
            return _Location;

        }

        [HttpGet("GetAllLocations")]
        public List<string> GetAllLocations()
        {
            GetConnect();
            string query = "SELECT WKT FROM Locations";
            List<string> Locations = new List<string>();

            var cmd = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Locations.Add(reader.GetString(0));
            }
            connection.Close();
            return Locations;

        }

    }
}
