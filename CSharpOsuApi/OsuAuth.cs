using System.Text.Json;
using CSharpOsuApi.JsonUtils;
using CSharpOsuApi.Models;
using CSharpOsuApi.Models.Http.Query;

namespace CSharpOsuApi;

public static class OsuAuth
{
    private const string OAuthTokenFileName = "OAuthToken.json";

    public static OAuthTokenResponse GetOAuthTokenFromCode(
        int clientId, string clientSecret, Scopes scope)
    {
        OAuthTokenResponse? oAuthFile = RetrieveOAuthTokenFile();
        if (oAuthFile != null)
        {
            // Console.WriteLine($"ExpiresAt: {oAuthFile.ExpiresAt.ToLongTimeString()}");
            // Console.WriteLine($"DateTime.Today: {DateTime.Now.ToLongTimeString()}");
            // throw new Exception();
            if (oAuthFile.ExpiresAt > DateTime.Now)  // if the token hasnt expired yet
            {
                Console.WriteLine("using oAuthToken from file");
                // Console.WriteLine(oAuthFile.AccessToken);
                return oAuthFile;
            }
            
            Console.WriteLine("refreshing oAuthToken using refresh_token");

            return GetNewAccessToken(clientId, clientSecret, refreshToken: oAuthFile.RefreshToken);
        }
        Console.WriteLine("getting new oAuthToken");

        return GetNewAccessToken(clientId, clientSecret, scope: scope);
    }

    public static OAuthTokenResponse GetNewAccessToken(
        int clientId, string clientSecret, string? refreshToken=null, Scopes? scope=null)
    {
        HttpClient authClient = new HttpClient();
        
        List<KeyValuePair<string, string>> requestForm =
        [
            new KeyValuePair<string, string>("client_id", clientId.ToString()),
            new KeyValuePair<string, string>("client_secret", clientSecret),
        ];
        if (refreshToken != null)
        {
            requestForm.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));
            requestForm.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
        }
        else
        {
            if (scope is null) throw new NullReferenceException();
            string oAuthCode = GetOAuthCode(clientId, scope);
            requestForm.Add(new KeyValuePair<string, string>("code", oAuthCode));
            requestForm.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
        }
        if (scope != null) requestForm.Add(new KeyValuePair<string, string>("scope", scope.ToString()));
        
        
        HttpRequestMessage tokenRequest = 
            new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");
        tokenRequest.Content = new FormUrlEncodedContent(requestForm);
        tokenRequest.Headers.Add("Accept", "application/json");
        
        // UtilityFunctions.PrintAllHeaders(tokenRequest.Headers);
        // UtilityFunctions.PrintAllHeaders(tokenRequest.Content.Headers);
        // foreach (KeyValuePair<string, string> thing in requestForm)
        // {
        //     Console.WriteLine($"{thing.Key}: {thing.Value}");
        // }
        
        HttpResponseMessage tokenResponse = authClient.Send(tokenRequest);
        OAuthTokenResponse theToken;
        try
        {
            theToken = UtilityFunctions.SerializeHttpResponseMessage<OAuthTokenResponse>(tokenResponse);
        }
        catch (JsonException e)
        {
            OsuBackend.HandleOsuApiHttpJsonError(e, tokenRequest, tokenResponse, 
                "https://osu.ppy.sh/oauth/token");
            throw;
        }
        
        
        theToken.ExpiresAt = DateTime.Now.AddSeconds(theToken.ExpiresIn);
        theToken.ExpiresAtUnixTime = ((DateTimeOffset)theToken.ExpiresAt).ToUnixTimeSeconds();
        
        SaveOAuthTokenToFile(theToken);
        
        return theToken;
    }
    
    private static string GetOAuthCode(int clientId, Scopes? scope)
    {
        // open up the page in the users browser
        string oAuthUrl = $"https://osu.ppy.sh/oauth/authorize?client_id={clientId}&" +
                          $"response_type=code";
        if (scope != null) oAuthUrl += $"&scope={scope}";
        
        Console.WriteLine(
            $"hello! please go to this link in your browser: \n" +
            $"{oAuthUrl}\n" +
            $"after you have authorized the application, please paste in the URL of the page it redirected you to.");
        string redirectUrl = Console.ReadLine() ?? throw new NullReferenceException();

        return GetOAuthCodeFromUrl(redirectUrl);
    }
    
    private static void SaveOAuthTokenToFile(OAuthTokenResponse theOAuthToken)
    {
        if (File.Exists(OAuthTokenFileName))
        {
            File.Delete(OAuthTokenFileName);
        }

        using FileStream createStream = File.Create(OAuthTokenFileName);
        JsonSerializer.Serialize(createStream, theOAuthToken, JsonOptions.PrettyPrint);
    }

    private static OAuthTokenResponse? RetrieveOAuthTokenFile()
    {
        try
        {
            using StreamReader r = new StreamReader(OAuthTokenFileName);
            string json = r.ReadToEnd();
            OAuthTokenResponse parsedJson = JsonSerializer.Deserialize<OAuthTokenResponse>(json) 
                                                       ?? throw new Exception();
            parsedJson.ExpiresAt = UtilityFunctions.UnixTimeStampToDateTime(parsedJson.ExpiresAtUnixTime);
            return parsedJson;
        }
        catch // the file couldnt be found or is corrupt or something
        {
            return null;
        }
    }
    
    private static string GetOAuthCodeFromUrl(string url)
    {
        char[] stopChars = ['?', '&', '='];
        string currentToken = "";
        bool seekingForCode = false;
        foreach (char letter in url)
        {
            if (stopChars.Contains(letter))
            {
                if (currentToken == "code" && !seekingForCode)
                {
                    seekingForCode = true;
                }
                else if (seekingForCode)  // we got the whole code now
                {
                    return currentToken;
                }
                else
                {
                    seekingForCode = false;
                }

                currentToken = "";
                continue;
            }
            currentToken += letter;
        }
        
        // if we got here then we either couldnt find the code in the url or the code was the last part of the url
        if (currentToken.Length == 0)
            throw new Exception($"Could not find code in given url:\n" +
                                $"{url}");
        return currentToken;
    }
}