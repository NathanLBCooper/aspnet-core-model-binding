using AliasModelBinder.ExampleApp.Client;
using Microsoft.AspNetCore.Mvc;

namespace AliasModelBinder.ExampleApp.Web.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("echo")]
        public ActionResult<int> Echo([FromQuery] EchoRequest request)
        {
            if (request == null)
            {
                return base.BadRequest();
            }

            return request.Number;
        }

        [HttpGet]
        [Route("echoCollection")]
        public ActionResult<int[]> EchoCollection([FromQuery] EchoCollectionRequest request)
        {
            if (request?.Integers == null)
            {
                return base.BadRequest();
            }

            return request.Integers;
        }

        [HttpGet]
        [Route("add")]
        public ActionResult<int> Add([FromQuery] AddRequest request)
        {
            if (request == null)
            {
                return base.BadRequest();
            }

            return request.LeftSummand + request.RightSummand;
        }
    }
}
