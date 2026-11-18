namespace SanLichWeb.Models
{
    [System.Serializable]
    public class NguoiDung
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string VaiTro { get; set; }
        public bool LaQuanLy { get { return VaiTro == "MANAGER"; } }
    }
}
