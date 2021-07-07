using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Extensions;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
	[Route("api/[controller]")]
	[ApiController]
	public class VehiclesController : ControllerBase {
		private readonly IAutobarnDatabase db;

		public VehiclesController(IAutobarnDatabase db) {
			this.db = db;
		}

		private dynamic Paginate(string url, int index, int count, int total)
        {
			dynamic links = new ExpandoObject();
			links.self = new { href = $"{url}?page={index}&size={count}" };
			if (index < total)
            {
				links.next = new { href = $"{url}?page={index + count}&size={count}" };
			}
            if (index > 0)
            {
				links.previous = new { href = $"{url}?page={index - count}&size={count}" };
			}
			return links;
		}

		// GET: api/vehicles
		[HttpGet]
		[Produces("application/hal+json")]
		public object Get(int index = 0, int count = 10) {
			var items = db.ListVehicles().Skip(index).Take(count).Select(v => v.ToResource());

			var _links = Paginate("/api/vehicles", index, count, db.CountVehicles());
			var result = new
			{
				_links,
				items
			};

			return items;
		}

		// GET api/vehicles/ABC123
		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			return Ok(vehicle);
		}

		// POST api/vehicles
		[HttpPost]
		public IActionResult Post([FromBody] VehicleDto dto) {
			var vehicleModel = db.FindModel(dto.ModelCode);
			var vehicle = new Vehicle {
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				VehicleModel = vehicleModel
			};
			db.CreateVehicle(vehicle);
			return Ok(dto);
		}

		// PUT api/vehicles/ABC123
		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromBody] VehicleDto dto) {
			var vehicleModel = db.FindModel(dto.ModelCode);
			var vehicle = new Vehicle {
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				ModelCode = vehicleModel.Code
			};
			db.UpdateVehicle(vehicle);
			return Ok(dto);
		}

		// DELETE api/vehicles/ABC123
		[HttpDelete("{id}")]
		public IActionResult Delete(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			db.DeleteVehicle(vehicle);
			return NoContent();
		}
	}
}
