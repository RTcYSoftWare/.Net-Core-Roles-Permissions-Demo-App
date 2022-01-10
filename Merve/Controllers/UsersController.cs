using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Merve.DataAccess;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Merve.Controllers
{
    public class UsersController : Controller
    {
        private readonly DbTContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(DbTContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // kullanıcıları listeler
        // GET: Users
        public async Task<IActionResult> Index()
        {
            // cookie' den giriş yapmış kullanıcıyı okur varsa
            string remember = cookieOku("admin");

            if (remember != "0")
            {
                // cookie' den okuduğu kullanıcı veri tabanında var mı kontrol eder
                var checkUser = _context.Users.Where(x => x.UserId == remember).AsNoTracking().Include(x => x.UserRoles).FirstOrDefault();
                if (checkUser != null)
                {
                    // giriş yapmış kullanıcının izinlerini vt'den alır
                    var userRoles = (from u in _context.Users
                                     join u_r in _context.UserRoles on u.UserId equals u_r.UserId
                                     join r_p in _context.RolePermissions on u_r.RoleId equals r_p.RoleId
                                     join p in _context.Permissions on r_p.PermissionId equals p.PermissionId
                                     where u.UserId == checkUser.UserId
                                     select new
                                     {
                                         p
                                     }).ToList();

                    // giriş yapmış kullanıcının izinlerini arasında gerekli izin varmı kontrol eder
                    if (userRoles.Where(x=>x.p.PermissionId == "USER_ACCOUNT_READ_PAGE").FirstOrDefault() != null)
                    {
                        return View(await _context.Users.ToListAsync());
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
            return View(await _context.Users.ToListAsync());
        }


        // kullanıcının detayına gider.
        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // kullanıcının kullanıcının verileri vt'den çeker
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            // indexteki işlemler ile aynı
            string remember = cookieOku("admin");

            if (remember != "0")
            {
                var checkUser = _context.Users.Where(x => x.UserId == remember).AsNoTracking().Include(x => x.UserRoles).FirstOrDefault();
                if (checkUser != null)
                {
                    var userRoles = (from u in _context.Users
                                     join u_r in _context.UserRoles on u.UserId equals u_r.UserId
                                     join r_p in _context.RolePermissions on u_r.RoleId equals r_p.RoleId
                                     join p in _context.Permissions on r_p.PermissionId equals p.PermissionId
                                     where u.UserId == checkUser.UserId
                                     select new
                                     {
                                         p
                                     }).ToList();

                    if (userRoles.Where(x => x.p.PermissionId == "USER_ACCOUNT_UPDATE_PAGE").FirstOrDefault() != null)
                    {
                        return View(user);
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }


            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            //indexteki işlemler ile aynı
            string remember = cookieOku("admin");

            if (remember != "0")
            {
                var checkUser = _context.Users.Where(x => x.UserId == remember).AsNoTracking().Include(x => x.UserRoles).FirstOrDefault();
                if (checkUser != null)
                {
                    var userRoles = (from u in _context.Users
                                     join u_r in _context.UserRoles on u.UserId equals u_r.UserId
                                     join r_p in _context.RolePermissions on u_r.RoleId equals r_p.RoleId
                                     join p in _context.Permissions on r_p.PermissionId equals p.PermissionId
                                     where u.UserId == checkUser.UserId
                                     select new
                                     {
                                         p
                                     }).ToList();

                    if (userRoles.Where(x => x.p.PermissionId == "USER_ACCOUNT_CREATE_PAGE").FirstOrDefault() != null)
                    {
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // kullanıcı 'yı vt.Ye kaydeder.
            user.CreatedAt = DateTime.Now;
            user.UserId = "0";
            //user.status = Enums.USER_STATUS.ACTIVE;
            //[Bind("UserId,Email,Password,SecondaryEmail,Gsm,Firstname,Lastname,Locale,Timezone,CreatedAt,UpdatedAt")]
            if (ModelState.IsValid)
            {

            }
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            return View(user);
        }

        // kullanıcı login sayfasına yönlendirir.
        public IActionResult Login()
        {
            string remember = cookieOku("admin");

            if (remember != "0")
            {
                var checkUser = _context.Users.Where(x => x.UserId == remember).AsNoTracking().Include(x => x.UserRoles).FirstOrDefault();
                if (checkUser != null)
                {
                    if (checkUser.UserRoles.Where(x => x.RoleId == "Root") != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }

        // kullanıcı login işlemlerini gerçekleştirir.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var checkUser = _context.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (checkUser != null)
            {
                cookieYaz("admin", checkUser.UserId, 10);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
            return View(user);
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            string remember = cookieOku("admin");

            if (remember != "0")
            {
                var checkUser = _context.Users.Where(x => x.UserId == remember).AsNoTracking().Include(x => x.UserRoles).FirstOrDefault();
                if (checkUser != null)
                {
                    var userRoles = (from u in _context.Users
                                     join u_r in _context.UserRoles on u.UserId equals u_r.UserId
                                     join r_p in _context.RolePermissions on u_r.RoleId equals r_p.RoleId
                                     join p in _context.Permissions on r_p.PermissionId equals p.PermissionId
                                     where u.UserId == checkUser.UserId
                                     select new
                                     {
                                         p
                                     }).ToList();

                    if (userRoles.Where(x => x.p.PermissionId == "USER_ACCOUNT_UPDATE_PAGE").FirstOrDefault() != null)
                    {
                        return View(user);
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }


            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,Email,Password,SecondaryEmail,Gsm,Firstname,Lastname,Locale,Timezone,CreatedAt,UpdatedAt")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // kullanıcı siler
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            string remember = cookieOku("admin");

            if (remember != "0")
            {
                var checkUser = _context.Users.Where(x => x.UserId == remember).AsNoTracking().Include(x => x.UserRoles).FirstOrDefault();
                if (checkUser != null)
                {
                    var userRoles = (from u in _context.Users
                                     join u_r in _context.UserRoles on u.UserId equals u_r.UserId
                                     join r_p in _context.RolePermissions on u_r.RoleId equals r_p.RoleId
                                     join p in _context.Permissions on r_p.PermissionId equals p.PermissionId
                                     where u.UserId == checkUser.UserId
                                     select new
                                     {
                                         p
                                     }).ToList();

                    if (userRoles.Where(x => x.p.PermissionId == "USER_ACCOUNT_DELETE_PAGE").FirstOrDefault() != null)
                    {
                        return View(user);
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        // coocie ye veri yazan fonksiyon
        public void cookieYaz(string cookieAdi, string cookieValue, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                option.IsEssential = true;
            }
                
            else
            {
                option.Expires = DateTime.Now.AddMilliseconds(60);
                option.IsEssential = true;
            }
               
            Response.Cookies.Append(cookieAdi, cookieValue, option);
        }

        // coocie den veri okuyan fonksiyon
        public string cookieOku(string cookieAdi)
        {
            string deneme = _httpContextAccessor.HttpContext.Request.Cookies[cookieAdi];
            string sonuc = "0";
            if (deneme != null) { sonuc = deneme.ToString(); }
            return sonuc;
        }
    }
}
