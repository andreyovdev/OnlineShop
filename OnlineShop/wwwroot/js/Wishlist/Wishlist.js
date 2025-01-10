$(document).ready(function () {
    const $wishlistCardContainer = $('.wishlist-products-container');
    const $loadingMessage = $('.loading-message');
    const $noProductMessage = $('.no-product-message');
    const $pageHeading = $('.page-heading');

    let productsInWishlistCount = 0;

    fetchProducts();

    function renderWishlistCard(product) {

        let isInCart = false;

        if (isUserAuthenticated) {
            isInCart = isProductInCart(product.Id);
        }

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
                    <button class="cart-wishlist-btn">
                        <a class="cart-add" style="display: ${isInCart ? 'none' : 'inline-block'};">Add To Cart</a>
                        <a class="cart-added" href="/Cart/" style="display: ${isInCart ? 'inline-block' : 'none'};">View In Cart</a>
                     </button>
                    <button class="remove-from-wishlist-btn">Remove from Wishlist</button>
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
                $pageHeading.css('display', 'flex');
                if (!response.products || response.products.length === 0) {
                    $noProductMessage.css('display', 'flex');
                    $pageHeading.css('display', 'none');
                }
                

                response.products.forEach(product => {
                    $wishlistCardContainer.append(renderWishlistCard(product));
                });
                setupWishlistButtons();
                productsInWishlistCount = response.products.length;

            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }

    function isProductInCart(productId) {
        let isInCart = false;
        $.ajax({
            url: `/Cart/IsProductInCart?productId=${productId}`,
            type: 'GET',
            async: false,
            success: function (response) {
                isInCart = response;
            },
            error: function () {
                isInCart = false;
            }
        });
        return isInCart;
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
                    const cardElement = buttonElement.closest(`.wishlist-card`).remove();

                    productsInWishlistCount--;

                   

                    $noProductMessage.css('display', 'none');
                    $pageHeading.css('display', 'flex');
                    if (productsInWishlistCount <= 0) {
                        $noProductMessage.css('display', 'flex');
                        $pageHeading.css('display', 'none');
                    }

                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        });
    }
});
