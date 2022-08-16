using Microsoft.AspNetCore.Mvc;
using Staj1.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Staj1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController2 : ControllerBase
    {
        private readonly DataContext _context;

        public LocationController2(DataContext context)
        {
            _context = context;
        }
        [HttpGet]

        public Response GetAll()
        {
            var response = new Response();
            response.Value = _context.Locations.OrderBy(x => x.Id).ToList();
            //response.Status = true;
            return response;
        }

        [HttpGet("id")]
        public Response GetId(int id)
        {
            var response = new Response();
            var loc = _context.Locations.Find(id);
            if (loc == null)
            {
                response.Result = "Location not found.";
            }
            else
            {
                response.Result = "Location founded.";
                response.Value = loc;
            }

            return response;
        }
            [HttpPost]
            public Response AddLocation(Location _location)
            {
                var response = new Response();
                Location tempLocation = new Location();
                tempLocation = _context.Locations.FirstOrDefault(x => x.Name.ToUpper() == _location.Name.ToUpper());

                if (_location.Name == "string" || _location.WKT)
                {
                    response.Result = "Tüm değerleri giriniz.";

                }
                else
                {
                    if (tempLocation == null)
                    {

                        Location loc = new Location();
                        loc.Name = _location.Name;
                        loc.X = _location.X;
                        loc.Y = _location.Y;
                        _context.Locations.Add(_location);
                        _context.SaveChanges();
                        response.Value = loc;
                    }
                    else response.Result = "Farklı name giriniz.";
                }


                response.Value = _context.Locations.ToList();
                return response;
            }

            [HttpPut]
            public Response Update(Location _location)
            {
            var response = new Response();
                var loc = _context.Locations.Find(_location.Id);
                if (loc == null)
                {
                    response.Result = "Location not found";
                }
                else
                {
                    loc.Id = _location.Id;
                    if (_location.Name != "string")
                    {
                        loc.Name = _location.Name;
                    }
                    if (_location.X != 0)
                    {
                        loc.X = _location.X;
                    }
                    if (_location.Y != 0)
                    {
                        loc.Y = _location.Y;
                    }
                     _context.SaveChanges();

                    response.Value = _context.Locations.ToList();
                }
                return response;




            }

            [HttpDelete]
            public Response Delete(int id)
            {
                var response = new Response();
                var loc = _context.Locations.Find(id);
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


        }
    }

