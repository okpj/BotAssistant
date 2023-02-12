namespace BotAssistant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TelegramController : ControllerBase
{
    private readonly IOptions<ApiOptions> _apiOptions;
    private readonly IUpdateService _updateService;
    public TelegramController(
        IOptions<ApiOptions> apiOptions,
        IUpdateService updateService)
    {
        _apiOptions = apiOptions;
        _updateService = updateService;
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromQuery] string AuthToken, [FromBody] Update update)
    {
        if (AuthToken != _apiOptions.Value.AuthToken)
            return Unauthorized();

        await _updateService.Handle(update);

        return Ok();
    }


}
