using Application.Contracts;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/reports")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("sales")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Генерация отчета по продажам за указанный период. Поддерживает фильтрацию по издателям, жанрам и авторам.")]
    [EndpointSummary("Получить отчет по продажам")]
    public async Task<ActionResult<SalesReportResponse>> GetSalesReport([FromQuery] ReportFilters filters)
    {
        var result = await _reportService.GenerateSalesReportAsync(filters);

        return Ok(result);
    }
}
