namespace PolicyService.API.Controllers
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Mvc;
    
    using MicroservicesPOC.Shared.API.Controllers;
    
    using PolicyService.Application.Offer.Commands;

    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateOfferCommand data) => this.Ok(await this.Mediator.Send(data));
    }
}
