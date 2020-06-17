using Industrial_Tools.Models;
using Industrial_Tools.Models.DAL;
using Industrial_Tools.Repository;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Industrial_Tools.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        GenericUnitToWork _unitToWork = new GenericUnitToWork();

        // GET: Payment
        public ActionResult PaymentWithPayPal(string Cancel = null)
        {
            if (Session["usr"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            APIContext apiContext = PayPalConfig.GetAPIContext();

            try
            {
                string PayerId = Request.Params["PayerId"];
                if (string.IsNullOrEmpty(PayerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority
                        + "/Payment/PaymentWithPayPal?";

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apiContext, PayerId, Session[guid] as string);

                    if (executePayment.state.ToLower() != "approved")
                    {
                        ViewBag.ErrorMessage = executePayment.failure_reason;
                        return View("FailureView");
                    }
                }
            }
            catch (Exception x)
            {
                ViewBag.ErrorMessage = x.Message;
                return View("FailureView");
            }
            RegistrarVenta();
            return View("SuccessView");
        }

        private Payment Payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.Payment = new Payment() { id = paymentId };
            return this.Payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            List<Item> itemsPayPal = new List<Item>();
            List<CarritoModel> carrito = (List<CarritoModel>)Session["carrito"];
            foreach (CarritoModel itemCarrito in carrito)
            {
                Item item = new Item()
                {
                    name = itemCarrito.Nombre.ToString(),
                    currency = "MXN",
                    price = itemCarrito.Precio.ToString(),
                    quantity = itemCarrito.Cantidad.ToString(),
                    sku = itemCarrito.Id.ToString()
                };
                itemsPayPal.Add(item);
            }

            double subtotal = (double)carrito.Sum(i => i.Precio * i.Cantidad);

            var itemList = new ItemList() { items = itemsPayPal };

            var payer = new Payer() { payment_method = "paypal" };

            var redirectUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = subtotal.ToString()
            };

            var amount = new Amount()
            {
                currency = "MXN",
                details = details,
                total = subtotal.ToString()
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Detalle de la compra",
                invoice_number = "#" + Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });

            Payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls
            };

            return this.Payment.Create(apiContext);
        }

        public ViewResult FailureView()
        {
            return View();
        }

        public ViewResult SuccessView()
        {
            return View();
        }

        public void RegistrarVenta()
        {
            List<CarritoModel> carrito = (List<CarritoModel>)Session["carrito"];
            Ventas venta = new Ventas();
            Detalle_Ventas detalle = new Detalle_Ventas();
            Ventas lastVenta = _unitToWork.GetRepositoryInstance<Ventas>().GetLastRecord();

            if (lastVenta != null)
            {
                venta.id = lastVenta.id + 1;
            }

            venta.fecha = DateTime.Today;
            venta.id_usuario = ((Usuarios)Session["usr"]).id;
            venta.iva = 0;
            venta.sub_total = carrito.Sum(i => i.Cantidad * i.Precio);
            venta.total = venta.sub_total;

            _unitToWork.GetRepositoryInstance<Ventas>().Add(venta);

            foreach (CarritoModel item in carrito)
            {
                Detalle_Ventas d = _unitToWork.GetRepositoryInstance<Detalle_Ventas>().GetLastRecord();
                if (d != null)
                {
                    detalle.id = d.id + 1;
                }

                Productos p = _unitToWork.GetRepositoryInstance<Productos>().GetFirstOrDefaultByParameter(i => i.id == item.Id);

                detalle.id_producto = item.Id;
                detalle.id_venta = venta.id;
                detalle.precio_venta = item.Precio;
                detalle.precio_compra = p.precio_compra;
                detalle.stat = 1;
                detalle.cantidad = item.Cantidad;

                _unitToWork.GetRepositoryInstance<Detalle_Ventas>().Add(detalle);

                int cantidad = p.cantidad - item.Cantidad;

                p.cantidad = cantidad;

                _unitToWork.GetRepositoryInstance<Productos>().Update(p);

                detalle = new Detalle_Ventas();
            }
            Session["carrito"] = null;
        }
    }
}