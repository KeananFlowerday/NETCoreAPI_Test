using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml;
using Microsoft.Extensions.Configuration;
using NET.API.Models;
using In = NET.API.Models.Incoming ;
using Out = NET.API.Models.Output;

using Saxon.Api;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace NET.API.Controllers
{
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [Route("[controller]")]
    public class SalesOrderController : Controller
    {
        private IConfiguration configuration;
        private readonly ILogger<SalesOrderController> _logger;

        public SalesOrderController(IConfiguration iConfig,  ILogger<SalesOrderController> logger)
        {
            configuration = iConfig;
            _logger = logger;
        }

        [HttpPost]
        [Route($"{Constants.ORDERSROUTE}AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] In.Root order)
        {
            try
            {
                bool noFileExtension = false;
                var xmlOrder = SaxxonTransform(ConvertJsonToXML(JsonConvert.SerializeObject(order)), "C:\\Users\\USER\\source\\repos\\NET.API\\XSLT\\SalesOrder.xslt");

                //"C:\\Users\\USER\\source\\repos\\NET.API\\XSLT\\SalesOrder.xslt"

                return Ok(order.SalesOrderRequest.SalesOrder.salesOrderRef);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error ,Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        private string SaxxonTransform(byte[] fileArray, string filepath)
        {
           

            var mapString = string.Empty;

            var s = System.IO.File.ReadAllBytes(filepath);
            mapString = Encoding.UTF8.GetString(s);
            Stream streamFile = new MemoryStream(fileArray);

            var streamTransform = new MemoryStream(Encoding.UTF8.GetBytes(mapString));

            XmlReader xmlReader1 = XmlReader.Create(streamFile);

            var xsltProcessor = new Processor();
            var documentBuilder = xsltProcessor.NewDocumentBuilder();
            var xdmNode = documentBuilder.Build(xmlReader1);

            var xsltCompiler = xsltProcessor.NewXsltCompiler();
            var xsltExecutable = xsltCompiler.Compile(streamTransform);
            var xsltTransformer = xsltExecutable.Load();
            xsltTransformer.InitialContextNode = xdmNode;

            var results = new XdmDestination();

            xsltTransformer.Run(results);

            return results.XdmNode.OuterXml.ToString();
        }
        private byte[] ConvertJsonToXML(string jsonData)
        {
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonData, Constants.ROOT_TAG);
            var xmlstr = doc.OuterXml;

            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }
    }
}
