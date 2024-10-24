using AutoMapper;
using DataService.Repository.Interfaces;
using Entities.DbSet;
using Entities.Dtos.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace EventBookingApi.Controllers;

public class EventsController : BaseController
{
    private readonly IValidator<GetGeolocationRequest> _createValidator;
    private readonly IGeolocationService _geolocationService;

    public EventsController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<GetGeolocationRequest> createValidator,
        IGeolocationService geolocationService) : base(unitOfWork, mapper)
    {
        _createValidator = createValidator;
        _geolocationService = geolocationService;
    }

    [HttpGet]
    [Route("GetByIp/{name}")]
    public async Task<IActionResult> GetEventByName(string name) 
    {
        var searchedEvent = await _geolocationService.GetGeolocationData(name.Replace("%2F", "/"));

        if (searchedEvent == null) 
        {
            return NotFound($"Event with name {name} not found");
        }

        //var result = _mapper.Map<GeolocationDataResposne>(searchedEvent);
        return Ok(searchedEvent);
    }

    [HttpGet]
    [Route("GetByUrl/{country}")]
    public async Task<IActionResult> GetEventsByCountry(string country)
    {
        var eventsByCountry = await _geolocationService.GetGeolocationData(country.Replace("%2F", "/"));
        //var result = _mapper.Map<IEnumerable<Entities.Dtos.Responses.GeolocationData>>(eventsByCountry);
        return Ok(eventsByCountry);
    }

    [HttpPost("Event")]
    public async Task<IActionResult> CreateEvent([FromBody] GeolocationData createEventRequest) 
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest();
        }
        //createEventRequest.Url = createEventRequest.Url.Replace("%2F", "/");
        //var validationResult = await _createValidator.ValidateAsync(createEventRequest);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(validationResult.ToString("~"));
        //}

        try
        {
            var eventToCreate = _mapper.Map<Entities.DbSet.GeolocationData>(createEventRequest);
            var result = await _unitOfWork.Events.Create(eventToCreate);
            //var searchedEvent = await _geolocationService.AddGeolocationData(eventToCreate);
            var unitOfWorkResult = await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetEventByName), new { name = result?.Ip }, result);
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(ex.Message);
        }     
    }

    [HttpDelete]
    [Route("Event/{name}")]
    public async Task<IActionResult> UpdateEvent(string name)
    {
        var result = await _unitOfWork.Events.Delete(name);
        var unitOfWorkResult = await _unitOfWork.CompleteAsync();
        if (result == null)
        {
            return NotFound($"Event with name {name} not found");
        }

        return Ok(result);
    }
}
