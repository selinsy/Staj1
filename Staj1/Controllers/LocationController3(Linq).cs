using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Staj1.Data;

namespace Staj1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController3_Linq_ : ControllerBase
    {
        private readonly DataContext _context;

        public LocationController3_Linq_(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Response GetAll()
        {
            var response = new Response();
            var temp = (from all in _context.Locations.OrderBy(x => x.Id) select all).ToList();
            response.Value = temp;
            return response;
        }

        [HttpGet("id")]
        public Response GetId(int id)
        {
            var response = new Response();
            //var loc = _context.Locations.Find(id);
            response.Value = (from n in _context.Locations.Where(n => n.Id == id)
                              select n).FirstOrDefault();
            if (response.Value == null)
            {
                response.Result = "Location not found.";
            }
            else
            {
                response.Result = "Location founded.";
            }
            return response;
        }

        [HttpDelete]
        public Response Delete(int id)
        {
            var response = new Response();
            var loc = (from n in _context.Locations.Where(n => n.Id == id)
                       select n).FirstOrDefault();
            if (loc == null)
            {
                response.Result = "Location not found";
            }
            else
            {
                response.Result = "Location deleted.";
                _context.Locations.Remove(loc);
                _context.SaveChanges();
            }
            return response;
        }

        [HttpPost]
        public Response AddLocation(Location _location)
        {
            var response = new Response();

            /*var tempLocation = (from n in _context.Locations.Where(x => x.Name.ToUpper() == _location.Name.ToUpper())
                            select n).FirstOrDefault();*/


            if (_location.Name.Trim().Length == 0 || _location.WKT.Trim().Length == 0)
            {
                response.Result = "Tüm değerleri giriniz.";
            }
            else
            {
                //string nameloc = _location.Name.toUpper(new CultureInfo("en-US", false));

                var name = (from _context in _context.Locations where _context.Name.ToUpper() == _location.Name.ToUpper() select _context).FirstOrDefault();
                if (name == null)
                {
                    Location loc = new Location();
                    _location.Name.Replace('I', 'İ');
                    _location.Name.Replace('ı', 'i');
                    loc.Name = _location.Name.ToUpper();
                    loc.WKT = _location.WKT;
                    _context.Locations.Add(loc);
                    _context.SaveChanges();
                    response.Result = "Eklendi.";
                }
                else response.Result = "Farklı name giriniz.";
            }
            return response;
        }

        [HttpPut]
        public Response Update(Location _location)
        {
            var response = new Response();
            var update = (from n in _context.Locations where n.Id == _location.Id select n).FirstOrDefault();

            if (update == null)
            {
                response.Result = "Location not found";
            }
            else
            {
                update.Id = _location.Id;
                if (_location.Name != "string")
                {
                    update.Name = _location.Name;
                }
                if (_location.WKT != "string")
                {
                    update.WKT = _location.WKT;
                }

                _context.SaveChanges();

                response.Value = _context.Locations.ToList();
            }
            return response;
        }
    }
}
