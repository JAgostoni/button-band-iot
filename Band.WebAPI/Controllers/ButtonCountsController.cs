using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Band.WebAPI.Controllers
{
    public class ButtonCountsController : ApiController
    {


        public static ButtonData _DATA = new ButtonData() { Blue = 5, Green = 4, Red = 3, Yellow = 2};

        [HttpGet]
        public IHttpActionResult Get()
        {
            _DATA.Yellow++;
            return Ok(_DATA);

        }
    }


    public class ButtonData
    {
        public long Red { get; set; }
        public long Green { get; set; }
        public long Blue { get; set; }
        public long Yellow { get; set; }
    }
}
