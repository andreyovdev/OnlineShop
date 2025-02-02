//Displaying products in wishlist and cart
let wishlistCountElement = null;
let cartCountElement = null;

let wishlistCount = 0;
let cartCount = 0;

document.addEventListener("DOMContentLoaded", function () {

    wishlistCountElement = document.querySelector(".wishlist-count");
    cartCountElement = document.querySelector(".cart-count");

    if (!isUserAuthenticated) return;

        fetch('/Header/GetProductsInCartCount')
            .then(response => response.json())
            .then(data => {
                cartCountElement.textContent = `${data.productsInCart}`;
                cartCount = data.productsInCart;
                if (cartCount === 0) {
                    cartCountElement.style.display = 'none';
                } else {
                    cartCountElement.style.display = 'block';
                }
            })
            .catch(error => console.error('Error:', error));

        fetch('/Header/GetProductsInWishlistCount')
            .then(response => response.json())
            .then(data => {
                wishlistCountElement.textContent = `${data.productsInWishlist}`;
                wishlistCount = data.productsInWishlist;
                if (wishlistCount === 0) {
                    wishlistCountElement.style.display = 'none';
                } else {
                    wishlistCountElement.style.display = 'block';
                }
            })
            .catch(error => console.error('Error:', error));
    
});

//Searching
function updateSearchLink() {
    var searchInput = document.getElementById('searchInput').value;
    var newUrl = `${baseUrl}/Shop?input=${encodeURIComponent(searchInput)}&pg=1`;
    window.location.href = newUrl;
}

document.getElementById('searchInput').addEventListener('keydown', function (event) {
    if (event.key === 'Enter') { 
        event.preventDefault(); 
        updateSearchLink(); 
    }
});