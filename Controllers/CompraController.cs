using Microsoft.AspNetCore.Mvc;
using ListaCompra.Models;
using ListaCompra.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListaCompra.Controllers
{
    public class CompraController : Controller
    {
        private CompraService service;

        public CompraController(CompraService service)
        {
            this.service = service;
        }



        public async Task<IActionResult> Comprar(string id, string categoria)
        {
            await this.service.ComprarItemsAsync(id);
            return Redirect("/Compra/IndexCategory/?categoria=" + categoria);
        }

        public async Task<IActionResult> Borrar(ItemCompra item)
        {
            await this.service.BorrarItemsAsync(item);
            return Redirect("/Compra/HistorialCategory/?categoria=" + item.Categoria);
        }

        public async Task<IActionResult> BorrarTodo(string categoria)
        {
            await this.service.BorrarTodoAsync(categoria);
            return Redirect("/Compra/HistorialCategory/?categoria=" + categoria);
        }





        public async Task<IActionResult> IndexCategory (string categoria)
        {
            List<ItemCompra> items = await this.service.GetItemsAsync(categoria);
            return View(items);
        }
        public async Task<IActionResult> HistorialCategory(string categoria)
        {
            List<ItemCompra> items = await this.service.GetItemsRemovedAsync(categoria);
            return View(items);
        }
        public IActionResult CreateItemForm(string categoria)
        {
            return View();
        }
        public async Task<IActionResult> CreateItem(ItemCompra item, string categoria)
        {
            await this.service.CreateItemsAsync(item.Nombre, item.Prioridad, item.Nota, categoria);
            return Redirect("/Compra/IndexCategory/?categoria=" + categoria);
        }

    }

}
