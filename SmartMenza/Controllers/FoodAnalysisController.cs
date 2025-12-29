using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models;
using SmartMenza.Business.Models.FoodAnalysis;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.Api.Controllers;

[ApiController]
[Route("api/food")]
public sealed class FoodAnalysisController : ControllerBase
{
    private readonly IFoodAnalyzer _analyzer;

    public FoodAnalysisController(IFoodAnalyzer analyzer)
    {
        _analyzer = analyzer;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<FoodAnalysisResult>> Analyze([FromBody] AnalyzeFoodRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Text))
            return BadRequest("Text is required.");

        var result = await _analyzer.AnalyzeAsync(req.Text, ct);
        return Ok(result);
    }
}
