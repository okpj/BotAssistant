namespace BotAssistant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TelegramController : ControllerBase
{
    private readonly IOptions<ApiOptions> _apiOptions;
    private readonly IHandleUpdateService _handleUpdateService;
    public TelegramController(
        IOptions<ApiOptions> apiOptions,
        IHandleUpdateService handleUpdateService)
    {
        _apiOptions = apiOptions;
        _handleUpdateService = handleUpdateService;
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromQuery] string AuthToken, [FromBody] Update update)
    {
        if (AuthToken != _apiOptions.Value.AuthToken)
            return Unauthorized();

        await _handleUpdateService.Handle(update);

        return Ok();
    }


}
