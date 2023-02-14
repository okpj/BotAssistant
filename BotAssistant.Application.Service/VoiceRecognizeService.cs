namespace BotAssistant.Application.Service;

public sealed class VoiceRecognizeService : IVoiceRecognizeService
{
    private readonly IYandexSpeechService _yandexSpeechService;
    private readonly IYandexObjectService _yandexObjectService;

    public VoiceRecognizeService(IYandexSpeechService yandexSpeechService,
        IYandexObjectService yandexObjectService)
    {
        _yandexObjectService = yandexObjectService;
        _yandexSpeechService = yandexSpeechService;
    }

    public async Task<RecognizeResult> RecognizeShortMassageAsync(byte[] voiceByte)
    {
        var yandexRecognizeResult = await _yandexSpeechService.RecognizeAsync(voiceByte);
        return new RecognizeResult { Text = yandexRecognizeResult?.Result };
    }


    public async Task<RecognizeResult> RecognizeLongMassageAsync(Stream voiceStream, string voiceFileName)
    {
        string? text = string.Empty;
        var filePutResult = await _yandexObjectService.Put(voiceStream, voiceFileName);
        if (filePutResult)
        {
            var operation = await _yandexSpeechService.StartLongRecognizeTaskAsync(voiceFileName);
            if (string.IsNullOrEmpty(operation?.Id) is false)
            {
                var yandexRecognizeResult = await _yandexSpeechService.GetLongRecognizeResultAsync(operation.Id);
                text = yandexRecognizeResult?.Result;
            }
        }

        return new RecognizeResult { Text = text };

    }
}
