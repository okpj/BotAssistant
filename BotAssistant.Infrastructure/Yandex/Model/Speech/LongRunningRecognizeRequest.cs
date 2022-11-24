﻿namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class LongRunningRecognizeRequest
{
    [JsonPropertyName("config")]
    public Config Config { get; set; }

    [JsonPropertyName("audio")]
    public Audio Audio { get; set; }
}


