using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.Core.Contracts;
using CCT.Core.Entities;
using CCT.WebAPI.DataTransferObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Returns all data from people stored in db
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllPersons()
        {
            var personsInDb = await _unitOfWork.PersonRepository.GetAllPersonAsync();

            if (personsInDb != null)
            {
                return Ok(personsInDb);
            }

            return NotFound();
        }

        /// <summary>
        /// returns the person with the given phoneNumber from the db
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("phone/{phoneNumber}")]
        public async Task<ActionResult> GetPersonByPhoneNumber(string phoneNumber)
        {
            var person = await _unitOfWork.PersonRepository.GetPersonByPhoneNumberAsync(phoneNumber);

            if(person != null)
            {
                return Ok(person);
            }

            return NotFound(phoneNumber);
        }

        private string CleanPhoneNumber(string phoneNumber)
        {
            if(phoneNumber.Contains("/"))
            {
                string cleanString = phoneNumber.Replace("/", string.Empty);
                return cleanString;
            }

            return phoneNumber;
        }

        /// <summary>
        /// returns all entries for the given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("date/{date}")]
        public async Task<ActionResult> GetPersonsByDate(DateTime date)
        {
            var persons = await _unitOfWork.PersonRepository.GetPersonsByDateAsync(date);

            if (persons != null)
            {
                return Ok(persons);
            }

            return NotFound();
        }

        /// <summary>
        /// returns all entries for today
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("today")]
        public async Task<ActionResult> GetPersonsForToday()
        {
            var persons = await _unitOfWork.PersonRepository.GetPersonsForTodayAsync();

            if (persons != null)
            {
                return Ok(persons);
            }

            return NotFound();
        }

        /// <summary>
        /// Posts a person to the DB
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostPerson(PersonDto personDto)
        {
            string cleanedNumber = CleanPhoneNumber(personDto.PhoneNumber);

            try
            {
                await _unitOfWork.PersonRepository
                .AddPersonAsync(new Person
                {
                    FirstName = personDto.FirstName,
                    LastName = personDto.LastName,
                    PhoneNumber = cleanedNumber,
                    RecordTime = DateTime.Now
                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return CreatedAtAction("PostPerson", personDto);
        }
    }
}
