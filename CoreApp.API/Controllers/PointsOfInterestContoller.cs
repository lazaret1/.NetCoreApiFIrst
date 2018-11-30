using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.API.Controllers {
    [Route ("api/cities")]
    [ApiController]
    public class PointsOfInterestContoller : Controller 
    {
        [HttpGet ("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest (int cityId) {
            var city = CitiesDataStore.Current.Cities.
                FirstOrDefault (c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound ();
            }
            
            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.
                FirstOrDefault (c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound ();
            }

            var pointOfInterest = city.PointsOfInterest.
                FirstOrDefault(p => p.Id == id);

            if(pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

    }

}