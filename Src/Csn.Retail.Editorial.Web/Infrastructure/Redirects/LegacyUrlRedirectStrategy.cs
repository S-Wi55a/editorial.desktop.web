using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Listings;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Parser;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Binary;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public class EditorialMetadataDto
    {
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        // ReSharper disable once InconsistentNaming - needed for Ryvus response parsing
        public string query { get; set; }

        public string Seo { get; set; }
        public string Title { get; set; }
    }


    public class RedirectionDto
    {
        public RedirectInstruction RedirectInstructions { get; set; }

        public bool Redirect { get; set; }
    }

    public class ParserDto
    {
        public IExpressionParser Parser { get; set; }

        public RyvussSyntax Syntax { get; set; }
    }

    public enum RyvussSyntax
    {
        V2,
        V4,
        Seo,
        NotSure = 99
    }

    [AutoBind]
    public class LegacyUrlRedirectStrategy : IRedirectStrategy
    {
        private readonly ISmartServiceClient _client;
        private const string ServiceName = "api-search-editorials";
        private readonly IDetailsRedirectLogger _redirectLogger;

        public LegacyUrlRedirectStrategy(ISmartServiceClient client, IDetailsRedirectLogger redirectLogger)
        {
            _client = client;
            _redirectLogger = redirectLogger;
        }

        public RedirectInstruction Apply(ActionExecutingContext filterContext)
        {
            var originalQuery = filterContext.HttpContext.Request?.QueryString["q"];

            if (originalQuery != null && filterContext.RequestContext?.HttpContext?.Request?.Url != null &&
                IsBinary(originalQuery))
            {
                return new RedirectInstruction
                {
                    Url = GetRedirectUrl(originalQuery),
                    IsPermanent = true
                };
            }

            return RedirectInstruction.None;
        }

        public int Order => 1;

        public RyvussSyntax GetRyvussSyntax(string query)
        {
            var availableParsers = new ParserDto[]
            {
                new ParserDto()
                {
                    Syntax = RyvussSyntax.V2,
                    Parser = new FlatBinaryTreeParser(new BinaryTreeSanitiser())
                },
                new ParserDto() {Syntax = RyvussSyntax.V4, Parser = new RoseTreeParser(new RoseTreeSanitiser())},
            };

            foreach (var parserDto in availableParsers)
            {
                try
                {
                    var parsed = parserDto.Parser.Parse(query);
                    return parserDto.Syntax;
                }
                catch (Exception ex)
                {
                    //Try next
                }
            }
            return RyvussSyntax.NotSure;
        }


        public bool IsBinary(string query)
        {
            var parser = new FlatBinaryTreeParser(new BinaryTreeSanitiser());
            try
            {
                var parsed = parser.Parse(query);
                return true;
            }
            catch
            {
                //An exception shows it isn't parsed by binary - therefore is V4
            }
            return false;
        }

        public string GetRedirectUrl(string query)
        {
            return "";
        }

        /*public RedirectionDto GetRedirectionInstructions(ActionExecutingContext filterContext, string query)
        {
            var redirectionInstructions = new RedirectionDto() {Redirect = false};

            var response = _client.Service(ServiceName)
                .Path("v4/editoriallisting")
                .QueryString("q", query)
                .QueryString("metadata", "")
                .Get<EditorialMetadataDto>();

            if (response.IsSucceed)
            {
                if (response.Data.Metadata.Seo.IsNullOrEmpty())
                {
                    //Create URL with Seo
                    _redirectLogger.Log($"{query} redirected to {response.Data.Metadata.Seo}");
                    return new RedirectionDto()
                        {
                            Redirect = true,
                            RedirectInstructions = new RedirectInstruction
                            {
                                Url = response.Data.Metadata.Seo,
                                IsPermanent = true
                            }
                        }
                        ;
                }
                else if (response.Data.Metadata.query.IsNullOrEmpty())
                {
                    //Create URL with Metadata
                    _redirectLogger.Log($"{query} redirected to {response.Data.Metadata.query}");
                    return new RedirectionDto()
                        {
                            Redirect = true,
                            RedirectInstructions = new RedirectInstruction
                            {
                                Url = filterContext.RequestContext?.HttpContext?.Request?.Url?.PathAndQuery.Replace(
                                    query,
                                    response.Data.Metadata.query),
                                IsPermanent = true
                            }
                        }
                        ;
                }
            }
            else
            {
                //TODO: ??
            }


            return redirectionInstructions;
        }*/
    }
}