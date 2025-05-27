using System;
using System.Collections.Generic;
using System.Linq;

public class Book
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public int Quantity { get; set; }

    public Book(int id, string title, string author, string genre, int quantity)
    {
        ID = id;
        Title = title;
        Author = author;
        Genre = genre;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"[{ID}] {Title} - {Author} | Thể loại: {Genre} | Số lượng: {Quantity}";
    }
}

class Program
{
    static List<Book> books = new List<Book>();
    static Stack<string> lichSuMuonTra = new Stack<string>();
    static Dictionary<string, List<int>> dsNguoiMuon = new Dictionary<string, List<int>>();
    static Dictionary<int, Queue<string>> dsHangCho = new Dictionary<int, Queue<string>>();

    static void ThemSach(Book newBook)
    {
        var existing = books.FirstOrDefault(b => b.ID == newBook.ID);
        if (existing != null)
        {
            existing.Quantity += newBook.Quantity;
            Console.WriteLine("Sách đã tồn tại, cập nhật số lượng.");
        }
        else
        {
            books.Add(newBook);
            Console.WriteLine("Đã thêm sách mới.");
        }
    }

    static void TimKiemSach(string keyword, bool theoTacGia = false)
    {
        var ketQua = theoTacGia
            ? books.Where(b => b.Author.Contains(keyword)).ToList()
            : books.Where(b => b.Genre.Contains(keyword)).ToList();

        Console.WriteLine("Kết quả tìm kiếm:");
        ketQua.ForEach(Console.WriteLine);
    }

    static void PhanLoaiSach()
    {
        var phanLoai = books
            .GroupBy(b => b.Genre)
            .Select(g => new { TheLoai = g.Key, TongSo = g.Sum(b => b.Quantity) });

        Console.WriteLine("Tổng sách theo thể loại:");
        foreach (var item in phanLoai)
        {
            Console.WriteLine($"{item.TheLoai}: {item.TongSo} cuốn");
        }
    }

    static void XuLyMuonSach(int bookId)
    {
        var book = books.FirstOrDefault(b => b.ID == bookId);
        if (book != null && book.Quantity > 0)
        {
            book.Quantity--;
            lichSuMuonTra.Push($"Mượn: [{bookId}]");
            Console.WriteLine($"Mượn sách thành công: {book.Title}");
        }
        else
        {
            Console.WriteLine("Không thể mượn. Sách đã hết hoặc không tồn tại.");
            if (!dsHangCho.ContainsKey(bookId))
                dsHangCho[bookId] = new Queue<string>();
        }
    }

    static void XuLyTraSach(int bookId)
    {
        var book = books.FirstOrDefault(b => b.ID == bookId);
        if (book != null)
        {
            book.Quantity++;
            lichSuMuonTra.Push($"Trả: [{bookId}]");
            Console.WriteLine($"Trả sách thành công: {book.Title}");

            if (dsHangCho.ContainsKey(bookId) && dsHangCho[bookId].Count > 0)
            {
                string nguoiMuon = dsHangCho[bookId].Dequeue();
                Console.WriteLine($"=> Sách được tự động mượn bởi người đang chờ: {nguoiMuon}");
                XuLyMuonChoNguoiDung(nguoiMuon, bookId);
            }
        }
    }

    static void XuLyMuonChoNguoiDung(string ten, int bookId)
    {
        var book = books.FirstOrDefault(b => b.ID == bookId);
        if (book == null || book.Quantity == 0)
        {
            Console.WriteLine("Không thể mượn, thêm vào hàng chờ.");
            if (!dsHangCho.ContainsKey(bookId))
                dsHangCho[bookId] = new Queue<string>();
            dsHangCho[bookId].Enqueue(ten);
            return;
        }

        book.Quantity--;
        lichSuMuonTra.Push($"Mượn: [{bookId}] bởi {ten}");

        if (!dsNguoiMuon.ContainsKey(ten))
            dsNguoiMuon[ten] = new List<int>();
        dsNguoiMuon[ten].Add(bookId);
    }

    static void HienThiNguoiDangMuon()
    {
        var ketQua = dsNguoiMuon
            .SelectMany(u => u.Value.Select(bookId => new
            {
                TenNguoi = u.Key,
                Sach = books.FirstOrDefault(b => b.ID == bookId)?.Title ?? "Không rõ"
            }));

        Console.WriteLine("Danh sách người đang mượn:");
        foreach (var item in ketQua)
            Console.WriteLine($"{item.TenNguoi} đang mượn: {item.Sach}");
    }

    static void HienThiHangCho()
    {
        Console.WriteLine("Hàng chờ mượn:");
        foreach (var entry in dsHangCho)
        {
            if (entry.Value.Count > 0)
            {
                Console.WriteLine($"Sách ID {entry.Key}:");
                foreach (var ten in entry.Value)
                    Console.WriteLine($"- {ten}");
            }
        }
    }

    static void HienThiTopSach()
    {
        var top = books.OrderByDescending(b => b.Quantity).Take(3);
        Console.WriteLine("Top 3 sách nhiều nhất:");
        foreach (var b in top)
            Console.WriteLine(b);
    }

    static void ThongKeTheLoai()
    {
        HashSet<string> theLoais = new HashSet<string>(books.Select(b => b.Genre));
        Console.WriteLine("Thể loại sách hiện có:");
        foreach (var g in theLoais)
            Console.WriteLine($"- {g}");
    }

    static void Main()
    {
        ThemSach(new Book(1, "Dế Mèn Phiêu Lưu Ký", "Tô Hoài", "Thiếu nhi", 15));
        ThemSach(new Book(2, "Truyện Kiều", "Nguyễn Du", "Văn học", 22));
        ThemSach(new Book(3, "Cho tôi xin một vé đi tuổi thơ", "Nguyễn Nhật Ánh", "Truyện ngắn", 20));

        TimKiemSach("Nguyễn Du", true);
        PhanLoaiSach();
        XuLyMuonChoNguoiDung("An", 2);
        XuLyTraSach(2);
        HienThiNguoiDangMuon();
        HienThiTopSach();
        ThongKeTheLoai();
        HienThiHangCho();
    }
}