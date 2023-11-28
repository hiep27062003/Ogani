using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogani.Models;
using X.PagedList;

namespace Ogani.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QlbanHangThucPhamContext db=new QlbanHangThucPhamContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int? page)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi() 
        {
            ViewBag.MaChatLieu=new SelectList(db.TChatLieus.ToList(),"MaChatLieu","ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(TDanhMucSp sanPham)
        {
            if(ModelState.IsValid)
             {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanPham);
        }
        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSanPham)
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sanPham= db.TDanhMucSps.Find(maSanPham);
            return View(sanPham);
        }
        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham","HomeAdmin");
            }
            return View(sanPham);
        }
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            TempData["Message"] = "";
            var chiTietSanPhams=db.TChiTietSanPhams.Where(x=>x.MaSp==maSanPham).ToList();
            if (chiTietSanPhams.Count()>0)
            {
                TempData["Message"] = "Không xóa được sản phẩm này ";
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }
            var anhSanPhams=db.TAnhSps.Where(x=>x.MaSp == maSanPham).ToList();
            if (anhSanPhams.Any()) db.RemoveRange(anhSanPhams);
            db.Remove(db.TDanhMucSps.Find(maSanPham));
            db.SaveChanges() ;
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        }
        [Route("danhsachmenu")]
        public IActionResult DanhSachMenu()
        {
            var lstMenu=db.TLoaiSps.ToList();
            return View(lstMenu);
        }
        [Route("ThemMenuMoi")]
        [HttpGet]
        public IActionResult ThemMenuMoi()
        {
            return View();
        }
        [Route("ThemMenuMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemMenuMoi(TLoaiSp loaiSp)
        {
            if (ModelState.IsValid)
            {
                db.TLoaiSps.Add(loaiSp);
                db.SaveChanges();
                return RedirectToAction("DanhSachMenu");
            }
            return View(loaiSp);
        }
        [Route("SuaMenu")]
        [HttpGet]
        public IActionResult SuaMenu(string maLoaiMn)
        {
            var loaiSp = db.TLoaiSps.Find(maLoaiMn);
            return View(loaiSp);
        }
        [Route("SuaMenu")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaMenu(TLoaiSp loaiSp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiSp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachMenu","HomeAdmin");
            }
            return View(loaiSp);
        }
        [Route("XoaMenu")]
        [HttpGet]
        public IActionResult XoaMenu(string maLoaiMn)
        {
            TempData["Message"] = "";
            var DanhMucSps=db.TDanhMucSps.Where(x=>x.MaLoai==maLoaiMn).ToList();
            if (DanhMucSps.Count()>0)
            {
                TempData["Message"] = "Không được xóa Menu này";
                return RedirectToAction("DanhSachMenu", "HomeAdmin");
            }    
            db.Remove(db.TLoaiSps.Find(maLoaiMn));
            db.SaveChanges();
            TempData["Message"] = "Menu đã được xóa";
            return RedirectToAction("DanhSachMenu","HomeAdmin");
        }
        [Route("chitietsanpham")]
        public IActionResult ChiTietSanPham(int? page)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var ctsanpham=db.TChiTietSanPhams.AsNoTracking().OrderBy(x => x.AnhDaiDien);
            PagedList<TChiTietSanPham> lst = new PagedList<TChiTietSanPham>(ctsanpham, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemChiTietSanPham")]
        [HttpGet]
        public IActionResult ThemChiTietSanPham()
        {
            ViewBag.MaSp = new SelectList(db.TDanhMucSps.ToList(), "MaSp", "TenSp");
            ViewBag.MaKichThuoc = new SelectList(db.TKichThuocs.ToList(), "MaKichThuoc", "KichThuoc");
            return View();
        }
        [Route("ThemChiTietSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemChiTietSanPham(TChiTietSanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.TChiTietSanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("ChiTietSanPham");
            }
            return View(sanPham);
        }
        [Route("SuaChiTietSanPham")]
        [HttpGet]
        public IActionResult SuaChiTietSanPham(string maChiTietSanPham)
        {
            ViewBag.MaSp = new SelectList(db.TDanhMucSps.ToList(), "MaSp", "TenSp");
            ViewBag.MaKichThuoc = new SelectList(db.TKichThuocs.ToList(), "MaKichThuoc", "KichThuoc");
            var sanPham = db.TChiTietSanPhams.Find(maChiTietSanPham);
            return View(sanPham);
        }
        [Route("SuaChiTietSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaChiTietSanPham(TChiTietSanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ChiTietSanPham", "HomeAdmin");
            }
            return View(sanPham);
        }
        [Route("XoaChiTietSanPham")]
        [HttpGet]
        public IActionResult XoaChiTietSanPham(string maChiTietSanPham)
        {
            TempData["Message"] = "";
            var chiTietHoaDonBans = db.TChiTietHdbs.Where(x => x.MaChiTietSp == maChiTietSanPham).ToList();
            if (chiTietHoaDonBans.Count() > 0)
            {
                TempData["Message"] = "Không xóa được danh mục này ";
                return RedirectToAction("ChiTietSanPham", "HomeAdmin");
            }
            var chitietSanPhams = db.TDanhMucSps.Where(x => x.MaSp == maChiTietSanPham).ToList();
            if (chitietSanPhams.Any()) db.RemoveRange(chitietSanPhams);
            db.Remove(db.TChiTietSanPhams.Find(maChiTietSanPham));
            db.SaveChanges();
            TempData["Message"] = "Danh mục đã được xóa";
            return RedirectToAction("ChiTietSanPham", "HomeAdmin");
        }

    }
}
