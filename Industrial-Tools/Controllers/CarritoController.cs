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
    public class CarritoController : Controller
    {
        GenericUnitToWork _unitOfWork = new GenericUnitToWork();

        // GET: Carrito
        [AllowAnonymous]
        public ActionResult Carrito()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AgregarProducto(int id)
        {
            CarritoModel item = new CarritoModel();
            Productos p = _unitOfWork.GetRepositoryInstance<Productos>().GetFirstOrDefaultByParameter(i => i.id == id);
            if (Session["carrito"] == null)
            {
                List<CarritoModel> carrito = new List<CarritoModel>();
                if (p != null)
                {
                    item = new CarritoModel
                    {
                        Precio = p.precio_venta,
                        Id = p.id,
                        Nombre = p.nombre,
                        Img = p.img,
                        Cantidad = 1
                    };
                }
                carrito.Add(item);
                Session["carrito"] = carrito;
            }
            else
            {
                List<CarritoModel> carrito = (List<CarritoModel>)Session["carrito"];
                int index = Exists(id);
                if (index != -1)
                {
                    if (carrito[index].Cantidad + 1 > p.cantidad)
                    {
                        Session["error"] = "No se puede agregar al carrito, la tienda no cuenta con suficiente cantidad de este producto.";
                        return RedirectToAction("InfoProducto", "Productos", new { id });
                    }
                    carrito[index].Cantidad++;
                }
                else
                {
                    item = new CarritoModel
                    {
                        Precio = p.precio_venta,
                        Id = p.id,
                        Nombre = p.nombre,
                        Img = p.img,
                        Cantidad = 1
                    };
                    carrito.Add(item);
                }
                Session["carrito"] = carrito;
            }
            Session["success"] = "Se ha agregado al carrito exitosamente.";
            return RedirectToAction("InfoProducto", "Productos", new { id });
        }

        private int Exists(int id)
        {
            List<CarritoModel> carrito = (List<CarritoModel>)Session["carrito"];
            for (int i = 0; i < carrito.Count(); i++)
            {
                if (carrito[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        [AllowAnonymous]
        public ActionResult DropAll(int id)
        {
            List<CarritoModel> carrito = (List<CarritoModel>)Session["carrito"];
            int index = Exists(id);
            carrito.RemoveAt(index);
            Session["carrito"] = carrito.Count() > 0 ? carrito : null;
            return RedirectToAction("Carrito");
        }

        [AllowAnonymous]
        public ActionResult DropOne(int id)
        {
            List<CarritoModel> carrito = (List<CarritoModel>)Session["carrito"];
            int index = Exists(id);
            if (carrito[index].Cantidad == 1)
            {
                DropAll(id);
            }
            else
            {
                carrito[index].Cantidad--;
            }
            Session["carrito"] = carrito.Count() > 0 ? carrito : null;
            return RedirectToAction("Carrito");
        }

    }
}