using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Lab1.Models;

namespace Lab1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private List<Student> listStudents = new List<Student>();

        public StudentController(IWebHostEnvironment env)
        {
            _env = env;
            listStudents = new List<Student>()
            {
                new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                    Gender = Gender.Male, IsRegular = true,
                    Address = "A1-2018", Email = "nam@g.com" },
                new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE,
                    Gender = Gender.Female, IsRegular = true,
                    Address = "A1-2019", Email = "tu@g.com" },
                new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                    Gender = Gender.Male, IsRegular = false,
                    Address = "A1-2020", Email = "phong@g.com" },
                new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                    Gender = Gender.Female, IsRegular = false,
                    Address = "A1-2021", Email = "mai@g.com" }
            };
        }
        [Route("Admin/Student/List", Name = "student-list")]
        public IActionResult Index()
        {
            return View(listStudents);
        }
        [HttpGet("Admin/Student/Add", Name = "student-add")]
        public IActionResult Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
                    return View();
                }

        [HttpPost("Admin/Student/Add")]
        public async Task<IActionResult> Create(Student s, IFormFile? avatar)
        {
            s.Id = listStudents.Last<Student>().Id + 1;

            if (avatar != null && avatar.Length > 0)
            {
                // Thư mục lưu ảnh: wwwroot/images
                string folder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(folder);

                // Đặt tên file mới để không bị trùng
                string fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                s.ImagePath = "/images/" + fileName;
            }

            listStudents.Add(s);
            return View("Index", listStudents);
        }
    }
}