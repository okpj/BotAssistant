using BotAssistant.Application.Contract.Model.Recognize;

namespace BotAssistant.Application.Contract
{
    public interface IVoiceRecognizeService
    {
        Task<RecognizeResult> RecognizeShortMassageAsync(byte[] voiceByte);

        Task<RecognizeResult> RecognizeLongMassageAsync(Stream voiceStream, string voiceFileName);
    }
}
