﻿using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {
        private ClientModel ClientModel = ClientModel.Instance;

        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            ClientModel.Open(ip, port);
            if (ClientModel.IsConnected())
            {
                ViewBag.time = time;
            }
            return View();
        }

        // GET: save
        [HttpGet]
        public ActionResult save(string ip, int port, int second, int time, string name)
        {
            ClientModel.Open(ip, port);
            ClientModel.SaveToFile(name);

            Session["time"] = time;
            Session["second"] = second;
            return View();
        }

        public ActionResult DisplayFile(string name, int time)
        {
            Session["time"] = time;
            ClientModel.ReadFile(name);
            return View();

        }
        public ActionResult Def()
        {
            return View();
        }


        [HttpPost]
        public string GetLonLat()
        {
            string lon = ClientModel.Lon.ToString();
            string lat = ClientModel.Lat.ToString();
            ClientModel.point.Lon = lon;
            ClientModel.point.Lat = lat;
            return ToXml(ClientModel.point);
        }
        public string ToXml(Point point)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            point.ToXml(writer);
            writer.WriteElementString("Lon", point.Lon);
            writer.WriteElementString("Lat", point.Lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        [HttpPost]
        public string SaveToXML()
        {
            List<float> point = new List<float>();

            var lon = ClientModel.Instance.Lon;
            var lat = ClientModel.Instance.Lat;
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(builder, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            //TODO: Save all parameters
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return builder.ToString();
        }

    }
}
