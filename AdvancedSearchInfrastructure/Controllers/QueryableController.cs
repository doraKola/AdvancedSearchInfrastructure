using AdvancedSearchInfrastructure.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace AdvancedSearchInfrastructure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryableController : ControllerBase
    {
        private readonly IQueryableService _queryableService;

        public QueryableController(IQueryableService queryableService)
        {
            _queryableService = queryableService;
        }

        [HttpPost("SetContent")]
        public string SetContent([FromBody] RequestModel model)
        {
            if (model.Content == null)
                return string.Empty;
            HttpContext.Session.SetString("Content", model.Content);
            return HttpContext.Session.GetString("Content");
        }

        [HttpPost(Name = "Queryable")]
        public string Post(string query)
        {
            if (query == null) return "no query";
            var content = HttpContext.Session.GetString("Content");
            if (content == null) return "no content";
            _queryableService.SetQueryable(query);
            return Regex.IsMatch(content, _queryableService.DynamicPattern, RegexOptions.IgnoreCase) ? "The pattern matched in the sentence." : "The pattern did not match in the sentence.";
        }
    }

    public class RequestModel
    {
        /// <summary>
        /// Multi-line text area.
        /// </summary>
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string Content { get; set; }
    }
}