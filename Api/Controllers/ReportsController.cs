using Application.Dto.Request.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        [HttpGet("sales")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [EndpointDescription("Генерация отчета по продажам за указанный период. Поддерживает фильтрацию по издателям, жанрам и авторам.")]
        [EndpointSummary("Получить отчет по продажам")]
        public IActionResult GetSalesReport([FromQuery] ReportFilters filters)
        {
            // Generate sales reports (admin only)
            return Ok();
        }
    }
}
