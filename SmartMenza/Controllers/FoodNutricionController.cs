using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.FoodAnalysis;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers;

[ApiController]
[Route("api/Nutrition")]
public sealed class NutritionController : ControllerBase
{
    private readonly IAINutritionAnalyzerService _nutrition;

    public NutritionController(IAINutritionAnalyzerService nutrition)
    {
        _nutrition = nutrition;
    }

    [HttpPost("analysis")]
    public async Task<ActionResult<NutritionResult>> AnalyzeNutrition([FromBody] AnalyzeNutritionRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Text))
            return BadRequest("Text is required.");

        var result = await _nutrition.AnalyzeAsync(req.Text, ct);
        return Ok(result);
    }
}
