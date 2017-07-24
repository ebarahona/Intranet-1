using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Extensions;
using Intranet.Shared.Factories;
using Intranet.API.Helpers;
using Intranet.API.ViewModels;
using Intranet.Shared.Extensions;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage news items.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class FaqController : Controller, IRestControllerAsync<Faq>
    {
        private readonly IntranetApiContext _context;

        public FaqController(IntranetApiContext context)
        {
            _context = context;
        }

        [Authorize("IsAdmin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var faq = await _context.Faqs
                    .Include(f => f.Category)
                        .ThenInclude(c => c.Faqs)
                    .SingleOrDefaultAsync(f => f.Id == id);

                if (faq.IsNull())
                {
                    return NotFound();
                }

                if (faq.Category.HasNoRelatedEntities(faq))
                {
                    _context.Remove(faq.Category);
                }

                _context.Remove(faq);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var faqs = await _context.Faqs
                    .Include(f => f.Category)
                        .ThenInclude(c => c.Faqs)
                    .ToListAsync();

                if (faqs.IsNull())
                {
                    faqs = new List<Faq>();
                }

                return Ok(faqs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var faq = await _context.Faqs
                    .Include(f => f.Category)
                        .ThenInclude(c => c.Faqs)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (faq.IsNull())
                {
                    return NotFound();
                }

                return Ok(faq);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize("IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Faq faq)
        {
            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Title.Equals(faq.Category.Title, StringComparison.OrdinalIgnoreCase));

                if (category.IsNotNull())
                {
                    faq.Category = category;
                }
                else
                {
                    faq.Category.Url = UrlHelper.URLFriendly(faq.Category.Title);
                }

                faq.Url = UrlHelper.URLFriendly(faq.Question);

                await _context.AddAsync(faq);

                await _context.SaveChangesAsync();

                return Ok(faq);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize("IsAdmin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Faq faq)
        {
            try
            {
                var entity = await _context.Faqs
                    .Include(f => f.Category)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                if (!entity.Category.Title.Equals(faq.Category.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var category = await _context.Categories.SingleOrDefaultAsync(c => c.Title.Equals(faq.Category.Title, StringComparison.OrdinalIgnoreCase));

                    if (category.IsNotNull())
                    {
                        entity.Category = category;
                    }
                    else
                    {
                        entity.Category = new Category
                        {
                            Title = faq.Category.Title,
                            Url = UrlHelper.URLFriendly(faq.Category.Title),
                        };
                    }
                }

                entity.Answer = faq.Answer;
                entity.Question = faq.Question;

                await _context.SaveChangesAsync();

                return Ok(entity);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
