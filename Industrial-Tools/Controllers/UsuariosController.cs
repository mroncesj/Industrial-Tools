using Industrial_Tools.Models;
using Industrial_Tools.Models.DAL;
using Industrial_Tools.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Industrial_Tools.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        public GenericUnitToWork _unitOfWork = new GenericUnitToWork();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // GET: Usuarios
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.correo, Email = model.correo };
                var result = await UserManager.CreateAsync(user, model.pass);

                if (result.Succeeded)
                {
                    Usuarios newUser = new Usuarios();
                    Usuarios lastUser = _unitOfWork.GetRepositoryInstance<Usuarios>().GetLastRecord();
                    if (lastUser != null)
                    {
                        newUser.id = lastUser.id + 1;
                    }
                    newUser.correo = model.correo;
                    newUser.username = model.username;
                    newUser.role_id = 1;
                    newUser.nombre = model.nombre;
                    newUser.apellido_paterno = model.apellido_paterno;
                    newUser.apellido_materno = model.apellido_materno;

                    Session["usr"] = newUser;

                    newUser.telefono = model.telefono;
                    newUser.pass = model.pass;
                    newUser.status = 1;

                    newUser.fecha_nacimiento = model.fecha_nacimiento;

                    _unitOfWork.GetRepositoryInstance<Usuarios>().Add(newUser);

                    Chat nuevo = new Chat()
                    {
                        id = _unitOfWork.GetRepositoryInstance<Chat>().GetLastRecord().id + 1,
                        mensaje = "¡Hola! bienvenido(a) a Industrial-Tools, ¿En que podemos ayudarte?",
                        fecha = DateTime.Now,
                        id_usuario = newUser.id,
                        status = 1,
                        tipo_mensaje = 1
                    };

                    _unitOfWork.GetRepositoryInstance<Chat>().Add(nuevo);

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
                Session["usr"] = null;
            }
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult Perfil()
        {
            Usuarios actual = (Usuarios)Session["usr"];
            if (actual == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Usuarios data = _unitOfWork.GetRepositoryInstance<Usuarios>().GetFirstOrDefaultByParameter(i => i.id == actual.id);

            UsuarioModelEdit edit = new UsuarioModelEdit
            {
                id = data.id,
                nombre = data.nombre,
                apellido_materno = data.apellido_materno,
                apellido_paterno = data.apellido_paterno,
                fecha_nacimiento = data.fecha_nacimiento,
                username = data.username,
                telefono = data.telefono,
                correo = data.correo
            };

            return View(edit);
        }

        [HttpPost]
        public ActionResult EditarPerfil(UsuarioModelEdit model)
        {
            if (ModelState.IsValid)
            {
                Usuarios newUser = _unitOfWork.GetRepositoryInstance<Usuarios>().GetFirstOrDefaultByParameter(i => i.id == model.id);
                //newUser.correo = model.correo;
                newUser.username = model.username;
                newUser.nombre = model.nombre;
                newUser.apellido_paterno = model.apellido_paterno;
                newUser.apellido_materno = model.apellido_materno;

                Session["usr"] = newUser;

                newUser.telefono = model.telefono;
                newUser.fecha_nacimiento = model.fecha_nacimiento;
                _unitOfWork.GetRepositoryInstance<Usuarios>().Update(newUser);
                Session["secc"] = "Se han guardado correctamente los datos.";
            }
            return RedirectToAction("Perfil");
        }

    }
}