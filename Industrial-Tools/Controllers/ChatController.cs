using Industrial_Tools.Models.DAL;
using Industrial_Tools.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Industrial_Tools.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        GenericUnitToWork _unitToWork = new GenericUnitToWork();
        // GET: Chat
        [HttpPost]
        public void MarcarLeidos()
        {
            int id = ((Usuarios)Session["usr"]).id;
            List<Chat> mensajes = _unitToWork.GetRepositoryInstance<Chat>().GetListParameter(i => i.id_usuario == id && i.status == 1 && i.tipo_mensaje == 1).ToList();
            foreach (var item in mensajes)
            {
                item.status = 0;
                _unitToWork.GetRepositoryInstance<Chat>().Update(item);
            }
        }

        public void EnviarMensaje(string mensaje)
        {
            int id = ((Usuarios)Session["usr"]).id;
            Chat nuevo = new Chat()
            {
                id = _unitToWork.GetRepositoryInstance<Chat>().GetLastRecord().id + 1,
                mensaje = mensaje,
                fecha = DateTime.Now,
                id_usuario = id,
                status = 1,
                tipo_mensaje = 0
            };
            _unitToWork.GetRepositoryInstance<Chat>().Add(nuevo);
        }

        [HttpPost]
        public void MarcarLeidosAdmin(int id)
        {
            List<Chat> mensajes = _unitToWork.GetRepositoryInstance<Chat>().GetListParameter(i => i.id_usuario == id && i.status == 1 && i.tipo_mensaje == 0).ToList();
            foreach (var item in mensajes)
            {
                item.status = 0;
                _unitToWork.GetRepositoryInstance<Chat>().Update(item);
            }
        }

        public void EnviarMensajeAdmin(string mensaje, int id)
        {
            Chat nuevo = new Chat()
            {
                id = _unitToWork.GetRepositoryInstance<Chat>().GetLastRecord().id + 1,
                mensaje = mensaje,
                fecha = DateTime.Now,
                id_usuario = id,
                status = 1,
                tipo_mensaje = 1
            };
            _unitToWork.GetRepositoryInstance<Chat>().Add(nuevo);
        }

        [HttpGet]
        public JsonResult GetMensajesUser(int id)
        {
            List<Chat> mensajes = _unitToWork.GetRepositoryInstance<Chat>().GetListParameter(i => i.id_usuario == id).ToList();
            mensajes.OrderBy(i => i.id);
            return Json(new { data = mensajes }, JsonRequestBehavior.AllowGet);
        }
    }
}