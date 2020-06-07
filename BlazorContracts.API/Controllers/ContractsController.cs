using BlazorContracts.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BlazorContracts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private static readonly List<Contract> contracts = GenerateContracts(10);

        private static List<Contract> GenerateContracts(int number)
        {
            return Enumerable.Range(1, number).Select(index => new Contract
            {
                Id = index,
                Name = $"First{index} Last{index}",
                PhoneNumber = $"+1 555 987{index}",
            }).ToList();
        }

        [HttpGet]
        public ActionResult<List<Contract>> GetAllContracts()
        {
            return contracts;
        }

        // GET: /api/contracts/4
        [HttpGet("{id}")]
        public ActionResult<Contract> GetContractById(int id)
        {
            var contract = contracts.FirstOrDefault(s => s.Id == id);
            if (contract == null) return NotFound();
            return contract;
        }

        // POST: /api/contracts
        [HttpPost]
        public void AddContract([FromBody] Contract contract)
        {
            contracts.Add(contract);
        }

        // PUT: /api/contracts/4
        [HttpPut("{id}")]
        public void EditContract(int id, [FromBody] Contract contract)
        {
            var idx = contracts.FindIndex(p => p.Id == id);
            if (idx >= 0)
                contracts[idx] = contract;
        }

        // DELETE: /api/contract/4
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var idx = contracts.FindIndex(s => s.Id == id);
            if (idx >= 0)
                contracts.RemoveAt(idx);
        }
    }
}