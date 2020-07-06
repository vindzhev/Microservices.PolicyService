namespace PolicyService.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Mvc;
    
    using MicroservicesPOC.Shared.Controllers;
    using MicroservicesPOC.Shared.Common.Models;
    
    using PolicyService.Application.Policy.Queries;
    using PolicyService.Application.Policy.Commands;

    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<PolicyDTO>> Post([FromBody] CreatePolicyCommand data)
        {
            Guid policyId = await this.Mediator.Send(data);
            PolicyDTO policy = await this.Mediator.Send(new GetPolicyDetailsQuery(policyId));

            return this.CreatedAtRoute("GetPolicyDetails", new { policyNumber = policyId }, policy);
        }

        [HttpHead]
        [HttpGet("{policyNumber}", Name = "GetPolicyDetails")]
        public async Task<ActionResult<PolicyDTO>> Get(Guid policyNumber) => 
            this.Ok(await this.Mediator.Send(new GetPolicyDetailsQuery(policyNumber)));

        [HttpDelete("{policyNumber:Guid}")]
        public async Task<ActionResult> Delete([FromQuery]Guid policyNumber, [FromBody]TerminatePolicyCommand data)
        {
            data.PolicyNumber = policyNumber;
            await this.Mediator.Send(data);

            return NoContent();
        }
    }
}
