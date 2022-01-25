using Application.mvc.Data;
using Application.mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.mvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // armazena a lista de categorias
            var categoryList = _context.Categories.ToList();

            // envia a lista de categorias através da View
            return View(categoryList);
        }

        // GET - Tela de cadastro
        public IActionResult Create()
        {
            return View();
        }

        // POST - Cadastra a Categoria
        [HttpPost]
        // Previne Cross Site Request Forgery
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            // verifica existe alguma categoria com o mesmo nome da categoria que está sendo cadastrada
            if (!!_context.Categories.Any(c => c.Name == category.Name))
            {
                // AddModelError - Adiciona uma mensagem de erro personalizada para a model
                // A mensagem de erro exibibida para a propriedade "Nome" que está na model será
                // "This category already exists"
                ModelState.AddModelError("Name", "This category already exists");
            }

            // verifica se a model recebida é válida
            if (ModelState.IsValid)
            {
                // se for valido, faz o cadastro

                _context.Categories.Add(category);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // se não for, retorna para a própria tela de cadastro e envia a model para ela
            return View(category);
        }

        // GET - Tela de Editar
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST - Editar Categoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST - Excluir Categoria
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            };

            _context.Categories.Remove(category);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
