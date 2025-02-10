using System.Text.Json.Serialization;
// ReSharper disable ClassNeverInstantiated.Global

namespace CSharpOsuApi.Models;

public class OsuErrors
{
    public abstract class OsuError : Exception
    {
        public OsuError()
        {
            
        }

        public OsuError(string message) 
            : base(message)
        {
            
        }
    }

    public class OsuHttpError : OsuError
    {
        public HttpRequestMessage? OriginalHttpRequest { get; set; }
        public HttpResponseMessage? OriginalHttpResponse { get; set; }
        
        public OsuHttpError()
        {
            
        }

        public OsuHttpError(string message) 
            : base(message)
        {
            
        }
    }
    
    // im pretty sure most osu api error jsons are like this 
    public class OsuErrorGeneric : OsuHttpError
    {
        [JsonPropertyName("error")]
        public required string Error { get; init; }
        [JsonPropertyName("error_description")]
        public required string ErrorDescription { get; init; }
        [JsonPropertyName("hint")]
        public required string Hint { get; init; }
        [JsonPropertyName("message")]
        public required string OsuResponseMessage { get; init; }
        
        public new string? Message { get; set; }
    }

    // this happens when u try to call a api thing and u dont put in the token or the token is too old
    public class OsuAuthenticationError : OsuHttpError
    {
        // i think this can only be "basic" and "verify" ??
        [JsonPropertyName("authentication")]
        public required string Authentication { get; init; }

        public OsuAuthenticationError() : base("Authentication Error")
        {
            
        }
    }
}