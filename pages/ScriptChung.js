// Chỉ đánh dấu mục điều hướng. Các điều kiện nghiệp vụ luôn kiểm tra ở C#.
document.addEventListener("DOMContentLoaded", function () {
    var links = document.querySelectorAll(".nav a");

    links.forEach(function (link) {
        if (link.pathname.toLowerCase() === window.location.pathname.toLowerCase()) {
            link.classList.add("active");
            link.setAttribute("aria-current", "page");
        }
    });
});
