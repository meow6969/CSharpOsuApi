using System.Net;
using System.Text;
using System.Text.Json;
using CSharpOsuApi.Models;
using CSharpOsuApi.Models.BeatmapModels;
using CSharpOsuApi.Models.BeatmapModels.Metadata;
using CSharpOsuApi.Models.Http;
using CSharpOsuApi.Models.Http.Query;
using CSharpOsuApi.Models.OsuEnums;
using CSharpOsuApi.Models.UserModels;

// ReSharper disable MemberCanBePrivate.Global

namespace CSharpOsuApi;

public class OsuBackend
{
    public readonly int ClientId;
    public readonly string ClientSecret;
    public readonly Scopes Scopes;
    public const string OsuApiEndPoint = "https://osu.ppy.sh/api/v2";
    public readonly string? OsuSessionCookie;
    public HttpClient SharedClient;
    public OAuthTokenResponse OAuthToken;

    public OsuBackend(int clientId, string clientSecret, Scopes scopes, string? osuCookiesFilePath=null)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        Scopes = scopes;
        if (osuCookiesFilePath != null)
            OsuSessionCookie = UtilityFunctions.GetOsuSessionStringFromOsuCookiesFile(osuCookiesFilePath);
        OAuthToken = OsuAuth.GetOAuthTokenFromCode(clientId, clientSecret, scopes);
        SharedClient = SetupSharedClient();
    }

    public bool DownloadBeatmapset(Beatmapset beatmapset, params string[] saveDirectories)
    {
        if (OsuSessionCookie == null) throw new Exception("cannot download beatmapset without osuSessionCookie");
        
        string url = $"https://osu.ppy.sh/beatmapsets/{beatmapset.Id}/download";
        HttpResponseMessage? res = null;
        while (res == null)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9," +
                                                         "image/avif,image/webp,image/apng,*/*;q=0.8," +
                                                         "application/signed-exchange;v=b3;q=0.7");
                httpRequestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
                httpRequestMessage.Headers.Add("Cookie", $"osu_session={OsuSessionCookie}");
                httpRequestMessage.Headers.Add("Referer", $"https://osu.ppy.sh/beatmapsets/{beatmapset.Id}");
                httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 " + 
                                                             "(KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");
                
                res = SharedClient.Send(httpRequestMessage).EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Thread.Sleep(60000); // sleep for 1min
            }
        }
        
        if (res.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return false;
        }
        byte[] beatmapsetBytes = res.Content.ReadAsByteArrayAsync().Result;
        string fileName = $"{beatmapset.Id} {beatmapset.Artist} - {beatmapset.Title}.osz";
        string[] illegalCharacters =
        [
            "/",
            "\\",
            ":",
            "\"",
            "\'",
            "|",
            "?",
            "*"
        ];
        foreach (string illegalChar in illegalCharacters)
        {
            fileName = fileName.Replace(illegalChar, "");
        }
        string saveDirectoriesString = "";
        foreach (string saveDirectory in saveDirectories)
        {
            string savePath = Path.Combine(saveDirectory, fileName);
            if (File.Exists(savePath))
            {
                Console.WriteLine($"beatmap {savePath} already exists, skipping");
                continue;
            }
            saveDirectoriesString += savePath + ", ";
            File.WriteAllBytes(savePath, beatmapsetBytes);
        }
        Console.WriteLine($"{fileName} saved in these directories: {saveDirectoriesString}");
        return true;
    }

    public bool DownloadBeatmapset(BeatmapsetExtended beatmapset, params string[] saveDirectories)
    {
        return DownloadBeatmapset((Beatmapset)beatmapset, saveDirectories);
    }

    public BeatmapExtended GetBeatmap(int beatmapId)
    {
        return SendOsuApiRequest<BeatmapExtended>(HttpMethod.Get, $"/beatmaps/{beatmapId}");
    }
    
    public BeatmapsetExtended GetBeatmapset(int beatmapsetId)
    {
        return SendOsuApiRequest<BeatmapsetExtended>(HttpMethod.Get, $"/beatmapsets/{beatmapsetId}");
    }

    public BeatmapsetsSearchResponse SearchBeatmapsets(BeatmapsetsSearchRequest? request=null)
    {
        if (request != null)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(request);
            StringContent searchParameters = new StringContent(
                Encoding.UTF8.GetString(bytes), 
                Encoding.UTF8, 
                "application/json"
            );
            // Console.WriteLine(searchParameters.ReadAsStringAsync().Result);
            
            return SendOsuApiRequest<BeatmapsetsSearchResponse>(HttpMethod.Get, "/beatmapsets/search", 
                searchParameters);
        }
        
        return SendOsuApiRequest<BeatmapsetsSearchResponse>(HttpMethod.Get, "/beatmapsets/search");
    }
    
    public BeatmapPlaycount[] GetUserMostPlayedBeatmaps(User user, int limit=5, int offset=0)
    {
        return SendOsuApiRequest<BeatmapPlaycount[]>(
            HttpMethod.Get, 
            $"/users/{user.Id}/beatmapsets/most_played?limit={limit}&offset={offset}"
        );
    }

    private BeatmapsetExtended[] GetUserBeatmaps(User user, BeatmapType type, int limit=5, int offset=0)
    {
        return SendOsuApiRequest<BeatmapsetExtended[]>(
            HttpMethod.Get, 
            $"/users/{user.Id}/beatmapsets/{type.Description()}?limit={limit}&offset={offset}"
            );
    }
    
    public BeatmapPlaycount[] GetAllUserPlayedBeatmaps(User user)
    {
        int limit = 100;
        int offset = 0;
        List<BeatmapPlaycount> playedBeatmaps = [];
        while (true)
        {
            BeatmapPlaycount[] maps = GetUserMostPlayedBeatmaps(user, limit, offset);
            playedBeatmaps.AddRange(maps);
            if (maps.Length == 0) break;
            offset += limit;
        }
        
        return playedBeatmaps.ToArray();
    }

    // TODO: add the thing here im to lazy rn
    public User GetMe()
    {
        // /me/{mode?}
        return SendOsuApiRequest<User>(HttpMethod.Get, "/me");
    }

    public User GetUser(int userId, RulesetEnum? mode = null)
    {
        string queryString = "?key=id";
        if (mode != null) queryString = $"/{((RulesetEnum)mode).OsuApiName()}{queryString}";
        return SendOsuApiRequest<User>(HttpMethod.Get, $"/users/{userId}{queryString}");
    }

    private T SendOsuApiRequest<T>(HttpMethod httpMethod, string endpoint, HttpContent? httpContent=null)
    {
        // we add 60 seconds here just to be sure
        if (OAuthToken.ExpiresAt.AddSeconds(-60) < DateTime.Now)
        {
            OAuthToken = OsuAuth.GetNewAccessToken(ClientId, ClientSecret, OAuthToken.RefreshToken);
        }
        
        string url = $"{OsuApiEndPoint}{endpoint}";
        // Console.WriteLine($"sending to url {url}");
        HttpRequestMessage osuApiRequest = 
            GetNewHttpRequestMessage(httpMethod, url);
        if (httpContent != null) osuApiRequest.Content = httpContent;
        
        // UtilityFunctions.PrintAllHeaders(osuApiRequest.Headers);
        HttpResponseMessage osuApiResponse = SharedClient.Send(osuApiRequest);
        while (true)
        {
            // if the response has an error we pass it to the serializer in case osu returned an error message response
            if (osuApiResponse.StatusCode != HttpStatusCode.TooManyRequests) break;
            Console.WriteLine("we are being rate limited! waiting 5s...");
            Thread.Sleep(5000);
            osuApiResponse = SharedClient.Send(osuApiRequest);
        }

        // string content = new StreamReader(osuApiResponse.Content.ReadAsStream()).ReadToEnd();
        
        try
        {
            return UtilityFunctions.SerializeHttpResponseMessage<T>(osuApiResponse);
        }
        catch (JsonException e)
        {
            throw HandleOsuApiHttpJsonError(e, osuApiRequest, osuApiResponse, endpoint);
        }
    }

    public static Exception HandleOsuApiHttpJsonError(JsonException e, HttpRequestMessage osuApiRequest, 
        HttpResponseMessage osuApiResponse, string endpoint)
    {
        if (osuApiResponse.StatusCode == HttpStatusCode.NotFound)
        {
            throw new OsuErrors.OsuHttpError($"osu api was not able to find the request for {endpoint}");
        }

        try
        {
            OsuErrors.OsuErrorGeneric requestError =
                UtilityFunctions.SerializeHttpResponseMessage<OsuErrors.OsuErrorGeneric>(osuApiResponse, osuApiRequest,
                    osuApiResponse);
            requestError.Message = GetGenericOsuErrorMessage(requestError);
            throw requestError;
        }
        catch (JsonException e2)
        {
            try
            {
                OsuErrors.OsuAuthenticationError requestError =
                    UtilityFunctions.SerializeHttpResponseMessage<OsuErrors.OsuAuthenticationError>(osuApiResponse, osuApiRequest,
                        osuApiResponse);
                throw requestError;
            }
            catch 
            {
                // this a error i havent seen before
                throw e2;
            }
        }
    }

    private static string GetGenericOsuErrorMessage(OsuErrors.OsuErrorGeneric osuError)
    {
        string text = "";
        if (osuError.OriginalHttpRequest != null)
        {
            text += 
                $"Error in request to URI: {osuError.OriginalHttpRequest.RequestUri}\n" +
                $"Headers: {{\n" +
                $"{UtilityFunctions.GetAllRequestHeadersString(osuError.OriginalHttpRequest.Headers, ",\n")}" +
                $"}}\n" +
                $"Error attributes: {{\n" +
                $"{UtilityFunctions.GetAllAttributeValuesText(osuError, ",\n")}" +
                $"}}\n";
        }

        return text;
    }
    
    private HttpClient SetupSharedClient()
    {
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(OsuApiEndPoint);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("client_id", ClientId.ToString());
        httpClient.DefaultRequestHeaders.Add("client_secret", ClientSecret);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OAuthToken.AccessToken}");
        return httpClient;
    }

    private HttpRequestMessage GetNewHttpRequestMessage(HttpMethod httpMethod, string url)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
        
        foreach (var header in SharedClient.DefaultRequestHeaders)
        {
            string valueText = "";
            foreach (string thing in header.Value)
            {
                valueText += $"{thing}";
            }
            httpRequestMessage.Headers.Add(header.Key, valueText);
        }

        return httpRequestMessage;
    }
}
