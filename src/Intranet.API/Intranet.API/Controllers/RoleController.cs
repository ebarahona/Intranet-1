﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Intranet.API.Domain.Models.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Intranet.API.Domain.Data;
//using Intranet.API.ViewModels;

//namespace Intranet.API.Controllers
//{
//    /// <summary>
//    /// Manage the type of worker roles that can be added to an assignment.
//    /// An assignment is a project that an employee is assigned to.
//    /// </summary>
//    [Produces("application/json")]
//    [Route("/api/v1/[controller]")]
//    public class RoleController : Controller, IRestController<Role>
//    {
//        private readonly IntranetApiContext _intranetApiContext;

//        public RoleController(IntranetApiContext intranetApiContext)
//        {
//            _intranetApiContext = intranetApiContext;
//        }

//        /// <summary>
//        /// Retrieve a list of all roles.
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public IActionResult Get()
//        {
//            try
//            {
//                var roles = _intranetApiContext.Roles.ToList();

//                if (roles.Count == 0)
//                {
//                    var error = Json("No roles found");
//                    return NotFound(error);
//                }

//                return Ok(roles);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Retrieve a specific role.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpGet]
//        public IActionResult Get(int id)
//        {
//            try
//            {
//                var role = _intranetApiContext.Roles.Find(id);

//                if (role == null)
//                {
//                    var error = Json("No role found with id: " + id);
//                    return NotFound(error);
//                }

//                return Ok(role);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Remove a specific role.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            try
//            {
//                var role = _intranetApiContext.Roles.Find(id);

//                if (role == null)
//                {
//                    var error = Json("No role found with id: " + id);
//                    return NotFound(error);
//                }

//                _intranetApiContext.Remove(role);
//                _intranetApiContext.SaveChanges();

//                return Ok(id);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Add a new role.
//        /// </summary>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public IActionResult Post([FromBody] Role body)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var newRole = new Role
//                {
//                    Description = body.Description
//                };

//                _intranetApiContext.Roles.Add(newRole);
//                _intranetApiContext.SaveChanges();

//                return Ok(ModelState);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Change contents of a specific role.
//        /// Can be changed: Description.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpPut]
//        public IActionResult Put(int id, [FromBody] Role body)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var role = _intranetApiContext.Roles.Find(id);

//                if (role == null)
//                {
//                    var error = Json("No role found with id: " + id);
//                    return NotFound(error);
//                }

//                role.Description = body.Description;

//                _intranetApiContext.SaveChanges();

//                return Ok(ModelState);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }
//    }
//}
