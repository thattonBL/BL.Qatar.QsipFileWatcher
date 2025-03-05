using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RestSharp;

namespace BL.Qatar.QsipFileWatcher.Services;

public class ArkMinterService : IArkMinterService
{
    private readonly IConfiguration _config;
    private readonly ILogger<ArkMinterService> _logger;
    private string _url;
    private string _namespace;

    public ArkMinterService(IConfiguration config, ILogger<ArkMinterService> logger)
    {
        _config = config;
        _logger = logger;

        var hostname = _config["ArkMinter:Hostname"];
        if (String.IsNullOrEmpty(hostname))
        {
            _logger.LogError("There is no hostname provided for the ArkMinter service");
            return;
        }

        _url = CreateUrl(hostname);
        _namespace = _config["ArkMinter:Namespace"];
    }

    public List<string> GetArkList(int number = 1)
    {
        if (string.IsNullOrEmpty(_namespace))
            //throw new ArgumentException(@"Value cannot be null or empty.", "namespace");

            if (number <= 0)
                //throw new ArgumentException(@"Value must be greater than zero.", "number");

                if (number >= 1000000)
                    //throw new ArgumentException(@"Value must be less than 1,000,000.", "number");

                    _logger.LogDebug("BL.Dom.GenericIngest.Utilities.Minter.ArkMinter.GetArkList(namespace={0}, number={1})", _namespace, number);

        var arkList = new List<string>();

        var url = string.Format("{0}/{1}?arks={2}", _url, _namespace, number);
        _logger.LogDebug("Calling PII service at URL '{0}'.", url);

        var client = new RestClient(url);
        var request = new RestRequest { Method = Method.Post };

        // execute the request on the client
        var response = ExecuteRestAsync<Pii>(request, client).Result.Data;//   Execute<Pii>(request); 

        if (response.TheErrors.Count > 0)
        {
            var messages = new List<string>();

            foreach (var errors in response.TheErrors)
            {
                if (errors.TheErrors.Count > 0)
                {
                    messages.AddRange(errors.TheErrors.Select(error => String.Format("{0} - {1}", error.Code, error.Value)));
                }
            }

            _logger.LogError(String.Format("The following error(s) occurred calling the ARK minting service at '{0}'.", url), messages);
        }

        // get the ARK values from the response
        foreach (var arks in response.TheResults.SelectMany(result => result.TheArkList))
        {
            arkList.AddRange(arks.Arks.Select(a => a.Value).ToArray());
        }

        // return it
        return arkList;
    }

    private async Task<RestResponse<T>> ExecuteRestAsync<T>(RestRequest request, RestClient client) where T : class, new()
    {
        var response = await client.ExecuteAsync<T>(request);
        if (response.ErrorException != null)
        {
            const string message = "Error retrieving response.";
            throw new ApplicationException(message, response.ErrorException);
        }
        return response;
    }

    private string CreateUrl(string hostname)
    {
        return $"http://{hostname}/pii";
    }
}

internal class Pii
{
    public List<Results> TheResults { get; set; }

    public List<Errors> TheErrors { get; set; }
}

internal class Results
{
    public List<ArkList> TheArkList { get; set; }
}

internal class ArkList
{
    public List<Ark> Arks { get; set; }
}

internal class Ark
{
    public string Value { get; set; }
}

internal class Errors
{
    public List<Error> TheErrors { get; set; }
}

internal class Error
{
    [XmlAttribute]
    public string Code { get; set; }

    public string Value { get; set; }
}
