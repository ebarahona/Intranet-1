﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Intranet.API.ViewModels;

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class ProfileController : Controller, IRestController<Employee>
  {
    private readonly IntranetApiContext _intranetApiContext;

    public ProfileController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
      try
      {
        if (id == 0) return BadRequest(id);

        Employee employee = _intranetApiContext.Employees.Find(id);

        if (employee == null) return NotFound(id);

        _intranetApiContext.Employees.Remove(employee);
        _intranetApiContext.SaveChanges();

        return Ok(id);
      }
      catch (Exception e)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        if (!_intranetApiContext.Employees.Any()) return NotFound(new ProfileViewModel());

        var employees = _intranetApiContext.Employees.ToList();

        IList<ProfileViewModel> profiles = new List<ProfileViewModel>();
        foreach (var employee in employees)
        {
          var profile = new ProfileViewModel()
          {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Description = employee.Description,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Mobile = employee.Mobile,
            StreetAdress = employee.StreetAdress,
            PostalCode = employee.PostalCode,
            City = employee.City
          };

          profiles.Add(profile);
        }

        return Ok(profiles);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpGet]
    public IActionResult Get(int id)
    {
      try
      {
        if (id == 0)
        {
          return BadRequest(new ProfileViewModel());
        }

        var employee = _intranetApiContext.Employees.Find(id);

        if (employee == null)
        {
          return NotFound(new ProfileViewModel());
        }

        var profile = new ProfileViewModel()
        {
          Id = employee.Id,
          FirstName = employee.FirstName,
          LastName = employee.LastName,
          Description = employee.Description,
          Email = employee.Email,
          PhoneNumber = employee.PhoneNumber,
          Mobile = employee.Mobile,
          StreetAdress = employee.StreetAdress,
          PostalCode = employee.PostalCode,
          City = employee.City
        };

        return Ok(profile);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new ProfileViewModel());
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpPost]
    public IActionResult Post([FromBody] Employee employee)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var newEmployee = new Employee
        {
          FirstName = employee.FirstName,
          LastName = employee.LastName,
          Description = employee.Description,
          Email = employee.Email,
          PhoneNumber = employee.PhoneNumber,
          Mobile = employee.Mobile,
          StreetAdress = employee.StreetAdress,
          PostalCode = employee.PostalCode,
          City = employee.City
        };

        _intranetApiContext.Employees.Add(newEmployee);
        _intranetApiContext.SaveChanges();
        return Ok(ModelState);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpPut]
    public IActionResult Put(int id, [FromBody] Employee update)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var entityToUpdate = _intranetApiContext.Employees.Find(id);

        if (entityToUpdate == null)
        {
          return NotFound(id);
        }

        entityToUpdate.FirstName = update.FirstName;
        entityToUpdate.LastName = update.LastName;
        entityToUpdate.Description = update.Description;
        entityToUpdate.Email = update.Email;
        entityToUpdate.PhoneNumber = update.PhoneNumber;
        entityToUpdate.Mobile = update.Mobile;
        entityToUpdate.StreetAdress = update.StreetAdress;
        entityToUpdate.PostalCode = update.PostalCode;
        entityToUpdate.City = update.City;

        _intranetApiContext.SaveChanges();

        return Ok(ModelState);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  }
}