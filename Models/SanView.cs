using System.Collections.Generic;
namespace SanLichWeb.Models
{
    public class SanView
    {
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public List<GioView> Gio { get; set; }
    }
    public class GioView
    {
        public int Id { get; set; }
        public string Nhan { get; set; }
        public string Gia { get; set; }
        public bool ConTrong { get; set; }
        public string LyDo { get; set; }
    }
}
