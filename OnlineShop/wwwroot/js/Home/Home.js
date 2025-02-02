document.querySelectorAll('.category-card').forEach(function (card) {
    card.addEventListener('click', function () {
        var category = card.querySelector('h3').innerText.trim();
        var newUrl = `${baseUrl}/Shop?category=${category}&pg=1`;
        window.location.href = newUrl;
    });
});