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

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class ChecklistController : Controller, IRestController<Checklist>
  {
    private readonly IntranetApiContext _intranetApiContext;

    public ChecklistController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpGet]
    // GET api/v1/checklist/1 return a checklist task by id and description 
    public IActionResult Get(int id)
    {
      try
      {
        var checklistTask = _intranetApiContext.Checklist.Find(id);

        if (checklistTask == null)
        {
          return NotFound(new Checklist());
        }

        return Ok(checklistTask);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new Checklist());
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    public IActionResult Get()
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpPost]
    public IActionResult Post([FromBody] Checklist body)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpPut]
    public IActionResult Put(int id, [FromBody] Checklist body)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }
  }
}