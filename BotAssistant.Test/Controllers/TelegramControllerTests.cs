namespace BotAssistant.Test.Controllers;

public class TelegramControllerTests
{
    Mock<IOptions<ApiOptions>> _mockApiOptions;
    Mock<IHandleUpdateService> _mockHandleUpdateService;
    Telegram.Bot.Types.Update _defaultUpdate;

    public TelegramControllerTests()
    {
        _mockApiOptions = new Mock<IOptions<ApiOptions>>();
        _mockApiOptions.Setup(opt => opt.Value).Returns(new ApiOptions { AuthToken = "secret-token", BaseUrl = "api-url" });

        _defaultUpdate = new Telegram.Bot.Types.Update();
        _mockHandleUpdateService = new Mock<IHandleUpdateService>();
        _mockHandleUpdateService.Setup(opt => opt.Handle(_defaultUpdate)).Returns(new Task(() => { }));
    }

    #region Telegram
    [Fact]
    public async void UpdateReturnsUnauthorizedError()
    {

        var controller = new TelegramController(_mockApiOptions.Object, _mockHandleUpdateService.Object);
        var result = await controller.Update("token", new Telegram.Bot.Types.Update());
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async void UpdateReturnsOkResult()
    {

        var controller = new TelegramController(_mockApiOptions.Object, _mockHandleUpdateService.Object);
        var result = await controller.Update("secret-token", _defaultUpdate);
        Assert.IsType<OkResult>(result);
    }
    #endregion
}