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

        public async Task<IActionResult> Index()
        {
            List<ItemCompra> items = await this.service.GetItemsAsync();
            return View(items);
        }

        public async Task<IActionResult> Historial()
        {
            List<ItemCompra> items = await this.service.GetItemsRemovedAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemCompra item)
        {
            await this.service.CreateItemsAsync(item.Nombre, item.Prioridad, item.Nota);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(ItemCompra item)
        {
            await this.service.DeleteItemsAsync(item);
            return RedirectToAction("Index");
        }

    }

}
