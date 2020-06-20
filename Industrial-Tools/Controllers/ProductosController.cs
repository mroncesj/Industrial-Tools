using Industrial_Tools.Repository;
using Industrial_Tools.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//Productos
namespace Industrial_Tools.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        public GenericUnitToWork _unitOfWork = new GenericUnitToWork();
        // GET: Productos
        [AllowAnonymous]
        public ActionResult Productos()
        {
            return View(_unitOfWork.GetRepositoryInstance<Productos>().GetAllRecords().ToList());
        }

        [AllowAnonymous]
        public ActionResult InfoProducto(int? id)
        {
            if (id != null)
            {
                Productos producto = _unitOfWork.GetRepositoryInstance<Productos>().GetFirstOrDefaultByParameter(i => i.id == id);
                return View(producto);
            }
            return RedirectToAction("Productos");
        }

        [AllowAnonymous]
        public ActionResult ProductosCategoria(int? id)
        {
            if (id != null)
            {
                if (id > 0 && id < 6)
                {
                    List<Productos> prodCat = _unitOfWork.GetRepositoryInstance<Productos>().GetListParameter(i => i.id_categoria == id).ToList();
                    string[] categorias = { "Carpintería", "Hidraúlicas", "Jardinería", "Uso General", "Mecánica" };
                    int idc = id.Value;
                    ViewBag.Categoria = categorias[idc - 1];
                    return View(prodCat);
                }
                if (id == 6)
                {
                    ViewBag.Categoria = "Todos los productos";
                    return View(_unitOfWork.GetRepositoryInstance<Productos>().GetAllRecords().ToList());
                }
            }
            return RedirectToAction("Productos");
        }
    }
}