using CCT.Core.Contracts;
using CCT.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("password")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostPassword(string password)
        {
            try
            {
                await _unitOfWork.SettingRepository.SetPasswordAsync(password);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("PostPassword", null);
        }

        [HttpGet("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetPassword()
        {
            var password = await _unitOfWork.SettingRepository.GetPasswordAsync();

            return Ok(password);
        }

        [HttpPost("welcomeText")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostWelcomeText(string welcomeText)
        {
            try
            {
                await _unitOfWork.SettingRepository.SetWelcomeTextAsync(welcomeText);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("PostWelcomeText", null);
        }

        [HttpGet("welcomeText")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetWelcomeText()
        {
            var welcomeText = await _unitOfWork.SettingRepository.GetWelcomeTextAsync();

            return Ok(welcomeText);
        }

        [HttpPost("duration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostStorageDuration(int durationInDays)
        {
            try
            {
                await _unitOfWork.SettingRepository.SetStorageDurationAsync(durationInDays);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("PostStorageDuration", null);
        }

        [HttpGet("duration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetStorageDuration()
        {
            var duration = await _unitOfWork.SettingRepository.GetStorageDurationAsync();

            return Ok(duration);
        }
    }
}
