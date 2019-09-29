﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NekoPetShop.Core.Entity;
using NekoPetShop.Core.ApplicationService;

namespace NekoPetShop.UI.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;


        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        // GET api/pets -- READ ALL
        [HttpGet]
        public ActionResult<IEnumerable<Pet>> Get([FromQuery] Filter filter)
        {
            try
            {
                List<Pet> filteredPets = _petService.ReadAll(filter);
                List<Object> specificPets = new List<object>();
                foreach (Pet pet in filteredPets)
                {
                    specificPets.Add(new { pet.Id, pet.Name, pet.Price, ownerFirstName = pet.Owner.FirstName ?? "No owner", ownerLastName = pet.Owner.LastName ?? "No owner", pet.PetColors });
                }

                return Ok(specificPets);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/pets/5 -- READ BY ID
        [HttpGet("{id}")]
        public ActionResult<Pet> Get(int id)
        {
            return _petService.ReadById(id);
        }

        // POST api/pets -- CREATE
        [HttpPost]
        public ActionResult Post([FromBody] Pet pet)
        {
            try
            {
                return Ok(_petService.Create(pet));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/pets/5 - UPDATE
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Pet pet)
        {
            try
            {
                if (id != pet.Id)
                {
                    return BadRequest("Parameter ID and pet ID have to be the same");
                }
                return Ok(_petService.Update(pet));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/pets/5 -- DELETE
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _petService.Delete(id);
                return Ok($"Deleted pet with Id: {id}");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}