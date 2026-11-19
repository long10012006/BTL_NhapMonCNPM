<%@ Page Title="Quản lý sân" Language="C#" AutoEventWireup="true" CodeBehind="QLSan.aspx.cs" Inherits="SanLichWeb.pages.QLSan.QLSan" MasterPageFile="~/pages/master/Site.Master" %>
<asp:Content ID="NoiDung" ContentPlaceHolderID="MainHolder" runat="server">
    <div class="page-head">
        <div>
            <p class="eyebrow">
                SÂN LỊCH
            </p>
            <h1>
                Quản lý sân
            </h1>
            <p class="subtitle">
                Thêm, sửa hoặc ẩn sân. Sân ẩn giữ nguyên các lượt đã xác nhận và lịch sử.
            </p>
        </div>
    </div>
    <asp:Label ID="lblThongBao" runat="server" CssClass="message" role="status" />
    <section class="card">
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtMa" Text="Mã sân (ví dụ SAN09)" />
            <asp:TextBox ID="txtMa" runat="server" MaxLength="20" />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtTen" Text="Tên sân" />
            <asp:TextBox ID="txtTen" runat="server" MaxLength="100" />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtMoTa" Text="Mô tả" />
            <asp:TextBox ID="txtMoTa" runat="server" TextMode="MultiLine" MaxLength="500" />
        </div>
        <p>
            <asp:CheckBox ID="chkAn" runat="server" Text="Ẩn sân với lượt đặt mới" />
        </p>
        <asp:Button ID="btnLuu" runat="server" Text="Lưu sân" CssClass="btn" OnClick="btnLuu_Click"  />
        <asp:Button ID="btnMoi" runat="server" Text="Nhập sân mới" CssClass="btn" OnClick="btnMoi_Click"  />
    </section>
    <div class="table-wrap">
        <asp:GridView
            ID="gvSan"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="data-table"
            EmptyDataText="Không có dữ liệu."
            GridLines="None"
            OnRowCommand="gvSan_RowCommand" >
            <Columns>
                <asp:BoundField DataField="court_code" HeaderText="Mã" />
                <asp:BoundField DataField="court_name" HeaderText="Tên sân" />
                <asp:BoundField DataField="description" HeaderText="Mô tả" />
                <asp:TemplateField HeaderText="Đang ẩn">
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("is_hidden")) == 1 ? "Đang ẩn" : "Hiển thị" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="Sửa" CommandName="SuaSan" CommandArgument='<%# Eval("court_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
