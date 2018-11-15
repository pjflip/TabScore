using System.Collections.Generic;
using System.Web.Http;

namespace TabScoreApi.Controllers
{
    public class Dv : ApiController
    {
        // GET api/<controller>
        [Route("api/v1/bridge-event")]
        public IHttpActionResult Get()
        {
            // read path from database
            var path = "myPath";

            // You wouldn't do this here as it's calling it's self
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new System.Uri("http://198.0.162.0/");
                var response = client.GetAsync("api/v1/bridge-event");
            };
            
            return Ok(path);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("api/v1/bridge-event/{path}")]
        public void Post([FromUri]string path)
        {
            // write path to database
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class BridgeEvent
    {
        public BridgeEvent(string id, string databasePath)
        {
            Id = id;
            DatabasePath = databasePath;
        }

        public string Id { get; }
        public string DatabasePath { get; set; }
    }
}