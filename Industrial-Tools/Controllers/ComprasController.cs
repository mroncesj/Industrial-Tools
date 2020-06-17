using Industrial_Tools.Models;
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
    public class ComprasController : Controller
    {
        GenericUnitToWork _uniToWork = new GenericUnitToWork();

        // GET: Compras
        public ActionResult HistorialCompras()
        {
            Usuarios current = (Usuarios)Session["usr"];
            if (current == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Ventas> ventasUsuario = _uniToWork.GetRepositoryInstance<Ventas>().GetListParameter(i => i.id_usuario == current.id).ToList();
            return View(ventasUsuario);
        }

        public ActionResult DetalleCompra(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["usr"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            DetalleCompraModel model = new DetalleCompraModel();
            model.Venta = _uniToWork.GetRepositoryInstance<Ventas>().GetFirstOrDefaultByParameter(i => i.id == id);
            List<Detalle_Ventas> listaProductos = _uniToWork.GetRepositoryInstance<Detalle_Ventas>().GetListParameter(i => i.id_venta == model.Venta.id).ToList();
            List<CarritoModel> productos = new List<CarritoModel>();

            foreach (Detalle_Ventas item in listaProductos)
            {
                Productos p = _uniToWork.GetRepositoryInstance<Productos>().GetFirstOrDefaultByParameter(i => i.id == item.id_producto);
                CarritoModel c = new CarritoModel()
                {
                    Cantidad = item.cantidad,
                    Precio = item.precio_venta,
                    Id = item.id_producto,
                    Nombre = p.nombre,
                    Img = p.img
                };

                productos.Add(c);
            }

            model.Productos = productos;

            return View(model);
        }
    }
}