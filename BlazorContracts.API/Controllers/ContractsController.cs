using BlazorContracts.API.Data;
using BlazorContracts.Shared.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContracts.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    [ODataRoutePrefix("contracts")]
    public class ContractsController : ODataController
    {
        private readonly ContractsDbContext _context;

        public ContractsController(ContractsDbContext context)
        {
            _context = context;
        }

        [EnableQuery(PageSize = 50)]
        [ODataRoute]
        public IQueryable<Contract> Get()
        {
            return _context.Contracts;
        }

        // GET: /api/contracts/4
        // [HttpGet("{id}")]
        [EnableQuery]
        [ODataRoute("({id})")]
        public async Task<IActionResult> GetContract([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        // POST: /api/contracts
        //[HttpPost]
        [ODataRoute]
        public async Task<IActionResult> Post([FromBody] Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Contracts.Add(contract);

            await _context.SaveChangesAsync();

            return Created(contract.Id.ToString(), contract);
        }

        // PATCH: /api/contracts/4
        // [HttpPut("{id}")]
        [ODataRoute("({id})")]
        public async Task<IActionResult> Patch([FromODataUri] int id, [FromBody] Delta<Contract> updatedContract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            updatedContract.Patch(contract);

            try
            {
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

            return Updated(contract);
        }

        // DELETE: /api/contract/4
        // [HttpDelete("{id}")]
        [ODataRoute("({id})")]
        public async Task<IActionResult> Delete([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound(contract);
            }

            _context.Contracts.Remove(contract);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}