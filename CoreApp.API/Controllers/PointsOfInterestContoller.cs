using System.Linq;
using CoreApp.API.Models;
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

        [HttpPost("{cityId}/pointsofinterest", Name = "GetPointOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if(pointOfInterest == null)
            {
                return BadRequest();
            }

            if(pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "Description should be diffrent froz the name!");
            }


            if(ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.
                FirstOrDefault (c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound ();
            }

            var maxPointOfInterest = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new 
                {cityId = cityId, id = finalPointOfInterest}, finalPointOfInterest);
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.
                FirstOrDefault (c => c.Id == cityId);

            if (city == null) 
            {
                return NotFound ();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.
                FirstOrDefault(p => p.Id == id);

            if(pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }

    }

}