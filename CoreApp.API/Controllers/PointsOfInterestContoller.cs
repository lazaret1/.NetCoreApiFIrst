using System;
using System.Linq;
using CoreApp.API.Models;
using CoreApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreApp.API.Controllers {
    [Route ("api/cities")]
    [ApiController]
    public class PointsOfInterestContoller : Controller {
        private ILogger<PointsOfInterestContoller> _logger;
        private IMailService _mailService;

        public PointsOfInterestContoller (ILogger<PointsOfInterestContoller> logger, 
        IMailService mailService) {
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet ("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest (int cityId) {
            try {

                var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);

                if (city == null) {
                    _logger.LogInformation ($"City with id {cityId} wasn't found when accessing points od interest.");
                    return NotFound ();
                }

                return Ok (city.PointsOfInterest);

            } catch (Exception ex) {
                _logger.LogInformation ($"Exception while getting points of interest for city with id {cityId}", ex);
                return StatusCode (500, "A problem happend while handling your request");
            }

        }

        [HttpGet ("{cityId}/pointsofinterest/{id}")]
        public IActionResult GetPointOfInterest (int cityId, int id) {
            var city = CitiesDataStore.Current.Cities.
            FirstOrDefault (c => c.Id == cityId);

            if (city == null) {
                return NotFound ();
            }

            var pointOfInterest = city.PointsOfInterest.
            FirstOrDefault (p => p.Id == id);

            if (pointOfInterest == null) {
                return NotFound ();
            }

            return Ok (pointOfInterest);
        }

        [HttpPost ("{cityId}/pointsofinterest", Name = "GetPointOfInterest")]
        public IActionResult CreatePointOfInterest (int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest) {
            if (pointOfInterest == null) {
                return BadRequest ();
            }

            if (pointOfInterest.Description == pointOfInterest.Name) {
                ModelState.AddModelError ("Description", "Description should be diffrent froz the name!");
            }

            if (ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var city = CitiesDataStore.Current.Cities.
            FirstOrDefault (c => c.Id == cityId);

            if (city == null) {
                return NotFound ();
            }

            var maxPointOfInterest = CitiesDataStore.Current.Cities.SelectMany (c => c.PointsOfInterest).Max (p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto () {
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add (finalPointOfInterest);

            return CreatedAtRoute ("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest }, finalPointOfInterest);
        }

        [HttpDelete ("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest (int cityId, int id) {
            var city = CitiesDataStore.Current.Cities.
            FirstOrDefault (c => c.Id == cityId);

            if (city == null) {
                return NotFound ();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.
            FirstOrDefault (p => p.Id == id);

            if (pointOfInterestFromStore == null) {
                return NotFound ();
            }

            city.PointsOfInterest.Remove (pointOfInterestFromStore);
            _mailService.Send("Point of interest deleted.",
             $"Point of interest {pointOfInterestFromStore} with id {pointOfInterestFromStore.Id} was deleted.");

            return NoContent ();
        }

    }

}