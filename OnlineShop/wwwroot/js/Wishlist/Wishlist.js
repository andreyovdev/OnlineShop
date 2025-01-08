$(document).ready(function () {
    const $wishlistCardContainer = $('.product-card-container');
    const $loadingMessage = $('.loading-message');
    const $noProductMessage = $('.no-product-message');
    const $pageHeading = $('.page-heading');

    fetchProducts();

    function renderWishlistCard(product) {
        return `
            <div class="wishlist-card" data-id="${product.Id}">
                <div>
                    <img class="img-container" alt="${product.Name}" src="${product.ImgUrl}">
                </div>
                <div class="product-card-details">
                    <h3>${product.Name}</h3>
                    <div class="price-container">
                        <p class="price">$${product.Price}</p>
                    </div>
                    <p>Type: ${product.Category}</p>
                    <div class="info"></div>
                </div>
                <div class="wishlist-btn-container">
                    <button class="cart-wishlist-btn" onclick="addToCart(${product.Id})">Add to Cart</button>
                    <button class="remove-from-wishlist-btn" onclick="removeFromWishlist(${product.Id})">Remove from Wishlist</button>
                </div>
            </div>
        `;
    }


    function fetchProducts() {

        if (!isUserAuthenticated) {
            console.log("not authed")
            $noProductMessage.css('display', 'flex');
            return;
        }

        $.ajax({
            url: '/Wishlist/AllProductsInWishlist',
            type: 'POST',
            contentType: 'application/json',
            beforeSend: () => { $loadingMessage.css('display', 'flex'); },
            complete: () => { $loadingMessage.css('display', 'none'); },
            success: function (response) {
                $wishlistCardContainer.children().not($noProductMessage).remove();

                $noProductMessage.css('display', 'none');
                $pageHeading.css('display', 'none');
                if (!response.products || response.products.length === 0) {
                    $noProductMessage.css('display', 'flex');
                    $pageHeading.css('display', 'none');
                }
                

                response.products.forEach(product => {
                    $wishlistCardContainer.append(renderWishlistCard(product));
                });
                setupWishlistButtons();
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }

    function setupWishlistButtons() {
        $('.remove-from-wishlist-btn').on('click', function () {
            const buttonElement = $(this);
            const productId = buttonElement.closest('.wishlist-card').data('id'); 

            $.ajax({
                url: '/Wishlist/RemoveFromWishlist',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productId),
                success: function () {
                    buttonElement.closest('.wishlist-card').remove();
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        });
    }
});
