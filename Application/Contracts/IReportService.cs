using Application.Dto.Request.Filters;
using Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IReportService
{
    Task<SalesReportResponse> GenerateSalesReportAsync(ReportFilters filters);
}
