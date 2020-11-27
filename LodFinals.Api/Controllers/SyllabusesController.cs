using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LodFinals.Api.Models;
using AutoMapper;
using DataModel.Responses;
using DataModel.Requests;

namespace LodFinals.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusesController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public SyllabusesController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Syllabus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SyllabusResponse>>> GetSyllabus()
        {
            return _mapper.Map<IEnumerable<SyllabusResponse>>(await _context.Syllabus.ToListAsync()).ToList();
        }

        // GET: api/Syllabus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SyllabusResponse>> GetSyllabus(int id)
        {
            var syllabus = await _context.Syllabus.FindAsync(id);

            if (syllabus == null)
            {
                return NotFound();
            }

            return _mapper.Map<SyllabusResponse>(syllabus);
        }

        // PUT: api/Syllabus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSyllabus(int id, SyllabusRequest request)
        {
            var syllabus = _mapper.Map<Syllabus>(request);
            if (id != syllabus.Id)
            {
                return BadRequest();
            }

            _context.Entry(syllabus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SyllabusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Syllabus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SyllabusResponse>> PostSyllabus(SyllabusRequest request)
        {
            var syllabus = _mapper.Map<Syllabus>(request);
            _context.Syllabus.Add(syllabus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSyllabus", new { id = syllabus.Id }, syllabus);
        }

        // DELETE: api/Syllabus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SyllabusResponse>> DeleteSyllabus(int id)
        {
            var syllabus = await _context.Syllabus.FindAsync(id);
            if (syllabus == null)
            {
                return NotFound();
            }

            _context.Syllabus.Remove(syllabus);
            await _context.SaveChangesAsync();

            return _mapper.Map<SyllabusResponse>(syllabus);
        }

        private bool SyllabusExists(int id)
        {
            return _context.Syllabus.Any(e => e.Id == id);
        }
    }
}
