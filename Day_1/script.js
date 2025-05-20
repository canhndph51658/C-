// Danh sách bánh có sẵn
const menu = [
  { ten: "Bánh Mì", gia: 10000 },
  { ten: "Bánh Kem", gia: 50000 },
  { ten: "Bánh Su", gia: 15000 },
  { ten: "Bánh Cuộn", gia: 20000 }
];

// Thống kê
let tongDonHang = 0;
let tongDoanhThu = 0;
let donLon = 0;
let donThuong = 0;

// DOM
const tenBanhSelect = document.getElementById("tenBanh");
const soLuongInput = document.getElementById("soLuong");
const orderForm = document.getElementById("orderForm");
const orderList = document.getElementById("orderList");

const totalOrders = document.getElementById("totalOrders");
const totalRevenue = document.getElementById("totalRevenue");
const bigOrders = document.getElementById("bigOrders");
const normalOrders = document.getElementById("normalOrders");

// Load menu vào select
menu.forEach((item, index) => {
  const option = document.createElement("option");
  option.value = index;
  option.text = `${item.ten} - ${item.gia.toLocaleString()} VND`;
  tenBanhSelect.appendChild(option);
});

// Xử lý đơn hàng
orderForm.addEventListener("submit", function (e) {
  e.preventDefault();

  const index = parseInt(tenBanhSelect.value);
  const soLuong = parseInt(soLuongInput.value);

  if (isNaN(index) || isNaN(soLuong) || soLuong <= 0) return;

  const banh = menu[index];
  const tongTien = TinhTien(banh.gia, soLuong);
  const loaiDon = XepLoaiDon(tongTien);

  tongDonHang++;
  tongDoanhThu += tongTien;
  loaiDon === "Đơn lớn" ? donLon++ : donThuong++;

  HienThiThongTin(banh.ten, soLuong, tongTien, loaiDon);
  CapNhatThongKe();

  orderForm.reset();
  tenBanhSelect.focus();
});

// Method overload (giá + số lượng)
function TinhTien(gia, soLuong) {
  return gia * soLuong;
}

// Xếp loại đơn
function XepLoaiDon(tongTien) {
  return tongTien > 100000 ? "Đơn lớn" : "Đơn thường";
}

// Hiển thị thông tin đơn hàng
function HienThiThongTin(ten, soLuong, tongTien, loaiDon) {
  const card = document.createElement("div");
  card.className = "card mb-3";
  card.innerHTML = `
    <div class="card-body">
      <h5 class="card-title">${ten} (${soLuong} cái)</h5>
      <p class="card-text">💰 Tổng tiền: ${tongTien.toLocaleString()} VND</p>
      <p class="card-text">📦 Loại đơn: <strong>${loaiDon}</strong></p>
    </div>
  `;
  orderList.prepend(card);
}

// Cập nhật thống kê
function CapNhatThongKe() {
  totalOrders.textContent = tongDonHang;
  totalRevenue.textContent = tongDoanhThu.toLocaleString();
  bigOrders.textContent = donLon;
  normalOrders.textContent = donThuong;
}
